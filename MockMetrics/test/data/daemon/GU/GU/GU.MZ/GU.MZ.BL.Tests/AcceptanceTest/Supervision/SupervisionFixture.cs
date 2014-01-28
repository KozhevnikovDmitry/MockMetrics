using System;
using System.IO;
using System.Linq;
using Autofac;
using BLToolkit.EditableObjects;
using Common.Types;
using GU.BL.Policy;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;
using GU.MZ.DataModel.Requisites;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Базовый класс для классов с приёмочными тестами на ведение тома лицензионного дела
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public abstract class SupervisionFixture : MzAcceptanceTests
    {

        protected SupervisionFacade Superviser;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Superviser.Dispose();
        }


        #region Arrange

        /// <summary>
        /// Создаёт и сохраняет тестовыую заявку по xml представлению
        /// ИНН и ОГРН случайные
        /// </summary>
        protected Task ArrangeTask(int serviceId)
        {
            return ArrangeTask(serviceId, RandomProvider.RandomNumberString(11), RandomProvider.RandomNumberString(13));
        }

        protected virtual Task ArrangeTask(string contentPath)
        {
            return ArrangeTask(contentPath, RandomProvider.RandomNumberString(11), RandomProvider.RandomNumberString(13));
        }

        /// <summary>
        /// Создаёт и сохраняет тестовыую заявку по xml представлению с заданными ИНН и ОГРН
        /// </summary>
        protected Task ArrangeTask(int serviceId, string inn, string ogrn)
        {
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            var task = contentParser.ParseTask(serviceId, inn, ogrn);
            return CompleteParsedTask(task);
        }

        protected Task ArrangeTask(string contentPath, string inn, string ogrn)
        {
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            var task = contentParser.ParseTask(contentPath, inn, ogrn);
            return CompleteParsedTask(task);
        }

        private Task CompleteParsedTask(Task paresedTask)
        {
            paresedTask.CustomerFio = "Джигурда";
            paresedTask.CustomerEmail = "gigurda@gmail.ru";
            paresedTask.CustomerPhone = "100500100500";
            var task = MzLogicFactory.ResolveDataMapper<Task>().Save(paresedTask);
            return task;
        }

        #endregion


        #region Act

        #region Supervision

        /// <summary>
        /// Осуществляет сохранение тома.
        /// </summary>
        protected void ActSaving()
        {
            if (Superviser.IsSpecificMapping)
            {
                Superviser.SaveDossierFile();
            }
            else
            {
                Superviser.DefaultSave();
            }
        }

        /// <summary>
        /// Переводит том на следующий этап сценария ведения
        /// </summary>
        protected void ActSupervisionStepNext()
        {
            Superviser.StepNext(Superviser.DossierFile.Employee);
            ActSaving();
        }

        /// <summary>
        /// Переводит том на следующий этап сценария ведения с выставлением статуса В работе
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <param name="status">Статус</param>
        protected void ActSupervisionStepNextWithStatus(TaskStatusType status)
        {
            Superviser.StepNextWithStatus(Superviser.DossierFile.Employee, status);
        }

        #endregion


        #region Accepting

        private bool IsScenarioFull(int serviceId)
        {
            return DictionaryManager.GetDictionary<Scenario>()
                                         .Single(t => t.ServiceId == serviceId).ScenarioType
                   == ScenarioType.Full;
        }

        /// <summary>
        /// Осуществляет принятие заявки к рассмотрению.
        /// </summary>
        /// <param name="task">Новая заявка</param>
        /// <returns>Том лицензионного дела созданный на основании принятой заявки</returns>
        protected DossierFile ActAccepting(Task task)
        {
            var file =
                MzLogicFactory.GetDossierFileBuilder()
                              .FromTask(task)
                              .ToEmployee(DictionaryManager.GetDynamicDictionary<Employee>().First())
                              .WithInventoryRegNumber(task.Id)
                              .WithAcceptedStatus("Принято к рассмотрению")
                              .AddProvidedDocument("Заявление", 1)
                              .AddProvidedDocument("Документ о занесении в реестр ОГРН", 1)
                              .AddProvidedDocument("Документ о постановке на учёт в налоговой", 1)
                              .Create();

            file = MzLogicFactory.GetDossierFileRegistr()
                                 .AcceptDossierFile(file);
            Superviser = MzLogicFactory.GetDossierFileSuperviser(file);

            return file;
        }

        /// <summary>
        /// Создаёт и сохраняет Лицензиата с реквизитами юр. лица по данным заявки
        /// </summary>
        protected LicenseHolder ActCreateJuridicalLicenseHolder(Task task)
        {
            var taskParser = MzLogicFactory.GetTaskParser();
            LicenseHolder licenseHolder;
            if (IsScenarioFull(task.ServiceId))
            {
                licenseHolder = taskParser.ParseHolder(task);
            }
            else
            {
                var holderInfo = taskParser.ParseHolderInfo(task);
                licenseHolder = LicenseHolder.CreateInstance();
                licenseHolder.Ogrn = holderInfo.Ogrn;
                licenseHolder.Inn = holderInfo.Inn;
                var requisites = HolderRequisites.CreateInstance();
                var jurRequisites = JurRequisites.CreateInstance();

                jurRequisites.FullName = holderInfo.FullName;
                jurRequisites.ShortName = holderInfo.FullName;
                jurRequisites.FirmName = holderInfo.FullName;
                jurRequisites.LegalFormId = 1;
                jurRequisites.HeadName = "Пигурда О.Щ.";
                jurRequisites.HeadPositionName = "Генеральный секретарь ЦК";
                requisites.JurRequisites = jurRequisites;
                requisites.Address = Address.CreateInstance();
                requisites.Address.City = "Красноярск";
                requisites.Address.Country = "Россия";
                requisites.Address.Street = "Парижской Коммуны";
                requisites.Address.Zip = "660049";
                licenseHolder.RequisitesList = new EditableList<HolderRequisites>();
                licenseHolder.RequisitesList.Add(requisites);
            }
            return MzLogicFactory.ResolveDataMapper<LicenseHolder>().Save(licenseHolder);
        }

        /// <summary>
        /// Создаёт и сохраняет Лицензионное дело для лицензиата 
        /// Заводит один том в лицензионном деле
        /// </summary>
        protected LicenseDossier ActCreateExistingLicenseDossier(LicenseHolder holder, Task task)
        {
            var licenseDossier = LicenseDossier.CreateInstance();
            licenseDossier.RegNumber = string.Format("ЛО-24-01-");
            licenseDossier.LicenseHolder = holder;
            licenseDossier.LicenseHolderId = holder.Id;
            licenseDossier.IsActive = true;
            licenseDossier.LicensedActivity =
                DictionaryManager.GetDictionary<LicensedActivity>()
                    .Single(l => l.ServiceGroupId == task.Service.ServiceGroupId);

            licenseDossier.LicensedActivityId = licenseDossier.LicensedActivity.Id;

            licenseDossier.RegNumber = string.Format("ЛО-24-{0}-{1}", licenseDossier.LicensedActivity.Code, holder.Inn);

            var result = MzLogicFactory.ResolveDataMapper<LicenseDossier>().Save(licenseDossier);

            var builder = MzLogicFactory.GetDossierFileBuilder()
                                        .FromTask(task)
                                        .ToEmployee(DictionaryManager.GetDynamicDictionary<Employee>().First())
                                        .WithInventoryRegNumber(100500)
                                        .WithAcceptedStatus("Тестовый том")
                                        .AddProvidedDocument("Заявление", 1);

            var oldFile = MzLogicFactory.GetDossierFileRegistr()
                                        .AcceptDossierFile(builder.Create());

            oldFile.RegNumber = 1;
            oldFile.LicenseDossierId = result.Id;
            oldFile.HolderRequisitesId = holder.RequisitesList.Single().Id;
            result.DossierFileList.Add(oldFile);
            MzLogicFactory.GetTaskPolicy().SetStatus(TaskStatusType.Rejected, "Тестовая заявка залочена", oldFile.Task);
            MzLogicFactory.ResolveDataMapper<DossierFile>().Save(oldFile);

            result = MzLogicFactory.ResolveDataMapper<LicenseDossier>().Save(result);

            return result;
        }

        protected License ActCreateExistingLicense(LicenseDossier dossier)
        {
            var license = License.CreateInstance();
            license.RegNumber = "100500";
            license.GrantDate = new DateTime(2013, 09, 1);
            license.DueDate = null;
            license.GrantOrderStamp = DateTime.Today;
            license.GrantOrderRegNumber = "100005";
            license.LicensiarHeadName = "Ы.Х. Плигурда";
            license.LicensiarHeadPosition = "Исполняющий обязанности заместителя заведущего отдела";
            license.LicenseDossier = dossier;
            license.LicenseDossierId = dossier.Id;
            license.LicensedActivity = dossier.LicensedActivity;
            license.LicensedActivityId = dossier.LicensedActivityId;
            license.BlankNumber = "001";
            var requisites = dossier.LicenseHolder.RequisitesList.First().ToLicenseRequisites();
            license.LicenseRequisitesList.Add(requisites);
            license.AddStatus(LicenseStatusType.Active, DateTime.Now, "Опа-Джигурда");
            return MzLogicFactory.ResolveDataMapper<License>().Save(license);
        }

        protected int GetNewLicenseServiceId(int serviceId)
        {
            if (serviceId <= 5)
            {
                return 1;
            }

            if (serviceId <= 10)
            {
                return 6;
            }

            return 11;
        }

        #endregion


        #region Linkaging

        /// <summary>
        /// Осуществляет привязку.
        /// </summary>
        protected IDossierFileLinkWrapper ActLinkaging(RequisitesOrigin requisitesOrigin)
        {
            var wrapper = Superviser.Linkage();
            wrapper.SelectedRequisites = wrapper.AvailableRequisites[requisitesOrigin];
            wrapper.Linkage();
            return wrapper;
        }

        #endregion


        #region Violations

        /// <summary>
        /// Заводит информацию об успешном устранении выявленных нарушений
        /// </summary>
        protected void ActPrepareResolvedViolationNotice()
        {
            var violNotice = Superviser.PrepareViolationNotice(Superviser.DossierFile.CurrentScenarioStep);
            violNotice.Violations = "Незначительные нарушения";

            var violResolveInfo = Superviser.PrepareViolationResolveInfo(Superviser.DossierFile.CurrentScenarioStep);
            violResolveInfo.IsResolved = true;
            violResolveInfo.ResolveStamp = DateTime.Now;
            violResolveInfo.Note = "Всё исправили";
        }

        /// <summary>
        /// Заводит информацию о возврате документов из-за выявенных нарушений
        /// </summary>
        protected void ActPrepareNotResolvedViolationNotice()
        {
            var violNotice = Superviser.PrepareViolationNotice(Superviser.DossierFile.CurrentScenarioStep);
            violNotice.Violations = "Ужасные нарушения";

            var violResolveInfo = Superviser.PrepareViolationResolveInfo(Superviser.DossierFile.CurrentScenarioStep);
            violResolveInfo.IsResolved = false;
            violResolveInfo.ReturnStamp = DateTime.Now;
            violResolveInfo.Note = "Документы возвращены";
        }

        /// <summary>
        /// Создаёт уведомление о необходимости устранения нарушений
        /// </summary>
        protected void ActNoticeOfRejectDossierFile()
        {
            Superviser.AddNotice(NoticeType.RejectDocuments, Superviser.DossierFile.CurrentScenarioStep);
        }

        #endregion


        #region Order

        /// <summary>
        /// Издаёт приказ о возврате документов
        /// </summary>
        protected void ActReturnTaskOrder()
        {
            var order = Superviser.PrepareReturnTaskOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.RejectDocuments, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ о приёме документов
        /// </summary>
        protected void ActAcceptTaskOrder()
        {
            var order = Superviser.PrepareAcceptTaskOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.AcceptDocuments, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ о проведении документарной проверки
        /// </summary>
        protected void ActExpertiseOrder()
        {
            var order = Superviser.PrepareExpertiseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddExpertiseOrderAgree(order, DictionaryManager.GetDynamicDictionary<Employee>().First());
            Superviser.AddExpertiseOrderAgree(order, DictionaryManager.GetDynamicDictionary<Employee>().Last());
            Superviser.AddNotice(NoticeType.DocumentExpertise, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ о проведении выездной проверки
        /// </summary>
        protected void ActInspectionOrder()
        {
            var order = Superviser.PrepareInspectionOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddInspectionOrderAgree(order, DictionaryManager.GetDynamicDictionary<Employee>().First());
            Superviser.AddInspectionOrderAgree(order, DictionaryManager.GetDynamicDictionary<Employee>().Last());
            Superviser.AddNotice(NoticeType.PlaceInspection, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }


        /// <summary>
        /// Издаёт приказ о предоставлении лицензии
        /// </summary>
        protected void ActGrantLicenseOrder()
        {
            var order = Superviser.PrepareGrantLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.ServiceGrant, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ об отказе в предоставлении лицензии
        /// </summary>
        protected void ActNotGrantLicenseOrder()
        {
            var order = Superviser.PrepareNotGrantLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.ServiseReject, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ о переоформлении лицензии
        /// </summary>
        protected void ActRenewalLicenseOrder()
        {
            var order = Superviser.PrepareRenewalLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.ServiceGrant, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ об отказе в переоформлении лицензии
        /// </summary>
        protected void ActNotRenewalLicenseOrder()
        {
            var order = Superviser.PrepareNotRenewalLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.ServiseReject, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        /// <summary>
        /// Издаёт приказ о прекращении действия лицензии
        /// </summary>
        protected void ActStopLicenseOrder()
        {
            var order = Superviser.PrepareStopLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            Superviser.AddNotice(NoticeType.ServiceGrant, Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "№ 100500 лиц.";
        }

        #endregion


        #region Expertise

        /// <summary>
        /// Заводит документарную проверку, заносит результаты и сохраняет изменения.
        /// </summary>
        protected void ActProvideExpertise()
        {
            Superviser.PrepareExpertise(Superviser.DossierFile.CurrentScenarioStep);

            Superviser.AddExpertiseResult(Superviser.GetAvailableDocs().First(), Superviser.DossierFile.CurrentScenarioStep);
        }

        #endregion


        #region Inspection

        /// <summary>
        /// Заводит выездную проверку, заносит результаты и сохраняет изменения.
        /// </summary>
        protected void ActProvideInspection()
        {
            var scenarioStep = Superviser.DossierFile.CurrentScenarioStep;
            var inspection = Superviser.PrepareHolderInspection(Superviser.DossierFile.CurrentScenarioStep);
            inspection.InspectionNote = "Всё замечательно";
            inspection.IsPassed = true;

            Superviser.AddInspectionEmployee(Superviser.GetAvailableEmployees(scenarioStep).First(), scenarioStep);
            Superviser.AddInspectionEmployee(Superviser.GetAvailableEmployees(scenarioStep).First(), scenarioStep);
        }

        #endregion


        #region GrantResult

        /// <summary>
        /// Заводит результат предоставления услуги для тома. 
        /// </summary>
        protected void ActGrantServiceResult()
        {
            Superviser.GrantServiseResult();
            Superviser.DossierFile.DossierFileServiceResult.GrantWay = "Вручено";
            Superviser.DossierFile.DossierFileServiceResult.Stamp = DateTime.Now.AddDays(10);
            Superviser.SaveServiceResult();
        }

        /// <summary>
        /// Прекращает действие лиценхии и заводит результат предоставления услуги 
        /// </summary>
        protected void ActStopLicense()
        {
            ActGrantServiceResult();
            Superviser.StopLicense();
            Superviser.DefaultSave();
        }

        protected void ActRenewalLicense()
        {
            Superviser.GrantServiseResult();
            Superviser.DossierFile.DossierFileServiceResult.GrantWay = "Вручено";
            Superviser.DossierFile.DossierFileServiceResult.Stamp = DateTime.Now.AddDays(10);
            Superviser.RenewalLicense();
        }

        #endregion

        #endregion


        #region Assert

        #region Supervision

        protected void AssertStep(DossierFile dossierFile, int stepCount)
        {
            Assert.AreEqual(dossierFile.DossierFileStepList.Count, stepCount,
               string.Format("Assert that there are [0] steps", stepCount));

            Assert.AreEqual(
                dossierFile.CurrentScenarioStepId,
                DictionaryManager.GetDictionary<Scenario>()
                                      .Single(t => t.ServiceId == dossierFile.Service.Id)
                                      .ScenarioStepList.OrderBy(t => t.SortOrder)
                                      .ToList()[stepCount - 1].Id,
                "Assert that dossierFile.CurrentScenarioStep equals the forth step");
        }

        protected void AssertPersistence(DossierFile dossierFile)
        {
            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        protected void AssertFileCurrentStatus(DossierFile dossierFile, TaskStatusType taskStatusType)
        {
            Assert.AreEqual(dossierFile.TaskState, taskStatusType,
                string.Format("Assert that task is in {0} status", taskStatusType.GetDescription()));

            Assert.NotNull(dossierFile.Task.StatusList.Single(t => t.State == taskStatusType),
                 string.Format("Assert that task has {0} status in status list", taskStatusType.GetDescription()));
        }

        protected void AssertLicenseCurrentStatus(License license, LicenseStatusType licenseStatusType)
        {
            Assert.AreEqual(license.CurrentStatus, licenseStatusType,
                  string.Format("Assert that license is in {0} status", licenseStatusType.GetDescription()));

            Assert.AreEqual(license.CurrentLicenseStatus.LicenseStatusType, licenseStatusType,
                string.Format("Assert that license has {0} status in status list", licenseStatusType.GetDescription()));
        }

        #endregion


        #region Notice

        protected void AssertNotice(DossierFile dossierFile, NoticeType noticeType)
        {
            var currentStep = dossierFile.CurrentFileStep;

            Assert.NotNull(currentStep.Notice,
               "Assert that there is a notice in current step");

            var notice = currentStep.Notice;

            Assert.AreEqual(notice.Stamp.Date, DateTime.Today,
                "Assert that notice is today dated");

            Assert.AreEqual(notice.NoticeType, noticeType,
                "Assert that notice is reject notice");

            Assert.AreEqual(notice.Email, dossierFile.Task.CustomerEmail,
                "Assert that notice may send to customer email");

            Assert.That(string.IsNullOrEmpty(notice.PostRequisites),
                "Assert that notice has empty postrequisites");

            Assert.AreEqual(notice.Address, dossierFile.HolderRequisites.Address.ToLongString(),
                "Assert that notice address equals holder requisites address");
        }

        protected void AssertViolationNotice(DossierFile dossierFile, string violations)
        {
            var currentStep = dossierFile.CurrentFileStep;

            Assert.NotNull(currentStep.ViolationNotice,
                "Assert that there is a violation notice in current step");

            var violNotice = currentStep.ViolationNotice;

            Assert.AreNotEqual(violNotice.TaskRegNumber, 0,
                "Assert violNotice.TaskRegNumber");
            Assert.AreEqual(violNotice.TaskRegNumber, dossierFile.TaskId,
                "Assert violNotice.TaskRegNumber");

            Assert.NotNull(violNotice.TaskStamp,
                "Assert violNotice.TaskStamp");
            Assert.AreEqual(violNotice.TaskStamp, dossierFile.TaskStamp,
                "Assert violNotice.TaskStamp");

            Assert.NotNull(violNotice.LicenseHolder,
                "Assert violNotice.LicenseHolder");
            Assert.AreEqual(violNotice.LicenseHolder, dossierFile.HolderFullName,
                "Assert violNotice.LicenseHolder");

            Assert.NotNull(violNotice.Address,
                "Assert violNotice.Address");
            Assert.AreEqual(violNotice.Address, dossierFile.HolderAddress,
                "Assert violNotice.Address");

            Assert.NotNull(violNotice.LicensedActivity,
                "Assert violNotice.LicensedActivity");
            Assert.AreEqual(violNotice.LicensedActivity, dossierFile.LicensedActivityName,
                "Assert violNotice.LicensedActivity");

            Assert.AreEqual(violNotice.Violations, violations,
                "Assert violNotice.Violations");

            Assert.NotNull(violNotice.EmployeeName,
                "Assert violNotice.EmployeeName");
            Assert.AreEqual(violNotice.EmployeeName, dossierFile.EmployeeName,
                "Assert violNotice.EmployeeName");

            Assert.NotNull(violNotice.EmployeePosition,
                "Assert violNotice.EmployeePosition");
            Assert.AreEqual(violNotice.EmployeePosition, dossierFile.EmployeePosition,
                "Assert violNotice.EmployeePosition");
        }

        #endregion


        #region Order

        protected void AssertStandartOrder(DossierFile dossierFile, StandartOrderType orderType)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();

            Assert.AreEqual(order.Stamp.Date, DateTime.Today,
                "Order is published today");
            Assert.AreEqual(order.RegNumber, "№ 100500 лиц.",
                "Order regnumber is 100500");
            Assert.AreEqual(order.EmployeeName, dossierFile.EmployeeName,
                "Order employee name from dossierFile employee");
            Assert.AreEqual(order.EmployeePosition, dossierFile.EmployeePosition,
                "Order employee position from dossierFile employee");
            Assert.AreEqual(order.EmployeeContacts, dossierFile.EmployeeContacts,
                "Order employee contacts from dossierFile employee");
            Assert.IsNotNull(order.OrderOption,
                "Order has order option");

            var orderOption = DictionaryManager.GetDictionary<StandartOrderOption>(t => t.OrderType == orderType).Single();
            Assert.AreEqual(order.OrderOption, orderOption,
                "Order option equal option from dictionaryManager");
            Assert.AreEqual(order.OrderOptionId, order.OrderOption.Id,
                "Order option id is typed");

            Assert.AreEqual(order.ActivityInfo,
                string.Format("{0} {1}", dossierFile.LicensedActivity.BlankName,
                    dossierFile.LicensedActivity.AdditionalInfo).Trim(),
                "Order activity info from licensedActivity of dossierfile");

            Assert.AreEqual(order.FileScenarioStep, dossierFile.CurrentFileStep,
                "Order fileScenarioStep is typed");
            Assert.AreEqual(order.FileScenarioStepId, dossierFile.CurrentFileStep.Id,
                "Order fileScenarioStep id is typed");

            AssertLicensiarData(order);
        }

        private void AssertLicensiarData(StandartOrder order)
        {
            var chief = DictionaryManager.GetDynamicDictionary<Employee>().FirstOrDefault(t => !t.ChiefId.HasValue);

            if (chief == null)
            {
                Assert.AreEqual(order.LicensiarHeadName, string.Empty,
                    "Order licensial head name is empty. There is no chief");
                Assert.AreEqual(order.LicensiarHeadPosition, string.Empty,
                    "Order licensial head position is empty. There is no chief");
            }
            else
            {
                Assert.AreEqual(order.LicensiarHeadName, chief.Name,
                    "Order licensial head name is chief name");
                Assert.AreEqual(order.LicensiarHeadPosition, chief.Position,
                    "Order licensial head position is chief position");
            }
        }

        protected void AssertOrderDetail(DossierFile dossierFile)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            var detail = order.DetailList.SingleOrDefault();
            Assert.NotNull(detail, "Order has single detail");

            Assert.AreEqual(detail.Address, dossierFile.HolderAddress,
                "Detail address equals doaaierFile address");
            Assert.AreEqual(detail.FullName, dossierFile.HolderFullName,
                "Detail full name equals doaaierFile holder full name");
            Assert.AreEqual(detail.ShortName, dossierFile.HolderShortName,
                "Detail short name equals doaaierFile holder short name");
            Assert.AreEqual(detail.FirmName, dossierFile.HolderFirmName,
                "Detail firm name equals doaaierFile holder firm name");
            Assert.AreEqual(detail.Inn, dossierFile.HolderInn,
                "Detail inn equals doaaierFile holder inn");
            Assert.AreEqual(detail.Ogrn, dossierFile.HolderOgrn,
                "Detail ogrn equals doaaierFile holder ogrn");
            Assert.True(order.DetailList.Contains(detail),
                "Order contacts detail");
            Assert.AreEqual(detail.StandartOrder, order,
                "Detail order is typed");
        }

        protected void AssertOrderTaskSubject(DossierFile dossierFile)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            var detail = order.DetailList.SingleOrDefault();
            Assert.NotNull(detail, "Order has single detail");
            Assert.AreEqual(detail.SubjectId, dossierFile.TaskId.ToString(),
                "Detail subject id is dossierFile task id");
            Assert.AreEqual(detail.SubjectStamp, dossierFile.TaskStamp,
                "Detail subject stamp is dossierFile task stamp");
        }

        protected void AssertOrderLicenseSubject(DossierFile dossierFile)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            var detail = order.DetailList.SingleOrDefault();
            Assert.NotNull(detail, "Order has single detail");
            Assert.AreEqual(detail.SubjectId, dossierFile.LicenseId,
                "Detail subject id is dossierFile license id");
            Assert.AreEqual(detail.SubjectStamp, dossierFile.LicenseGrantDate,
                "Detail subject stamp is dossierFile license grant date");
        }

        protected void AssertOrderTaskSubjectComment(DossierFile dossierFile)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            var detail = order.DetailList.SingleOrDefault();
            Assert.NotNull(detail, "Order has single detail");
            Assert.AreEqual(detail.Comment, string.Format(detail.DetailComment, dossierFile.TaskId, dossierFile.TaskStamp.GetValueOrDefault().ToShortDateString()),
                "Detail comment constructed");

        }

        protected void AssertOrderViolationComment(DossierFile dossierFile)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            var detail = order.DetailList.SingleOrDefault();
            Assert.NotNull(detail, "Order has single detail");
            Assert.AreEqual(detail.Comment, string.Format(detail.DetailComment + dossierFile.GetViolationNotice().Violations),
                "Detail violation comment constructed");
        }

        protected void AssertOrderAgree(DossierFile dossierFile, Employee employee, int agreeCnt)
        {
            var order = dossierFile.CurrentFileStep.StepStandartOrder();
            Assert.NotNull(order.AgreeList, "Order has agrees");
            var ordeAgrees = order.AgreeList.Where(t => t.EmployeeName == employee.Name)
                                  .Where(t => t.EmployeePosition == employee.Position);
            Assert.AreEqual(ordeAgrees.Count(), agreeCnt, "Order has agrees from employee");
        }

        #endregion

        #endregion
    }
}
