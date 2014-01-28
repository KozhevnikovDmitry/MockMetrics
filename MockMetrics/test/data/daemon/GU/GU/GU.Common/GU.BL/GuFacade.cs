using System;

using Common.BL;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.Search;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;

using GU.BL.Policy.Interface;
using GU.BL.Reporting.Mapping;
using GU.DataModel;

namespace GU.BL
{
    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class GuFacade
    {
        public static bool TestProviderConfiguration(IProviderConfiguration config)
        {
            return new DataAccessLayerInitializer().TestConfiguration(config);
        }

        private static readonly object _locker = new object();

        public static void InitializeCore(IProviderConfiguration config)
        {
            lock (_locker)
            {
                GuCore.Instance.Initialize(config);
                if (GuCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", GuCore.Instance.InitializationException);
                }
            }
        }

        /// <summary>
        /// Инициализация ядра общей логики для MZ
        /// </summary>
        public static void InitializeCore(IDomainLogicContainer container)
        {
            lock (_locker)
            {
                GuCore.Instance.Initialize(container);
                if (GuCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", GuCore.Instance.InitializationException);
                }
            }
        }

        public static DbUser GetDbUser()
        {
            return GuCore.Instance.DbUser;
        }

        public static IDictionaryManager GetDictionaryManager()
        {
            return GuCore.Instance.DictionaryManager;
        }

        public static GuUserPreferences GetUserPreferences()
        {
            return (GuUserPreferences)GuCore.Instance.UserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveDataMapper<T>();
        }

        public static ITaskPolicy GetTaskPolicy()
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskPolicy();
        }

        public static ISearchStrategy<Task> GetTaskSearchStrategy()
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskSearchStrategy();
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return GuCore.Instance.GuDomainLogicContainer;
        }

        public static IDomainValidator<T> GetValidator<T>() where T : IPersistentObject
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveValidator<T>();
        }

        public static TaskStatReport GetTaskStatReport()
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskStatReport();
        }

        public static TaskRegistrReport GetTaskRegistrReport(string username, DateTime startDate, DateTime endDate)
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskRegistrReport(username, startDate, endDate);
        }

        public static TaskInfoReport GetTaskInfoReport(Task task)
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskInfoReport(task);
        }

        public static TaskDataReport GetTaskDataReport(Task task, string username) 
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskDataReport(task, username);
        }

        public static TaskDataListReport GetTaskDataListReport(string username)
        {
            return GuCore.Instance.GuDomainLogicContainer.ResolveTaskDataListReport(username);
        }
    }
}
