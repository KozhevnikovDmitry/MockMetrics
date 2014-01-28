using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using Common.Types.Exceptions;

using GU.DataModel;
using GU.MZ.BL.Reporting.Data;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;
using GU.BL.Extensions;
using GU.MZ.DataModel.Requisites;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Полный отчёт по лицензированию по виду деятельности".
    /// </summary>
    public class FullActivityDataReport : BaseReport
    {
        /// <summary>
        /// Начальная дата промежутка для отчёта
        /// </summary>
        private DateTime _date1;

        /// <summary>
        /// Конечная дата промежутка для отчёта
        /// </summary>
        private DateTime _date2;

        /// <summary>
        /// Лицензируемая деятельность
        /// </summary>
        private LicensedActivity _licensedActivity;

        private readonly Func<IDomainDbManager> _getDb;

        /// <summary>
        /// Менеджер кэша справочников
        /// </summary>
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Маппер экспертов
        /// </summary>
        private readonly IDomainDataMapper<Expert> _expertMapper;

        /// <summary>
        /// Класс отчёт "Полный отчёт по лицензированию по виду деятельности".
        /// </summary>S
        /// <param name="getDb"></param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        /// <param name="expertMapper">Маппер экспертов</param>
        /// <param name="date2">Конечная дата промежутка для отчёта</param>
        /// <param name="licensedActivity">Лицензируемая деятельность</param>
        /// <param name="date1">Начальная дата промежутка для отчёта</param>
        public FullActivityDataReport(Func<IDomainDbManager> getDb,
                                      IDictionaryManager dictionaryManager,
                                      IDomainDataMapper<Expert> expertMapper)
        {
            _getDb = getDb;
            _dictionaryManager = dictionaryManager;
            _expertMapper = expertMapper;
            ViewPath = "Reporting/View/GU.MZ/FullActivityDataReport.mrt";
        }

        public IReport Initialize(LicensedActivity licensedActivity, DateTime date1, DateTime date2)
        {
            _date1 = date1;
            _date2 = date2;
            _licensedActivity = licensedActivity;
            return this;
        }

        /// <summary>
        /// Вовзращает данные для отчёта "Полный отчёт по лицензированию по виду деятельности".
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Объект с информацией для отчёта</returns>
        public override object RetrieveData()
        {
            using (var dbManager = _getDb())
            {
                var dossierFiles =
                    dbManager.GetDomainTable<DossierFile>()
                             .Where(t => t.LicenseDossier.LicensedActivityId == _licensedActivity.Id &&
                                         t.Task.CreateDate > _date1 &&
                                         t.Task.CreateDate < _date2)
                             .ToList();

                var result = new FullActivityData { Details = new List<FullActivityDataDetail>() };

                foreach (var dossierFile in dossierFiles)
                {
                    FillDossierFile(dossierFile, dbManager);
                    var detail = CreateReportDetail(dossierFile, dbManager);
                    result.Details.Add(detail);
                }

                return result;
            }
        }

        /// <summary>
        /// Заполняет ассоциации тома - заявка и CommonData
        /// </summary>
        /// <param name="dossierFile">Том</param>
        /// <param name="dbManager">Менеджер БД</param>
        private void FillDossierFile(DossierFile dossierFile, IDomainDbManager dbManager)
        {
            dossierFile.Task = dbManager.RetrieveDomainObject<Task>(dossierFile.TaskId);
            dossierFile.Task.CommonData =
                dbManager.GetDomainTable<CommonData>()
                         .Where(t => t.KeyValue == dossierFile.TaskId.ToString() && t.Entity == typeof(Task).Name)
                         .OrderBy(t => t.Stamp)
                         .First();
        }

        /// <summary>
        /// Возвращает объект детализацию отчёта по одному тому лицензионного дела.
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД<</param>
        /// <returns>Детализация отчёта</returns>
        private FullActivityDataDetail CreateReportDetail(DossierFile dossierFile, IDomainDbManager dbManager)
        {
            var result = new FullActivityDataDetail();

            FillDossierFileData(result, dossierFile, dbManager);

            FillTaskData(result, dossierFile, dbManager);

            if (dossierFile.LicenseDossierId.HasValue)
            {
                FillLicenseDossierData(result, dossierFile, dbManager);
            }

            var scenario = _dictionaryManager.GetDictionaryItem<Scenario>(dossierFile.ScenarioId);

            if (scenario.ScenarioType == ScenarioType.Full)
            {
                FillFullSceanriedData(result, dossierFile, dbManager);
            }

            if (dossierFile.DossierFileServiceResultId.HasValue)
            {
                FillServiceResultData(result, dossierFile, dbManager);
            }

            return result;
        }

        #region DossierFile mapping

        /// <summary>
        /// Добавляет в детализацию данные из тома лицензионного дела.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillDossierFileData(FullActivityDataDetail detail,
                                                           DossierFile dossierFile,
                                                           IDomainDbManager dbManager)
        {
            detail.RegNumber = dossierFile.RegNumber.ToString().PadLeft(4, '0');

            var inventory =
                dbManager.GetDomainTable<DocumentInventory>().Single(t => t.Id == dossierFile.Id);

            detail.Stamp = inventory.Stamp.ToShortDateString();

            var respEmp = _dictionaryManager.GetDictionaryItem<Employee>(dossierFile.EmployeeId);

            detail.ResponsibleEmployeeData = string.Format("{0}, {1}", respEmp.Name, respEmp.Position);

            return detail;
        }

        #endregion

        #region Task mapping

        /// <summary>
        /// Добавляет в детализацию данные из заявки.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillTaskData(FullActivityDataDetail detail, 
                                                    DossierFile dossierFile, 
                                                    IDomainDbManager dbManager)
        {
            detail.TaskDueDate = dossierFile.Task.DueDate.HasValue
                                     ? dossierFile.Task.DueDate.Value.ToShortDateString()
                                     : DateTime.MaxValue.ToShortDateString();

            var service = _dictionaryManager.GetDictionaryItem<Service>(dossierFile.Task.ServiceId);
            
            detail.ServiceName = service.Name;
            detail.ServicePeriod = service.Duration;

            FillAccepterData(detail, dossierFile);

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о приёмщике заявления.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillAccepterData(FullActivityDataDetail detail,
                                                        DossierFile dossierFile)
        {
            var acceptEmp =
                _dictionaryManager.GetDictionary<Employee>()
                    .SingleOrDefault(t => t.DbUser.Name.ToUpper() == dossierFile.Task.CommonData.User.ToUpper());


            detail.AccepterEmployeeData = acceptEmp != null
                                              ? string.Format("{0}, {1}", acceptEmp.Name, acceptEmp.Position)
                                              : string.Format(
                                                  "Ошибка, сотрудник по логину {0} не найден",
                                                  dossierFile.Task.CommonData.User.ToUpper());

            return detail;
        }

        #endregion

        #region ServiceResult mapping
        
        /// <summary>
        /// Добавляет в детализацию данные о результате предоставления услуги.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillServiceResultData(FullActivityDataDetail detail,
                                                             DossierFile dossierFile,
                                                             IDomainDbManager dbManager)
        {
            var fileResult =
                dbManager.GetDomainTable<DossierFileServiceResult>()
                         .Single(t => t.Id == dossierFile.DossierFileServiceResultId);

            var serviceResult = _dictionaryManager.GetDictionaryItem<ServiceResult>(fileResult.ServiceResultId);

            detail.ServiceResult = serviceResult.Name;
            detail.DoneData = string.Format("{0}, {1}", fileResult.Stamp.ToShortDateString(), fileResult.Note);

            FillGrantResultOrderData(detail, dossierFile, dbManager);

            FillTaskWorkingDaysData(detail, dossierFile, fileResult.Stamp);

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о приказе на предоставление услуги.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillGrantResultOrderData(FullActivityDataDetail detail,
                                                                DossierFile dossierFile,
                                                                IDomainDbManager dbManager)
        {
            var order = dbManager.GetDomainTable<StandartOrder>()
                                 .Where(t => t.FileScenarioStep.DossierFileId == dossierFile.Id)
                                 .Where(t => t.OrderOption.Id == (int)StandartOrderType.GrantLicense
                                          || t.OrderOption.Id == (int)StandartOrderType.NotGrantLicense
                                          || t.OrderOption.Id == (int)StandartOrderType.RenewalLicense
                                          || t.OrderOption.Id == (int)StandartOrderType.NotRenewalLicense
                                          || t.OrderOption.Id == (int)StandartOrderType.StopLicense)
                                 .SingleOrDefault();

            detail.GrantResultOrderRegNumber = order.RegNumber;
            detail.GrantResultOrderStamp = order.Stamp.ToShortDateString();

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о фактическом сроке предоставления услуги.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="resultStmap">Дата предоставления результата услуги</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillTaskWorkingDaysData(FullActivityDataDetail detail, 
                                                               DossierFile dossierFile, 
                                                               DateTime resultStmap)
        {
            try
            {
                detail.TaskWorkDays = dossierFile.Task.CreateDate.HasValue
                                          ? dossierFile.Task.CreateDate.Value.GetWorkingDaysTo(resultStmap)
                                          : -100500;
            }
            catch (BLLException)
            {
                detail.TaskWorkDays = -100500;
            }

            return detail;
        }


        #endregion

        #region LicenseDossier mapping

        /// <summary>
        /// Добавляет в детализацию данные из лицензионного дела.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillLicenseDossierData(FullActivityDataDetail detail,
                                                              DossierFile dossierFile,
                                                              IDomainDbManager dbManager)
        {
            var dossierRegNumber =
                dbManager.GetDomainTable<LicenseDossier>().Where(t => t.Id == dossierFile.LicenseDossierId).Select(t => t.RegNumber).Single();

            var holderQuery =
                dbManager.GetDomainTable<HolderRequisites>().Where(t => t.Id == dossierFile.HolderRequisitesId)
                          .Join(dbManager.GetDomainTable<LicenseHolder>(), requisites => requisites.LicenseHolderId, holder => holder.Id, (requisites, holder) => new {requisites, holder}).Single();

            if (holderQuery.requisites.JurRequisitesId.HasValue)
            {
                var jurRequisites =
                    dbManager.GetDomainTable<JurRequisites>()
                        .Single(t => t.Id == holderQuery.requisites.JurRequisitesId);
                detail.HolderFullName = jurRequisites.FullName;
                detail.LegalForm = _dictionaryManager.GetDictionaryItem<LegalForm>(jurRequisites.LegalFormId).Name;
            }
            else
            {
                var indRequisites =
                   dbManager.GetDomainTable<IndRequisites>()
                       .Single(t => t.Id == holderQuery.requisites.IndRequisitesId);
                detail.HolderFullName = indRequisites.ToString();
                detail.LegalForm = "ИП";
            }

            detail.LicenseDossierRegNumber = dossierRegNumber;
            detail.Inn = holderQuery.holder.Inn;
            return detail;
        }

        #endregion

        #region Violation mapping

        /// <summary>
        /// Добавляет в детализацию данные о нарушениях в документах заявки.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillViolationData(FullActivityDataDetail detail,
                                                         DossierFile dossierFile,
                                                         IDomainDbManager dbManager)
        {
            FillViolationNoticeData(detail, dossierFile, dbManager);

            FillViolationOrderData(detail, dossierFile, dbManager);

            FillViolationResolveData(detail, dossierFile, dbManager);

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о уведомлении об устранении нарушений.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillViolationNoticeData(FullActivityDataDetail detail, 
                                                               DossierFile dossierFile, 
                                                               IDomainDbManager dbManager)
        {
            Notice violationNotice = null;
            violationNotice = (from not in dbManager.GetDomainTable<Notice>()
                               join st in dbManager.GetDomainTable<DossierFileScenarioStep>() on not.Id equals st.Id
                               where st.DossierFileId == dossierFile.Id 
                                     && not.NoticeType == NoticeType.RejectDocuments
                               orderby st.Id 
                               select not).FirstOrDefault();


            if (violationNotice != null)
            {
                detail.ViolationNoticeStamp =
                    dbManager.GetDomainTable<Notice>()
                             .Where(t => t.Id == violationNotice.Id)
                             .Select(t => t.Stamp)
                             .Single()
                             .ToShortDateString();
            }

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о приказе о приёме или возврате документов.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillViolationOrderData(FullActivityDataDetail detail, 
                                                              DossierFile dossierFile, 
                                                              IDomainDbManager dbManager)
        {
            var order = dbManager.GetDomainTable<StandartOrder>()
                                 .Where(t => t.FileScenarioStep.DossierFileId == dossierFile.Id)
                                 .Where(t => t.OrderOption.Id == (int)StandartOrderType.AcceptTask
                                          || t.OrderOption.Id == (int)StandartOrderType.ReturnTask)
                                 .SingleOrDefault();

            if (order != null)
            {
                detail.AcceptRejectOrderRequisites = string.Format(
                    "от {0} № {1}", order.Stamp.ToShortDateString(), order.RegNumber);
            }

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные об устранении нарушений.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillViolationResolveData(FullActivityDataDetail detail, 
                                                                DossierFile dossierFile, 
                                                                IDomainDbManager dbManager)
        {
            var violationResolveInfo = (from vri in dbManager.GetDomainTable<ViolationResolveInfo>()
                                        join st in dbManager.GetDomainTable<DossierFileScenarioStep>() on vri.Id equals
                                            st.Id
                                        where st.DossierFileId == dossierFile.Id
                                        select vri).SingleOrDefault();

            if (violationResolveInfo != null)
            {
                if (violationResolveInfo.IsResolved)
                {
                    detail.ViolationResolveStamp = violationResolveInfo.ResolveStamp.HasValue
                                                       ? violationResolveInfo.ResolveStamp.Value.ToShortDateString()
                                                       : string.Empty;
                }
                else
                {
                    detail.ReturnDocumentsStamp = violationResolveInfo.ReturnStamp.HasValue
                                                      ? violationResolveInfo.ReturnStamp.Value.ToShortDateString()
                                                      : string.Empty;
                }
            }

            return detail;
        }

        #endregion

        #region Fullscenaried mapping

        /// <summary>
        /// Добавляет в детализацию данные об устранении нарушений.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillFullSceanriedData(FullActivityDataDetail detail,
                                                             DossierFile dossierFile,
                                                             IDomainDbManager dbManager)
        {
            FillViolationData(detail, dossierFile, dbManager);
            FillProvideExpertiseData(detail, dossierFile, dbManager);
            FillProvideInspectionData(detail, dossierFile, dbManager);

            return detail;
        }

        #region Document expertise mapping

        /// <summary>
        /// Добавляет в детализацию данные о проведении документарной проверке.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillProvideExpertiseData(FullActivityDataDetail detail,
                                                                 DossierFile dossierFile,
                                                                 IDomainDbManager dbManager)
        {
            var order = dbManager.GetDomainTable<ExpertiseOrder>()
                .Join(dbManager.GetDomainTable<DossierFileScenarioStep>().Where(t => t.DossierFileId == dossierFile.Id),
                    o => o.Id,
                    s => s.Id,
                    (o, s) => o)
                .SingleOrDefault();

            if (order != null)
            {
                detail.DocumentExpertiseOrderRequisites = string.Format(
                    "от {0} № {1}", order.Stamp.ToShortDateString(), order.RegNumber);

                FillExpertiseData(detail, dossierFile, dbManager);
            }

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о документарной проверке.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillExpertiseData(FullActivityDataDetail detail, 
                                                         DossierFile dossierFile, 
                                                         IDomainDbManager dbManager)
        {
            var expertise = (from exp in dbManager.GetDomainTable<DocumentExpertise>()
                             join st in dbManager.GetDomainTable<DossierFileScenarioStep>() on exp.Id equals st.Id
                             where st.DossierFileId == dossierFile.Id
                             select exp).SingleOrDefault();

            if (expertise != null)
            {
                detail.DocumentExpertiseActStamp = expertise.ActStamp.ToShortDateString();
                detail.DocumentExpertisePeriod = string.Format(
                    "c {0} по {1}", expertise.StartStamp, expertise.EndStamp);
            }

            return detail;
        }

        #endregion

        #region Inspection mapping

        /// <summary>
        /// Добавляет в детализацию данные о проведении выездной проверке.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillProvideInspectionData(FullActivityDataDetail detail,
                                                          DossierFile dossierFile,
                                                          IDomainDbManager dbManager)
        {
            var order = dbManager.GetDomainTable<InspectionOrder>()
                .Join(dbManager.GetDomainTable<DossierFileScenarioStep>().Where(t => t.DossierFileId == dossierFile.Id),
                      o => o.Id,
                      s => s.Id,
                      (o, s) => o)
                .SingleOrDefault();

            if (order != null)
            {
                detail.InspectionOrderRequisites = string.Format(
                    "от {0} № {1}", order.Stamp.ToShortDateString(), order.RegNumber);

                FillInspectionData(detail, dossierFile, dbManager);
            }

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные о выездной проверке.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillInspectionData(FullActivityDataDetail detail,
                                                          DossierFile dossierFile,
                                                          IDomainDbManager dbManager)
        {
            var inspection = (from insp in dbManager.GetDomainTable<Inspection>()
                              join st in dbManager.GetDomainTable<DossierFileScenarioStep>() on insp.Id equals st.Id
                              where st.DossierFileId == dossierFile.Id
                              select insp).SingleOrDefault();

            if (inspection != null)
            {
                detail.InspectionActStamp = inspection.ActStamp.ToShortDateString();

                detail.InspectionPeriod = string.Format("c {0} по {1}", inspection.StartStamp, inspection.EndStamp);

                FillInspectionExpertsData(detail, inspection, dbManager);
            }

            return detail;
        }

        /// <summary>
        /// Добавляет в детализацию данные об экспертах привлечённых к выездной проверке.
        /// </summary>
        /// <param name="detail">Детализация отчёта</param>
        /// <param name="inspection">Выездная проверка</param>
        /// <param name="dbManager">Менеджер БД</param>
        /// <returns>Дополненная детализация</returns>
        private FullActivityDataDetail FillInspectionExpertsData(FullActivityDataDetail detail,
                                                                 Inspection inspection,
                                                                 IDomainDbManager dbManager)
        {
            var expertsInfo = (from exp in dbManager.GetDomainTable<Expert>()
                                join ins in dbManager.GetDomainTable<InspectionExpert>() on exp.Id equals
                                    ins.ExpertId
                                where ins.InspectionId == inspection.Id
                                select new { exp.Id, ins.ExpertName, exp.ExpertStateType }).ToList();

            foreach (var info in expertsInfo)
            {
                if (info.ExpertStateType == ExpertStateType.Individual 
                    && !string.IsNullOrEmpty(info.ExpertName))
                {
                    detail.InvolvedExpertsData += info.ExpertName;
                    continue;
                }

                var expert = _expertMapper.Retrieve(info.Id, dbManager);
                detail.InvolvedExpertsData += expert.ExpertState.GetName();
            }

            return detail;
        }

        #endregion

        #endregion
    }
}
