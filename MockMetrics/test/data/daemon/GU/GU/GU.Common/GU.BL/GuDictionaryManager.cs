using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.DataModel;
using System.Threading.Tasks;
using GU.BL.Extensions;
using GU.DataModel.Inquiry;

namespace GU.BL
{
    /// <summary>
    /// Класс, предназначенный для хранения справочных данных схемы GU
    /// </summary>
    public class GuDictionaryManager : AbstractDictionaryManager
    {
        /// <summary>
        /// Класс, предназначенный для получения значений справочных таблиц БД.
        /// </summary>
        public GuDictionaryManager(IDomainDbManager dbManager, int? rootAgencyId)
        {
            try
            {
                _dictionaries = GetServiceHierarchy(dbManager, rootAgencyId);
                _dictionaries[typeof(DbRole)] = GetRoleDictionary(dbManager);

                _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
                _enumDictionaries[typeof(AttrDataType)] = GetAttrDataTypeList();
                _enumDictionaries[typeof(TaskStatusType)] = this.GetTaskStatusList();
                _enumDictionaries[typeof(DbUserStateType)] = this.GetDbUserStatusList();

                _dynamicDictionaries[typeof(DbUser)] = GetDbUserList;
            }
            catch (Exception ex)
            {
                throw new BLLException("DictionaryManager constructor failed", ex);
            }
        }

        #region DomainObjects Retrieving

        private Dictionary<int, Agency> GetAgencyDict(IDomainDbManager dbManager, int? rootAgencyId)
        {
            // загрузка всех ведомств
            var allAgencyDict = dbManager.GetDomainTable<Agency>().ToDictionary(t => t.Id);

            foreach (var t in allAgencyDict.Values)
            {
                t.ChildAgencyList = new List<Agency>();
                t.ServiceGroupList = new List<ServiceGroup>();
            }

            // проставление связей родитель-потомки
            foreach (var t in allAgencyDict.Values)
            {
                if (t.ParentAgencyId.HasValue)
                {
                    t.ParentAgency = allAgencyDict[t.ParentAgencyId.Value];
                    t.ParentAgency.ChildAgencyList.Add(t);
                }
            }

            if (!rootAgencyId.HasValue)
                return allAgencyDict;

            var descAgencyDict = allAgencyDict[rootAgencyId.Value].Descendants(t => t.ChildAgencyList, true);
            var ascAgencyDict = allAgencyDict[rootAgencyId.Value].Ancestors(t => t.ParentAgency, false);
            // видимые ведомства - как дочерние ведомства, так и предки
            var visibleAgencyDict = ascAgencyDict.Concat(descAgencyDict).ToDictionary(t => t.Id);

            //очистка от недоступных ведомств ("двоюродные братья" и пр.)
            foreach (var agency in visibleAgencyDict.Values)
            {
                agency.ChildAgencyList.RemoveAll(t => !visibleAgencyDict.ContainsKey(t.Id));
            }

            return visibleAgencyDict;
        }

        private Spec RetrieveSpec(IDomainDbManager dbManager,
                                  int specId, 
                                  Dictionary<int, Spec> specDict,
                                  Dictionary<int, SpecNode> specNodeDict)
        {
            if (specDict.ContainsKey(specId))
                return specDict[specId];

            var spec = dbManager.GetDomainTable<Spec>().Single(t => t.Id == specId);
            specDict.Add(spec.Id, spec);

            var specNodes = dbManager.GetDomainTable<SpecNode>().Where(t => t.SpecId == specId).ToDictionary(t => t.Id);
            foreach (var specNode in specNodes.Values)
            {
                specNode.Spec = spec;
                specNode.ChildSpecNodes = new List<SpecNode>();
                specNodeDict.Add(specNode.Id, specNode);

                if (specNode.RefSpecId.HasValue)
                    specNode.RefSpec = this.RetrieveSpec(dbManager, specNode.RefSpecId.Value, specDict, specNodeDict);
            }
            foreach (var specNode in specNodes.Values)
            {
                if (specNode.ParentSpecNodeId.HasValue)
                {
                    specNode.ParentSpecNode = specNodeDict[specNode.ParentSpecNodeId.Value];
                    specNode.ParentSpecNode.ChildSpecNodes.Add(specNode);
                }
            }

            //group.Elements = elements.Values.ToList();
            spec.RootSpecNodes = specNodes.Values.Where(t => !t.ParentSpecNodeId.HasValue).ToList();

            return spec;
        }

        private Dictionary<Type, List<IDomainObject>> GetServiceHierarchy(IDomainDbManager dbManager, int? rootAgencyId)
        {
            // все видимые ведомства
            var agencyDict = GetAgencyDict(dbManager, rootAgencyId);

            //видимость спеков и услуг проверяется по переданному списку ведомств
            var serviceGroupDict = dbManager.GetDomainTable<ServiceGroup>().Where(t => agencyDict.ContainsKey(t.AgencyId)).ToDictionary(t => t.Id);
            var serviceDict =
                dbManager.GetDomainTable<Service>().Where(t => serviceGroupDict.ContainsKey(t.ServiceGroupId)).ToDictionary(t => t.Id);

            //TODO: как-нибудь отсечь по ведомствам
            // виды межвед запросов
            var inquiryTypeDict = dbManager.GetDomainTable<InquiryType>().ToDictionary(t => t.Id);

            var specDict = new Dictionary<int, Spec>();
            var specNodeDict = new Dictionary<int, SpecNode>();

            //справочники загружаются все, но наборы их элементов загрузятся далее только для используемых
            var allDictDict = dbManager.GetDomainTable<Dict>().ToDictionary(t => t.Id);


            //проставление ссылок между объектами разных уровней
            //сортировка в циклах задает дефолтовый порядок

            foreach (var serviceGroup in serviceGroupDict.Values.OrderBy(t => t.ServiceGroupName))
            {
                serviceGroup.Agency = agencyDict[serviceGroup.AgencyId];
                if (agencyDict[serviceGroup.AgencyId].ServiceGroupList == null)
                    agencyDict[serviceGroup.AgencyId].ServiceGroupList = new List<ServiceGroup>();
                agencyDict[serviceGroup.AgencyId].ServiceGroupList.Add(serviceGroup);
                serviceGroup.ServiceList = new List<Service>();
            }

            foreach (var service in serviceDict.Values.OrderBy(t => t.Order).ThenBy(t => t.Name))
            {
                service.ServiceGroup = serviceGroupDict[service.ServiceGroupId];
                if (service.SpecId.HasValue)
                    service.Spec = this.RetrieveSpec(dbManager, service.SpecId.Value, specDict, specNodeDict);
                serviceGroupDict[service.ServiceGroupId].ServiceList.Add(service);
            }

            foreach (var inquiryType in inquiryTypeDict.Values)
            {
                inquiryType.RequestSpec = RetrieveSpec(dbManager, inquiryType.RequestSpecId, specDict, specNodeDict);
                inquiryType.ResponseSpec = RetrieveSpec(dbManager, inquiryType.ResponseSpecId, specDict, specNodeDict);
                if (inquiryType.ServiceId.HasValue)
                    inquiryType.Service = serviceDict[inquiryType.ServiceId.Value];
            }

            foreach (var specNode in specNodeDict.Values.OrderBy(t => t.Order).ThenBy(t => t.Name))
            {
                if (specNode.DictId.HasValue)
                    specNode.Dict = allDictDict[specNode.DictId.Value];
            }
            
            //отбираем только используемые справочники и грузим по ним детализацию
            var usedDictIdList = specNodeDict.Values.GroupBy(t => t.DictId).Select(t => t.Key).ToList();
            var dictDict = allDictDict.Values.Where(t => usedDictIdList.Contains(t.Id)).ToDictionary(t => t.Id);
            var dictDetList = dbManager.GetDomainTable<DictDet>().Where(t => dictDict.ContainsKey(t.DictId)).ToList();

            foreach (var dict in dictDict.Values)
            {
                dict.DictDets = new List<DictDet>();
            }

            foreach (var dictDet in dictDetList.OrderBy(t => t.SortOrder ?? int.MaxValue).ThenBy(t => t.ItemName))
            {
                dictDet.Dict = dictDict[dictDet.DictId];
                dictDict[dictDet.DictId].DictDets.Add(dictDet);
            }

            //NOTE: а нужно ли отдавать наружу справочники помимо Agency? Остальные будут доступны по ссылкам...
            //формирование результирующего набора словарей
            var dictionaries = new Dictionary<Type, List<IDomainObject>>();
            dictionaries[typeof(Agency)] = agencyDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(ServiceGroup)] = serviceGroupDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(Service)] = serviceDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(InquiryType)] = inquiryTypeDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(Spec)] = specDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(SpecNode)] = specNodeDict.Values.Cast<IDomainObject>().ToList();
            dictionaries[typeof(DictDet)] = dictDetList.Cast<IDomainObject>().ToList();
            return dictionaries;

        }

        private List<IDomainObject> GetRoleDictionary(IDomainDbManager db)
        {
            // TODO: неплохо бы в соответствии с правами пользователя убрать роли предков
            var agencyDict = GetDictionary<Agency>().Select(x => (int?)x.Id).ToList();
            var allRoles = db.GetDomainTable<DbRole>()
                             .Where(x => x.AgencyId == null || agencyDict.Contains(x.AgencyId))
                             .ToDictionary(x => x.Id, x => x);
            var roleRelations = db.GetDomainTable<DbRoleChild>().ToList();

            foreach (var role in allRoles.Values)
            {
                var subRoleIds = (from rr in roleRelations
                                  where rr.RoleId == role.Id
                                  select rr.ChildRoleId).ToList();

                foreach (var subRoleId in subRoleIds)
                {
                    if (role.ChildRoles == null)
                        role.ChildRoles = new EditableList<DbRole>();

                    role.ChildRoles.Add(allRoles[subRoleId]);
                }

                role.AcceptChanges();
            }

            return allRoles.Values.ToList<IDomainObject>();
        }

        private List<IDomainObject> GetDbUserList(IDomainDbManager dbManager)
        {
            var result =  dbManager.GetDomainTable<DbUser>()
                            .Select(t => t.Id)
                            .ToList()
                            .Select(t => dbManager.RetrieveDomainObject<DbUser>(t))
                            .OrderBy(t => t.Name)
                            .Cast<IDomainObject>()
                            .ToList();

            return result;
        }

        #endregion

        #region Enum dictionaries

        private Dictionary<int, string> GetAttrDataTypeList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list[1] = "Строка";
            list[2] = "Число";
            list[3] = "Дата";
            list[4] = "Флаг";
            list[5] = "Файл";
            list[6] = "Список";
            return list;
        }

        private Dictionary<int, string> GetTaskStatusList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list[1] = "Не заполнена";
            list[2] = "Ожидает проверки";
            list[3] = "Принята к рассмотрению";
            list[4] = "В работе";
            list[5] = "Готово для получения";
            list[6] = "Услуга предоставлена";
            list[7] = "В услуге отказано";
            return list;
        }

        private Dictionary<int, string> GetDbUserStatusList()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list[1] = "Активный";
            list[2] = "Заблокированный";
            list[3] = "Удалённый";
            return list;
        }

        #endregion
    }
}
