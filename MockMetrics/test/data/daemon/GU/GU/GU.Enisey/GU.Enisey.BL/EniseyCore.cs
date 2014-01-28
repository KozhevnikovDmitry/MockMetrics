using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Common.BL;
using Common.BL.DictionaryManagement;
using Common.DA.ProviderConfiguration;
using Common.DA;
using GU.BL;
using System.Security.Principal;

using GU.BL.Policy;
using GU.Enisey.BL.Converters;
using GU.Enisey.DataModel;

using Autofac;

namespace GU.Enisey.BL
{
    /// <summary>
    /// Класс содержит в себе экземпляры всех классов бизнес-логики,
    /// которые должны быть представлены в единственном числе
    /// </summary>
    internal class EniseyCore : ICore, IDisposable
    {
        #region Singleton

        /// <summary>
        /// Lazy singleton instance of <see cref="EniseyCore"/>
        /// </summary>
        private static readonly Lazy<EniseyCore> Lazy = new Lazy<EniseyCore>(() => new EniseyCore());

        /// <summary>
        /// Returns singleton-instance of <see cref="EniseyCore"/>
        /// </summary>
        public static EniseyCore Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        private EniseyCore()
        {
             
        }

        #endregion

        /// <summary>
        /// Вовзвращает менеджер справочников.
        /// </summary>
        public IDictionaryManager DictionaryManager { get; private set; }

        public EniseyDomainLogicContainer EniseyDomainLogicContainer { get; private set; }

        /// <summary>
        /// Возвращает Пользовательские настройки.
        /// </summary>
        public IUserPreferences UserPreferences { get; private set; }

        /// <summary>
        /// Список всех экшенов, которые нужно выполнять по расписанию
        /// </summary>
        public List<ServiceExecutor> ServiceExecutors { get; private set; }

        /// <summary>
        /// Возвращает исключение с информацией об ошибке инициализации.
        /// </summary>
        internal Exception InitializationException { get; private set; }

        /// <summary>
        /// Инициализирует слой BL
        /// </summary>
        public void Initialize(IProviderConfiguration config)
        {
            try
            {
                //Инитим базу слой DA
                InitializeDA(config);
                InitializeBL(config);
            }
            catch (Exception ex)
            {
                InitializationException = ex;
            }
        }

        private void InitializeBL(IProviderConfiguration config)
        {
            var assembly = Assembly.GetAssembly(typeof(GU.BL.GuFacade));
            // Задаём сборки с бизнес-логикой
            var blAssemlies = new List<Assembly> { assembly };
            
            using (var db = new EniseyDbManager())
            {
                // должно быть где-то при попытке логина
                UserPolicy.CheckUserRights(config.User);

                var userAgencyId = UserPolicy.GetUserAgencyId(config.User, db);

                // Инитим менеждер справочников
                DictionaryManager = new GuDictionaryManager(db, userAgencyId);

                // получаем DbUser
                var dbUser = UserPolicy.GetUser(config.User, db, DictionaryManager);

                // Инитим контейнер классов бизнес-логики
                this.EniseyDomainLogicContainer = new EniseyDomainLogicContainer(blAssemlies, dbUser, DictionaryManager);
            }
        }

        /// <summary>
        /// Инициализирует слой DA
        /// </summary>
        private void InitializeDA(IProviderConfiguration config)
        {
            try
            {
                // по дефолту BLT мапит NULL из строковых столбцов в string.Empty
                // заставляем его мапить в null
                BLToolkit.Common.Configuration.NullableValues.String = null;
                DataAccessLayerInitializer init = new DataAccessLayerInitializer();
                var initializer = new GuDomainObjectInitializer(WindowsIdentity.GetCurrent().Name);
                init.Initialize(config, typeof(TaskSend).Assembly, initializer);
            }
            catch (Exception ex)
            {
                InitializationException = ex;
            }
        }

        public void InitializeServices()
        {
            ServiceExecutors = new List<ServiceExecutor>();

            var se = new ServiceExecutor(AppQuiryPortTypeLogic.SetStateList, 10 * 1000);
            ServiceExecutors.Add(se);
        }

        public ConverterManager GetConverterManager()
        {
            return EniseyDomainLogicContainer.IocContainer.Resolve<ConverterManager>();
        }

        #region IDisposable

        public void Dispose()
        {
            foreach (var se in ServiceExecutors)
            {
                se.Dispose();
            }
        }

        #endregion
    }
}
