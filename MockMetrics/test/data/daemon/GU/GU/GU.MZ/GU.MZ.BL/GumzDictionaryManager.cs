using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;

using GU.DataModel;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL
{
    /// <summary>
    /// Класс, предназначенный для хранения справочных данных схемы GUMZ
    /// </summary>
    public class GumzDictionaryManager : AbstractDictionaryManager
    {
        /// <summary>
        /// Класс, предназначенный для получения значений справочных таблиц БД.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <exception cref="BLLException">Ошибка заполнения кэша справочников</exception>
        public GumzDictionaryManager(IDomainDbManager dbManager)
        {
            try
            {
                GetScenarioList(dbManager);

                _dictionaries[typeof(LicenseStatus)] = GetDomainList<LicenseStatus>(dbManager);
                _dictionaries[typeof(LicensedActivity)] = GetDomainList<LicensedActivity>(dbManager);
                _dictionaries[typeof(LicensedSubactivity)] = GetLicensedSubactivityList(dbManager);
                _dictionaries[typeof(SubactivityGroup)] = GetDomainList<SubactivityGroup>(dbManager);
                _dictionaries[typeof(LicenseObjectStatus)] = GetDomainList<LicenseObjectStatus>(dbManager);
                _dictionaries[typeof(AccreditateActivity)] = GetDomainList<AccreditateActivity>(dbManager);
                _dictionaries[typeof(Employee)] = GetEmployeeList(dbManager);
                _dictionaries[typeof(LegalForm)] = GetDomainList<LegalForm>(dbManager);
                _dictionaries[typeof(ServiceResult)] = GetDomainList<ServiceResult>(dbManager);
                _dictionaries[typeof(ExpertedDocument)] = GetDomainList<ExpertedDocument>(dbManager);
                _dictionaries[typeof(StandartOrderOption)] = GetDomainList<StandartOrderOption>(dbManager);
                _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
                _enumDictionaries[typeof(ExpertStateType)] = GetEnumList(typeof(ExpertStateType));
                _enumDictionaries[typeof(NoticeType)] = GetEnumList(typeof(NoticeType));
                _enumDictionaries[typeof(LicenseStatusType)] = GetEnumList(typeof(LicenseStatusType));

                _dynamicDictionaries[typeof(Employee)] = GetEmployeeList;
                _dynamicDictionaries[typeof(Expert)] = GetExpertList;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка заполнения кэша справочников", ex);
            }
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
        /// Возвращает словарь для перечисления
        /// </summary>
        /// <returns>Список доменных объектов</returns>
        private Dictionary<int, string> GetEnumList(Type enumType)
        {
            var enumDictionary = new Dictionary<int, string>();
            foreach (Enum enumValue in Enum.GetValues(enumType))
            {
                enumDictionary[Convert.ToInt32(enumValue)] = enumValue.GetDescription();
            }
            return enumDictionary;
        }

        /// <summary>
        /// Возвращает справочник сценариев ведения тома.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Справочник сценариев ведения тома</returns>
        private void GetScenarioList(IDomainDbManager dbManager)
        {
            var result = dbManager.GetDomainTable<Scenario>().ToList();
            result.ForEach(r => r.ScenarioStepList = dbManager.GetDomainTable<ScenarioStep>()
                                                              .Where(s => s.ScenarioId == r.Id)
                                                              .ToList());
            foreach (var scenario in result)
            {
                scenario.ScenarioStepList = dbManager.GetDomainTable<ScenarioStep>()
                                                     .Where(s => s.ScenarioId == scenario.Id)
                                                     .ToList();

                foreach (var scenarioStep in scenario.ScenarioStepList)
                {
                    scenarioStep.Scenario = scenario;
                }
            }

            _dictionaries[typeof(Scenario)] = result.Cast<IDomainObject>().ToList();
            _dictionaries[typeof(ScenarioStep)] = result.SelectMany(t => t.ScenarioStepList).Cast<IDomainObject>().ToList();

        }

        /// <summary>
        /// Возвращает справочник сотрудников.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Справочник сотрудников</returns>
        private List<IDomainObject> GetEmployeeList(IDomainDbManager dbManager)
        {
            var result = dbManager.GetDomainTable<Employee>().ToList();
            foreach (var employee in result)
            {
                employee.DbUser = dbManager.RetrieveDomainObject<DbUser>(employee.DbUserId);
            }

            return result.OrderBy(t => t.Name).Cast<IDomainObject>().ToList();
        }

        private List<IDomainObject> GetExpertList(IDomainDbManager dbManager)
        {
            var experts = dbManager.GetDomainTable<Expert>()
                .Select(t => t.Id)
                .ToList()
                .Select(t =>
                {
                    var expert = dbManager.RetrieveDomainObject<Expert>(t);

                    if (expert.ExpertStateType == ExpertStateType.Individual)
                    {
                        expert.ExpertState = dbManager.RetrieveDomainObject<IndividualExpertState>(t);
                    }
                    else
                    {
                        expert.ExpertState = dbManager.RetrieveDomainObject<JuridicalExpertState>(t);
                    }

                    return expert;
                });

            return experts.Cast<IDomainObject>().ToList();
        }

        /// <summary>
        /// Возвращает справочник лицензируемых поддеятельностей.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Справочник поддеятельностей</returns>
        private List<IDomainObject> GetLicensedSubactivityList(IDomainDbManager dbManager)
        {
            var result = dbManager.GetDomainTable<LicensedSubactivity>().ToList();
            foreach (var licensedSubactivity in result)
            {
                licensedSubactivity.SubactivityGroup = dbManager.RetrieveDomainObject<SubactivityGroup>(licensedSubactivity.SubactivityGroupId);
            }

            return result.OrderBy(t => t.SubactivityGroup.Id).ThenBy(t => t.Name).Cast<IDomainObject>().ToList();
        }
    }
}
