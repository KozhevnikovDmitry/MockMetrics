using System;
using System.Collections.Generic;
using System.Reflection;

using Common.BL;
using Common.BL.DictionaryManagement;
using Common.BL.ReportMapping;

using GU.BL;

namespace GU.MZ.DS.BL
{
    /// <summary>
    /// Класс содержит в себе экземпляры всех классов бизнес-логики,
    /// которые должны быть представлены в единственном числе
    /// </summary>
    internal class DsCore : ICore
    {
        #region Singleton
        
        /// <summary>
        /// Ленивый singleton-экземпляр DsCore. 
        /// </summary>
        private static readonly Lazy<DsCore> Lazy = new Lazy<DsCore>(() => new DsCore());

        /// <summary>
        /// Возвращает singleton-экземпляр DsCore.
        /// </summary>
        public static DsCore Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        /// <summary>
        /// Класс содержит в себе экземпляры всех классов бизнес-логики,
        /// которые должны быть представлены в единственном числе
        /// </summary>
        private DsCore()
        {
            this.Initialize();
            this.UserPreferences = new DsUserPreferences();
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
        /// Возвращает контейнер классов бизнес логики предметной области Аттестация.
        /// </summary>
        public DomainLogicContainer DomainLogicContainer { get; private set; }
        
        /// <summary>
        /// Возвращает исключение с информацией об ошибке инициализации.
        /// </summary>
        internal Exception InitializationException { get; private set; }

        /// <summary>
        /// Инициализирует слой BL
        /// </summary>
        /// <param name="config">Конфигурация подключения к базе данных</param>
        private void Initialize()
        {
            try
            {
                //Логирование в файловый логгер
                //LoggerContainer.Initilize(new FileLogger("log.txt", new AppLogGenerator()));

                //Задаём пользовательские настройки
                //UserPreferences = new BL.UserPreferences();

                //Собираем bl-сборки
                var blAssemlies = new List<Assembly>
                    { Assembly.GetExecutingAssembly(), Assembly.UnsafeLoadFrom("GU.BL.dll") };

                //Логирование в базу
                //LoggerContainer.Initilize(new DataBaseLogger("Local", new AppLogGenerator()),
                //                          new FileLogger("uec_log.txt", new AppLogGenerator()));

                using (var db = new DsDbManager())
                {
                    //Инитим менеждер справочников
                    this.DictionaryManager = new DsDictionaryManager(db);
                }

                // Мержим менеджеры справочников
                this.DictionaryManager.Merge(GuFacade.GetDictionaryManager());

                // Задаём контейнер классов бизнес логики
                this.DomainLogicContainer = new DomainLogicContainer(blAssemlies, this.DictionaryManager);
            }
            catch (Exception ex)
            {
                this.InitializationException = ex;
            }

        }
    }
}
