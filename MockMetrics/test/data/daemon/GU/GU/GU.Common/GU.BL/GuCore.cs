using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using Autofac;
using Common.BL;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.ReportMapping;
using Common.BL.Search.SearchSpecification;
using Common.DA;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;
using GU.BL.Policy;
using GU.DataModel;

namespace GU.BL
{
    /// <summary>
    /// Класс содержит в себе экземпляры всех классов бизнес-логики,
    /// которые должны быть представлены в единственном числе
    /// </summary>
    internal class GuCore : ICore
    {
        #region Singleton

        private static GuCore _instance;
        private static readonly object _locker = new object();

        /// <summary>
        /// Вовзращает GuCore синглтон
        /// </summary>
        public static GuCore Instance
        {
            get
            {
                if (_instance == null)
                    lock (_locker)
                        if (_instance == null)
                            _instance = new GuCore();

                return _instance;
            }
        }

        private GuCore()
        {
            UserPreferences = new GuUserPreferences();
        }

        #endregion

        /// <summary>
        /// Флаг доступности базы данных.
        /// </summary>
        private bool _isDbAvailable;

        /// <summary>
        /// Вовзвращает менеджер справочников.
        /// </summary>
        public IDictionaryManager DictionaryManager { get; private set; }

        /// <summary>
        /// Возвращает Пользовательские настройки.
        /// </summary>
        public IUserPreferences UserPreferences { get; private set; }

        /// <summary>
        /// Возвращает контейнер классов бизнес логики предметной области "Работа с заявлениями"
        /// </summary>
        public GuDomainLogicContainer GuDomainLogicContainer { get; private set; }

        /// <summary>
        /// Юзер, под которым зашли в систему
        /// </summary>
        public DbUser DbUser { get; private set; }

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
                // Инитим базу слой DA
                InitializeDA(config);

                var assembly = Assembly.GetAssembly(typeof(GU.BL.GuCore));
                // Задаём сборки с бизнес-логикой
                var blAssemlies = new List<Assembly> { assembly };

                if (_isDbAvailable)
                {
                    using (var db = new GuDbManager())
                    {
                        // должно быть где-то при попытке логина
                        UserPolicy.CheckUserRights(config.User);

                        var userAgencyId = UserPolicy.GetUserAgencyId(config.User, db);

                        // Инитим менеждер справочников
                        DictionaryManager = new GuDictionaryManager(db, userAgencyId);

                        // получаем DbUser
                        DbUser = UserPolicy.GetUser(config.User, db, DictionaryManager);
                    }

                    // Инитим контейнер классов бизнес-логики
                    this.GuDomainLogicContainer = new GuDomainLogicContainer(blAssemlies, this.DbUser, DictionaryManager);
                }
            }
            catch (Exception ex)
            {
                InitializationException = ex;
            }
        }
        
        /// <summary>
        /// Инициализация ядра общей логики для MZ
        /// </summary>
        public void Initialize(IDomainLogicContainer logicContainer)
        {
            try
            {
                DictionaryManager = logicContainer.IocContainer.Resolve<IDictionaryManager>();
                DbUser = logicContainer.IocContainer.Resolve<DbUser>();
                GuDomainLogicContainer = new GuDomainLogicContainer(logicContainer.IocContainer);
            }
            catch (Exception ex)
            {
                InitializationException = ex;
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

                // Инициализируем BLT конфигурацией подключения
                DataAccessLayerInitializer init = new DataAccessLayerInitializer();
                init.Initialize(config, typeof(CommonData).Assembly, new GuDomainObjectInitializer(WindowsIdentity.GetCurrent().Name));

                _isDbAvailable = true;
            }
            catch (Exception ex)
            {
                InitializationException = ex;
                _isDbAvailable = false;
            }

        }
    }
}
