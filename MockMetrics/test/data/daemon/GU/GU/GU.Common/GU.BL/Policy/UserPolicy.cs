using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.DAException;
using Common.DA.Interface;
using GU.BL.DataMapping;
using GU.BL.Extensions;
using GU.DataModel;
using Common.Types.Exceptions;

namespace GU.BL.Policy
{
    public static class UserPolicy
    {
        public static DbUser GetUser(string name, IDomainDbManager dbManager, IDictionaryManager dictionaryManager)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new GUException("Не указано имя пользователя");

            var userId = (from u in dbManager.GetDomainTable<DbUser>()
                          where u.Name == name.ToUpper()
                          select u.Id).SingleOrDefault();

            if (userId == 0)
                throw new GUException("Пользователь не заведен в системе.");

            var dm = new DbUserDataMapper(new StubDomainContext(dbManager), dictionaryManager);
            var user = dm.Retrieve(userId, dbManager);

            return user;
        }

        public static int? GetUserAgencyId(string userName, IDomainDbManager dbManager)
        {
            var id = (from u in dbManager.GetDomainTable<DbUser>()
                      where u.Name == userName.ToUpper()
                      select u.AgencyId).Single();

            return id;
        }

        public static IEnumerable<int> VisibleAgencyIds(DbUser user)
        {
            if (!user.AgencyId.HasValue)
                return GuFacade.GetDictionaryManager().GetDictionary<Agency>().Select(t => t.Id);

            return user.Agency.Descendants(t => t.ChildAgencyList, true).Select(t => t.Id);
        }

        public static void CheckUserRights(string userName)
        {
            // берем тип залепного приложения (ожидается agency_id)
            // и сравниваем с agency пользователя
            string val = ConfigurationManager.AppSettings["ZlpAppType"];
            int? zlpType = val == null ? null : (int?)Convert.ToInt32(val);
            //DbUser = UserPolicy.GetDbUserByName(userName);

            // NOTE: по идее такая проверка уже должна была вызваться в LoginVM, получается некая двойственность
            // NOTE2: а в GU.MZ после логина нет такой проверки
            if (!UserPolicy.IsDbUserValid(userName, zlpType))
                throw new BLLException("Вам нельзя.");
        }

        private static bool IsDbUserValid(string dbUserName, int? zlpType)
        {
            using (var db = new GuDbManager())
            {
                DbUser user;
                try
                {
                    user = db.DbUser.Single(u => u.Name == dbUserName.ToUpper());
                }
                catch (InvalidOperationException)
                {
                    return false;
                }

                // спец. обработка для программ со служебными пользователями
                if (!zlpType.HasValue)
                    return !user.AgencyId.HasValue;

                //после логина нужно проверить что пользователь зашел в свое ведомство
                //так как нету загруженного DictionaryManager с проставленными связями в Agency
                //то приходится изворачиваться
                int? agencyId = user.AgencyId;
                while (agencyId != null)
                {
                    if (agencyId.Value == zlpType)
                        return true;

                    agencyId = (from u in db.Agency
                                where u.Id == agencyId.Value
                                select u.ParentAgencyId).Single();
                }
                return false;
            }
        }

        public static bool HasRole(this DbUser user, int roleId)
        {
            return user.PlainRoleList.Any(r => r.Id == roleId);
        }

        public static DbUser SaveDbUser(DbUser user, string password, IDomainDataMapper<DbUser> dataMapper)
        {
            DbUser resultUser;

            using (var db = new GuDbManager())
            {
                try
                {
                    db.BeginDomainTransaction();

                    if (user.PersistentState == PersistentState.New)
                    {
                        if (db.DbUser.Count(x => x.Name == user.Name) > 0)
                            throw new BLLException("Пользователь с таким логином уже заведен в системе");
                        CreateDbUser(user, password, db);
                    }

                    // revoke roles
                    var revokeList = GetDbRoleList(user.UserRoleList.DelItems.Cast<DbUserRole>());
                    revokeList.ForEach(x => RevokeRole(user, x, db));

                    // grant role
                    var grantList = GetDbRoleList(user.UserRoleList);
                    grantList.ForEach(x => GrantRole(user, x, db));

                    resultUser = dataMapper.Save(user, db);

                    db.CommitDomainTransaction();
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception)
                {
                    db.RollbackDomainTransaction();
                    throw;
                }
            }

            return resultUser;
        }

        private static List<string> GetDbRoleList(IEnumerable<DbUserRole> userRoleList)
        {
            return userRoleList
                .SelectMany(x => x.Role.Descendants(_ => _.ChildRoles, true))
                .Where(x => x.DbRoleName != null)
                .Select(x => x.DbRoleName)
                .Distinct()
                .ToList();
        }

        public static DbUser Activate(DbUser user)
        {
            DbUser result;

            using (var db = new GuDbManager())
            {
                try
                {
                    db.BeginDomainTransaction();

                    ActivateDbUser(user, db);
                    user.State = DbUserStateType.Active;
                    // TODO: BLT после клонирования объекта с изменением лишь енума возвращает "чистый" объект
                    // поэтому forceSave. То же относится к Block/Delete
                    result = GuFacade.GetDataMapper<DbUser>().Save(user, db, true);

                    db.CommitDomainTransaction();
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception)
                {
                    db.RollbackDomainTransaction();
                    throw;
                }
            }

            return result;
        }

        public static DbUser Block(DbUser user)
        {
            DbUser result;

            using (var db = new GuDbManager())
            {
                try
                {
                    db.BeginDomainTransaction();

                    BlockDbUser(user, db);
                    user.State = DbUserStateType.Disabled;
                    result = GuFacade.GetDataMapper<DbUser>().Save(user, db, true);

                    db.CommitDomainTransaction();
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception)
                {
                    db.RollbackDomainTransaction();
                    throw;
                }
            }

            return result;
        }

        public static DbUser Delete(DbUser user)
        {
            DbUser result;

            using (var db = new GuDbManager())
            {
                try
                {
                    db.BeginDomainTransaction();

                    DeleteDbUser(user, db);
                    user.State = DbUserStateType.Deleted;
                    result = GuFacade.GetDataMapper<DbUser>().Save(user, db, true);

                    db.CommitDomainTransaction();
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception)
                {
                    db.RollbackDomainTransaction();
                    throw;
                }
            }

            return result;
        }

        #region db interaction

        public static void CreateDbUser(DbUser user, string password, DomainDbManager dbManager)
        {
            string cmd = string.Format("CREATE USER {0} WITH PASSWORD '{1}';", user.Name, password);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void ChangeUserPassword(DbUser user, string password)
        {
            using(var db = new GuDbManager())
            {
                ChangeUserPassword(user, password, db);
            }
        }

        public static void ChangeUserPassword(DbUser user, string password, DomainDbManager dbManager)
        {
            string cmd = string.Format("ALTER USER {0} WITH PASSWORD '{1}';", user.Name, password);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void GrantRole(DbUser user, string roleName, DomainDbManager dbManager)
        {
            string cmd = string.Format("GRANT {0} TO {1};", roleName, user.Name);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void RevokeRole(DbUser user, string roleName, DomainDbManager dbManager)
        {
            string cmd = string.Format("REVOKE {0} FROM {1};", roleName, user.Name);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void ActivateDbUser(DbUser user, DomainDbManager dbManager)
        {
            string cmd = string.Format("ALTER USER {0} LOGIN;", user.Name);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void BlockDbUser(DbUser user, DomainDbManager dbManager)
        {
            string cmd = string.Format("ALTER USER {0} NOLOGIN;", user.Name);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        public static void DeleteDbUser(DbUser user, DomainDbManager dbManager)
        {
            string cmd = string.Format("DROP USER {0};", user.Name);
            dbManager.SetCommand(CommandType.Text, cmd).ExecuteScalar();
        }

        #endregion
    }
}
