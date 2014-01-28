using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using Common.BL;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.ReportMapping;
using Common.BL.Search.SearchSpecification;
using Common.DA;
using Common.DA.ProviderConfiguration;
using GU.Archive.DataModel;
using GU.BL;

namespace GU.Archive.BL
{
    /// <summary>
    /// Класс содержит в себе экземпляры всех классов бизнес-логики,
    /// которые должны быть представлены в единственном числе
    /// </summary>
    internal class ArchiveCore : ICore
    {
        #region Singleton

        private static ArchiveCore _instance;
        private static readonly object _locker = new object();

        /// <summary>
        /// Вовзращает ArchiveCore синглтон
        /// </summary>
        public static ArchiveCore Instance
        {
            get
            {
                if (_instance == null)
                    lock (_locker)
                        if (_instance == null)
                            _instance = new ArchiveCore();

                return _instance;
            }
        }

        private ArchiveCore()
        {
            UserPreferences = new ArchiveUserPreferences();
        }

        #endregion

        /// <summary>
        /// Вовзвращает менеджер справочников.
        /// </summary>
        public IDictionaryManager DictionaryManager { get; private set; }

        /// <summary>
        /// Возвращает Пользовательские настройки.
        /// </summary>
        public IUserPreferences UserPreferences { get; private set; }

        /// <summary>
        /// Возвращает контейнер классов бизнес-логики
        /// </summary>
        public ArchiveLogicContainer ArchiveLogicContainer { get; private set; }
        
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
                //Логирование в файловый логгер
                //LoggerContainer.Initilize(new FileLogger("log.txt", new AppLogGenerator()));

                //Задаём пользовательские настройки
                //UserPreferences = new BL.UserPreferences();

                //Собираем bl-сборки
                var blAssemlies = new List<Assembly> { Assembly.GetExecutingAssembly(), Assembly.UnsafeLoadFrom("GU.BL.dll") };

                //Инитим базу слой DA
                InitializeDA(config);

                if (_isDbAvailable)
                {
                    //Логирование в базу
                    //LoggerContainer.Initilize(new DataBaseLogger("Local", new AppLogGenerator()),
                    //                          new FileLogger("uec_log.txt", new AppLogGenerator()));

                    //Инитим менеждер справочников
                    DictionaryManager = new ArchiveDictionaryManager();

                    // Мержим менеджеры справочников
                    DictionaryManager.Merge(GuFacade.GetDictionaryManager());

                    //Задаём контейнер классов бизнес-логики
                    this.ArchiveLogicContainer = new ArchiveLogicContainer(blAssemlies, DictionaryManager);
                }
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
                var init = new DataAccessLayerInitializer();
                var initializer = new ArchiveDomainObjectInitializer(WindowsIdentity.GetCurrent().Name);
                init.Initialize(config, typeof(CommonData).Assembly, initializer);
                _isDbAvailable = true;
            }
            catch (Exception ex)
            {
                InitializationException = ex;
                _isDbAvailable = false;
            }

        }

        private bool _isDbAvailable;
    }
}
