using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;
using GU.BL.Extensions;
using GU.DataModel;

namespace GU.BL.DataMapping
{
    public class DbUserDataMapper : AbstractDataMapper<DbUser>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public DbUserDataMapper(IDomainContext domainContext, IDictionaryManager dictionaryManager)
            : base(domainContext)
        {
            _dictionaryManager = dictionaryManager;
        }

        protected override DbUser RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var user = dbManager.RetrieveDomainObject<DbUser>(id);

            if (user.AgencyId.HasValue)
                user.Agency = _dictionaryManager.GetDictionaryItem<Agency>(user.AgencyId.Value);

            var allRoles = _dictionaryManager.GetDictionary<DbRole>();

            var userRoles = (from u in dbManager.GetDomainTable<DbUser>()
                             join ur in dbManager.GetDomainTable<DbUserRole>() on u.Id equals ur.UserId
                             where ur.UserId == user.Id
                             select ur.Id).ToList()
                .Select(x => dbManager.RetrieveDomainObject<DbUserRole>(x)).ToList();

            user.UserRoleList.AddRange(userRoles);

            foreach (var userRole in userRoles)
            {
                userRole.User = user;
                userRole.Role = allRoles.Single(x => x.Id == userRole.RoleId);
                user.RoleList.Add(userRole.Role);
                var oneRoleList = userRole.Role.Descendants(t => t.ChildRoles, true);
                user.PlainRoleList.AddRange(oneRoleList.ToList());
            }

            return user;
        }

        protected override DbUser SaveOperation(DbUser obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            // сохранили непосредственно DbUser
            dbManager.SaveDomainObject(tmp, forceSave);

            // 1. deleted roles
            foreach (var ur in tmp.UserRoleList.DelItems.Cast<DbUserRole>())
            {
                ur.MarkDeleted();
                dbManager.SaveDomainObject(ur);
            }

            // 2. new roles
            foreach (var ur in tmp.UserRoleList.NewItems.Cast<DbUserRole>())
            {
                ur.UserId = tmp.Id;
                dbManager.SaveDomainObject(ur);
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(DbUser obj, IDomainDbManager dbManager)
        {

        }
    }
}
