using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL;
using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;
using GU.DataModel;
using GU.HQ.BL.DomainLogic.AcceptTask.Interface;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.BL.Policy;
using GU.HQ.BL.Reporting.Mapping;

namespace GU.HQ.BL
{

    /// <summary>
    /// Класс-фасад слоя BL. Предназначен для получения доступа к классам бизнес-логики.
    /// </summary>
    public static class HqFacade
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
                HqCore.Instance.Initialize(config);
                if (HqCore.Instance.InitializationException != null)
                {
                    throw new BLLException("Failed to initialize BLL", HqCore.Instance.InitializationException);
                }
            }
        }

        public static HqDictionaryManager GetDictionaryManager()
        {
            return (HqDictionaryManager)HqCore.Instance.DictionaryManager;
        }

        public static HqUserPreferences GetUserPreferences()
        {
            return HqCore.Instance.UserPreferences as HqUserPreferences;
        }

        public static IDomainDataMapper<T> GetDataMapper<T>() where T : IPersistentObject
        {
            return HqCore.Instance.HqLogicContainer.ResolveDataMapper<T>();
        }

        /// <summary>
        /// Вовзвращает экземпляр преобразователя Task в Claim.
        /// </summary>
        /// <returns>преобразователь Task в Claim.</returns>
        public static ITaskDataParser GetTaskParser()
        {
            throw new NotImplementedException();
            //return HqCore.Instance.HqLogicContainer.ResolveTaskDataParser();
        }

        public static IDomainLogicContainer GetLogicContainer()
        {
            return HqCore.Instance.HqLogicContainer;
        }

        /// <summary>
        /// Получить валидатор для объекта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDomainValidator<T> GetValidator<T>() where T : IPersistentObject
        {
            return HqCore.Instance.HqLogicContainer.ResolveValidator<T>();
        }

        /// <summary>
        /// Получить объект управления очередями
        /// </summary>
        /// <returns></returns>
        public static IClaimStatusPolicy GetClaimStatusPolicy()
        {
            return HqCore.Instance.HqLogicContainer.ResolveClaimStatusPolicy();
        }

        /// <summary>
        /// Получить объект для управления очередью.
        /// </summary>
        /// <returns></returns>
        public static IQueueManager GetQueueManager()
        {
            return HqCore.Instance.HqLogicContainer.ResolveQueueManager();
        }


        public static ClaimRegistrReport GetClaimRegistrReport(string username, DateTime startDate, DateTime endDate)
        {
            return HqCore.Instance.HqLogicContainer.ResolveClaimRegistrReport(username, startDate, endDate);
        }
    }
}
