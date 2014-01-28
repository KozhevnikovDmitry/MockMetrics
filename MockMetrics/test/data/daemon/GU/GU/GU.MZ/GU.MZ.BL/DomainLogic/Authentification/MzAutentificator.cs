using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BLToolkit.Common;
using Common.BL.Authentification;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;

using GU.BL;
using GU.BL.DataMapping;
using GU.BL.Policy;
using GU.DataModel;
using GU.MZ.DataModel.Person;
using CommonData = GU.MZ.DataModel.CommonData;

namespace GU.MZ.BL.DomainLogic.Authentification
{
    /// <summary>
    /// Класс, ответсвенный за аутентификацию пользователя приложения Лицензирование
    /// </summary>
    public class MzAutentificator : Authentificator
    {
        /// <summary>
        /// Инициализатор базы данных
        /// </summary>
        private readonly IDataAccessLayerInitializer _dataAccessLayerInitializer;

        /// <summary>
        /// Класс, ответсвенный за аутентификацию пользователя приложения Лицензирование
        /// </summary>
        /// <param name="configuration">Конфигурация подключения к базе данных</param>
        /// <param name="dataAccessLayerInitializer">Объект, проверяющий подключение к БД.</param>
        public MzAutentificator(IProviderConfiguration configuration, IDataAccessLayerInitializer dataAccessLayerInitializer)
            : base(configuration, dataAccessLayerInitializer)
        {
            _dataAccessLayerInitializer = dataAccessLayerInitializer;
        }

        /// <summary>
        /// Проводит аутентификацию сотрудника отдела лицензирования, возвращает флаг успешности аутентификации.
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>Флаг успешности аутентификации</returns>
        public override bool AuthentificateUser(string username, string password)
        {
            if (base.AuthentificateUser(username, password))
            {
                if (AuthenificatedEmployeeExists(username.ToUpper()))
                {
                    Configuration.NullableValues.String = null;
                    _dataAccessLayerInitializer.Initialize(_configuration, new Dictionary<Assembly, IDomainObjectInitializer>
                    {
                        {typeof(CommonData).Assembly, new GumzDomainObjectInitializer(username)},
                        {typeof(GU.DataModel.CommonData).Assembly, new GuDomainObjectInitializer(username)}
                    });
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверяет наличие сотрудника и пользователя по логину
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        private bool AuthenificatedEmployeeExists(string username)
        {
            using (var dbManager = _dataAccessLayerInitializer.GetDbManager(typeof(TaskDataMapper).Assembly))
            {
                UserPolicy.CheckUserRights(username);

                var dbUser = dbManager.GetDomainTable<DbUser>()
                                      .SingleOrDefault(t => t.Name == username);

                if (dbUser != null)
                {
                    if (dbManager.GetDomainTable<Employee>()
                                 .Any(t => t.DbUserId == dbUser.Id))
                    {
                        return true;
                    }
                    else
                    {
                        ErrorMessage = string.Format("Сотрудник для пользователя с именем {0} не заведён", username);
                    }
                }
                else
                {
                    ErrorMessage = string.Format("Пользователь с именем {0} не заведён", username);
                }

                return false;
            }
        }
    }
}
