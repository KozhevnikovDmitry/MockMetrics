using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.BL;
using GU.BL.Extensions;
using GU.BL.Policy;
using GU.DataModel;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.BL
{
    public class HqDictionaryManager : AbstractDictionaryManager
    {
        public HqDictionaryManager(IDomainDbManager dbManager)
        {
            try
            {
                //справочники
                _dictionaries = new Dictionary<Type, List<IDomainObject>>();
                _dictionaries[typeof(PersonDocumentType)] = GetDomainList<PersonDocumentType>(dbManager);
                _dictionaries[typeof(DisabilityType)] = GetDomainList<DisabilityType>(dbManager);
                _dictionaries[typeof(RelativeType)] = GetDomainList<RelativeType>(dbManager);
                _dictionaries[typeof(QueueBaseRegType)] = GetQueueBaseRegList(dbManager);
                _dictionaries[typeof(QueuePrivBaseRegType)] = GetDomainList<QueuePrivBaseRegType>(dbManager);
                _dictionaries[typeof(QueueBaseDeRegType)] = GetDomainList<QueueBaseDeRegType>(dbManager);
                _dictionaries[typeof(CategoryType)] = GetDomainList<CategoryType>(dbManager);
                _dictionaries[typeof(Queue)] = GetDomainList<Queue>(dbManager);
                _dictionaries[typeof(DbUser)] = GetDbUserList(dbManager);

                // enums
                _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
                _enumDictionaries[typeof (Sex)] = GetSexList();
                _enumDictionaries[typeof (ClaimStatusType)] = GetClaimStatusList();
                _enumDictionaries[typeof (HouseTypePrivate)] = GetHousePrivList();
                _enumDictionaries[typeof (HouseTypeComfort)] = GetHouseComfortList();
                _enumDictionaries[typeof(QueueType)] = GetQueueTypeList(); 
            }
            catch (Exception ex)
            {
                throw new BLLException("DictionaryManager constructor failed", ex);
            }
        }
        
        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        private List<IDomainObject> GetDbUserList(IDomainDbManager dbManager)
        {
            return (from u in dbManager.GetDomainTable<DbUser>()
                    join a in dbManager.GetDomainTable<Agency>() on u.AgencyId equals a.Id
                    join sg in dbManager.GetDomainTable<ServiceGroup>() on a.Id equals sg.AgencyId
                    join s in dbManager.GetDomainTable<Service>() on sg.Id equals s.ServiceGroupId
                    where s.Id == 38 
                    select u.Id).ToList().
            Select(val => dbManager.RetrieveDomainObject<DbUser>(val)).ToList<IDomainObject>();
        }

        /// <summary>
        /// Возвращает список оснований для постановки на учет.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Список доменных объектов</returns>
        private List<IDomainObject> GetQueueBaseRegList(IDomainDbManager dbManager) 
        {
             return  (from qbr in dbManager.GetDomainTable<QueueBaseRegType>()
                            where qbr.DateEnd == Convert.ToDateTime("31.12.2999")
             select qbr.Id).ToList().
             Select(val => dbManager.RetrieveDomainObject<QueueBaseRegType>(val)).ToList<IDomainObject>();
        }

        /// <summary>
        /// Возвращает список доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Список доменных объектов</returns>
        private List<IDomainObject> GetDomainList<T>(IDomainDbManager dbManager) where T : class, IDomainObject
        {
            return dbManager.GetDomainTable<T>().ToList<IDomainObject>();
        }

        /// <summary>
        /// Создание словаря статусов
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetClaimStatusList()
        {
            var requestClaimStatusList = new Dictionary<int, string>();
            requestClaimStatusList[1] = "Проверка данных";
            requestClaimStatusList[2] = "Заявление поставлено в очередь.";
            requestClaimStatusList[3] = "Заявление поставлено в очередь внеочередников.";
            requestClaimStatusList[4] = "Заявление исключено из очереди внеочередников.";
            requestClaimStatusList[5] = "Жилье предоставлено.";
            requestClaimStatusList[6] = "Заявление отклонено/исключено из очереди.";
            return requestClaimStatusList;
        }

        /// <summary>
        /// Создание словаря полов
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetSexList()
        {
            var requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "Мужской";
            requestSexList[2] = "Женский";
            return requestSexList;
        }

        /// <summary>
        /// Создание словаря типов домов по комфортабельности
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetHouseComfortList()
        {
            var requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "Благоустроенный дом";
            requestSexList[2] = "Не благоустроенный";
            return requestSexList;
        }

        /// <summary>
        /// Создание словаря типов домов по приватности
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetHousePrivList()
        {
            var requestSexList = new Dictionary<int, string>();
            requestSexList[1] = "Частный дом";
            requestSexList[2] = "Многоквартирный дом";
            return requestSexList;
        }

        /// <summary>
        /// Создание словаря типов очереди
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, string> GetQueueTypeList()
        {
            var requestQueueTypeList = new Dictionary<int, string>();
            requestQueueTypeList[1] = "Очередь обычная";
            requestQueueTypeList[2] = "Очередь внеочередников";
            return requestQueueTypeList;
        }

    }
}
