using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GrantResult;
using GU.MZ.BL.DomainLogic.Inspect;
using GU.MZ.BL.DomainLogic.Licensing;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Notify;
using GU.MZ.BL.DomainLogic.Order;
using GU.MZ.BL.DomainLogic.Supervision.SupervisionException;
using GU.MZ.BL.DomainLogic.ViolationResolve;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.BL.DomainLogic.Supervision
{
    public class SupervisionFacade : IDisposable
    {
        public DossierFile DossierFile { get; protected set; }

        public bool IsSpecificMapping
        {
            get
            {
                return _specificMapping != null;
            }
        }
        
        private Action<SupervisionFacade> _specificMapping;

        public SupervisionFacade(InventoryReport inventory,
                                 ILinkager linakger,
                                 DossierFileNotifier notifier,
                                 OrderProvider orderProvider,
                                 DocumentExpert expert,
                                 HolderInspecter inspecter,
                                 ServiceResultGranter resultGranter,
                                 ViolationResolver violationResolver,
                                 DossierFileRepository fileRepository,
                                 ILicenseProvider licenseProvider,
                                 ILicenseRenewaller licenseRenewaller,
                                 ITaskPolicy taskPolicy,
                                 IDictionaryManager dictionaryManager,
                                 IDomainDataMapper<DossierFile> dossierFileMapper,
                                 DiActionContext diActionContext)
        {
            Inventory = inventory;
            Linakger = linakger;
            Notifier = notifier;
            OrderProvider = orderProvider;
            Expert = expert;
            Inspecter = inspecter;
            ResultGranter = resultGranter;
            ViolationResolver = violationResolver;
            FileRepository = fileRepository;
            LicenseProvider = licenseProvider;
            LicenseRenewaller = licenseRenewaller;
            TaskPolicy = taskPolicy;
            DictionaryManager = dictionaryManager;
            DossierFileMapper = dossierFileMapper;
            DiActionContext = diActionContext;
        }

        public void Initialize(DossierFile dossierFile)
        {
            DossierFile = dossierFile;
        }

        #region Logic Instances

        public InventoryReport Inventory { get; protected set; }

        public ILinkager Linakger { get; protected set; }

        public DossierFileNotifier Notifier { get; protected set; }

        public OrderProvider OrderProvider { get; protected set; }

        public DocumentExpert Expert { get; protected set; }

        public HolderInspecter Inspecter { get; protected set; }

        public ServiceResultGranter ResultGranter { get; protected set; }

        public ViolationResolver ViolationResolver { get; protected set; }

        public DossierFileRepository FileRepository { get; protected set; }

        public ILicenseProvider LicenseProvider { get; protected set; }

        public ILicenseRenewaller LicenseRenewaller { get; protected set; }

        public ITaskPolicy TaskPolicy { get; protected set; }

        public IDictionaryManager DictionaryManager { get; protected set; }

        public IDomainDataMapper<DossierFile> DossierFileMapper { get; protected set; }

        public DiActionContext DiActionContext { get; protected set; }

        #endregion

        #region SaveDossierFile

        public void SaveDossierFile()
        {
            _specificMapping(this);
            _specificMapping = null;
        }

        public void DefaultSave()
        {
            DossierFile = DossierFileMapper.Save(DossierFile);
        }
        
        #endregion

        #region Step Next

        public void StepNext(Employee responsibleEmployee)
        {
            if (!CanStepNext())
            {
                throw new NoNextScenarioStepException();
            }

            DossierFile.CurrentFileStep.EndDate = DateTime.Now;

            var nextStep = GetNextScenarioStep();

            AddNextStep(nextStep, responsibleEmployee);

        }

        public ScenarioStep GetNextScenarioStep()
        {
            var steps = DossierFile.Scenario
                                   .ScenarioStepList
                                   .OrderBy(t => t.SortOrder)
                                   .ToList();

            if (steps.Last().Id == DossierFile.CurrentScenarioStepId)
            {
                throw new NoNextScenarioStepException();
            }

            return steps[steps.IndexOf(steps.Single(t => t.Id == DossierFile.CurrentScenarioStepId)) + 1];
        }

        private void AddNextStep(ScenarioStep nextStep, Employee responsibleEmployee)
        {
            var newFileStep = DossierFileScenarioStep.CreateInstance();
            newFileStep.StartDate = DateTime.Now;
            newFileStep.DossierFileId = DossierFile.Id;
            newFileStep.ScenarioStepId = nextStep.Id;
            newFileStep.EmployeeId = responsibleEmployee.Id;

            DossierFile.DossierFileStepList.Add(newFileStep);
            DossierFile.CurrentScenarioStepId = nextStep.Id;
        }

        public bool CanStepNext()
        {
            var steps = DictionaryManager.GetDictionary<ScenarioStep>()
                                         .Where(t => t.ScenarioId == DossierFile.ScenarioId)
                                         .OrderBy(t => t.SortOrder)
                                         .ToList();

            return steps.Last().Id != DossierFile.CurrentScenarioStepId;
        }

        public bool CanStepNextFromStep(ScenarioStep sceanrioStep)
        {
            return DossierFile.CurrentScenarioStepId == sceanrioStep.Id && CanStepNext() && !IsSpecificMapping;
        }

        public void StepNextWithStatus(Employee responsibleEmployee, TaskStatusType taskStatusType)
        {
            StepNext(responsibleEmployee);

            TaskPolicy.SetStatus(taskStatusType, string.Empty, DossierFile.Task);

            DossierFile = DossierFileMapper.Save(DossierFile);
        }

        public bool IsRejectedOrderPrepared(ScenarioStep step)
        {
            var order = DossierFile.StepStandartOrder(step);
            return order != null && 
                   (order.OrderType == StandartOrderType.ReturnTask || 
                    order.OrderType == StandartOrderType.NotGrantLicense ||
                    order.OrderType == StandartOrderType.NotRenewalLicense);
        }

        #endregion

        #region Supervision Logic

        private IDossierFileLinkWrapper _dossierFileLinkWrapper;

        public IDossierFileLinkWrapper Linkage()
        {
            _dossierFileLinkWrapper = Linakger.Linkage(DossierFile);
            _specificMapping = s => { _dossierFileLinkWrapper.Linkage(); s.SaveLinkagedDossierFile(); };
            return _dossierFileLinkWrapper;
        }

        public Dictionary<RequisitesOrigin, HolderRequisites> AvailableRequisites
        {
            get
            {
                return _dossierFileLinkWrapper.AvailableRequisites;
            }
        }

        public bool GetIsHolderDataDoubtfull()
        {
            return _dossierFileLinkWrapper.IsHolderDataDoubtfull;
        }

        public List<ExpertedDocument> GetAvailableDocs()
        {
            return DiActionContext.Get<IDictionaryManager, List<ExpertedDocument>>(m => Expert.GetAvailableDocs(m, DossierFile.Service));            
        }

        public void AddExpertiseResult(ExpertedDocument expertedDocument, ScenarioStep step)
        {
            DiActionContext.Act<IDictionaryManager>(m => Expert.AddExpertiseResult(DossierFile, step, expertedDocument, m));
        }

        public DocumentExpertise PrepareExpertise(ScenarioStep step)
        {
            return Expert.PrepareExpertise(DossierFile, step);
        }

        public bool CanSetStatus(TaskStatusType taskStatusType)
        {
            return TaskPolicy.IsValidStatusTransition(DossierFile.TaskState, taskStatusType);
        }

        public void SetStatus(TaskStatusType taskStatusType)
        {
           TaskPolicy.SetStatus(taskStatusType, string.Empty, DossierFile.Task);
        }

        public Inspection PrepareHolderInspection(ScenarioStep scenarioStep)
        {
            return Inspecter.PrepareHolderInspection(DossierFile, scenarioStep);
        }

        public List<Employee> GetAvailableEmployees(ScenarioStep scenarioStep)
        {
            return DiActionContext.Get<IDictionaryManager, List<Employee>>(dm => Inspecter.GetAvailableEmployees(dm, DossierFile, scenarioStep));
        }

        public List<Expert> GetAvailableExperts(ScenarioStep scenarioStep)
        {
            return DiActionContext.Get<IDictionaryManager, List<Expert>>(dm => Inspecter.GetAvailableExperts(dm, DossierFile, scenarioStep));
        }

        public void AddInspectionEmployee(Employee employee, ScenarioStep scenarioStep)
        {
            Inspecter.AddInspectionEmployee(employee, DossierFile, scenarioStep);
        }

        public void AddInspectionExpert(Expert expert, ScenarioStep scenarioStep)
        {
            Inspecter.AddInspectionExpert(expert, DossierFile, scenarioStep);
        }

        public Notice AddNotice(NoticeType noticeType, ScenarioStep scenarioStep)
        {
            return Notifier.AddNotice(DossierFile, noticeType, scenarioStep);
        }

        public ViolationResolveInfo PrepareViolationResolveInfo(ScenarioStep scenarioStep)
        {
            return ViolationResolver.PrepareViolationResolveInfo(DossierFile, scenarioStep);
        }

        public ViolationNotice PrepareViolationNotice(ScenarioStep scenarioStep)
        {
            return ViolationResolver.PrepareViolationNotice(DossierFile, scenarioStep);
        }

        public DossierFileServiceResult GrantServiseResult()
        {
            _specificMapping = s => s.SaveServiceResult();
            return DiActionContext.Get<IDictionaryManager, DossierFileServiceResult>(d => ResultGranter.GrantServiseResult(DossierFile, d));
        }

        public void SaveServiceResult()
        {
            DossierFile = DiActionContext.Get<ITaskStatusPolicy, IDomainDataMapper<DossierFile>, DossierFile>((t, m) => ResultGranter.SaveServiceResult(DossierFile, t, m));
        }

        public void SaveRenewaledLicense()
        {
            DossierFile = LicenseRenewaller.SaveChanges(DossierFile);
        }

        public void SaveLinkagedDossierFile()
        {
            DossierFile = FileRepository.SaveLinkagedDossierFile(DossierFile);
        }

        public void AcceptDossierFile()
        {
            FileRepository.AcceptDossierFile(DossierFile);
        }

        public License GetNewLicense()
        {
            DossierFile.License = LicenseProvider.GetNewLicense(DossierFile);
            return DossierFile.License;
        }

        public void StopLicense()
        {
            LicenseProvider.GetStopLicense(DossierFile);
        }
        
        public void RenewalLicense()
        {
            _specificMapping = s => s.SaveRenewaledLicense();
            LicenseRenewaller.RenewalLicense(DossierFile);
        }

        public bool IsRenewalled()
        {
            return LicenseRenewaller.IsRenewalled(DossierFile);
        }

        #endregion

        #region Prepare Order

        public ExpertiseOrder PrepareExpertiseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareExpertiseOrder(DossierFile, step);
            OrderProvider.PrepareExpertiseOrderDates(order, step);
            return order;
        }

        public void AddExpertiseOrderAgree(ExpertiseOrder order, Employee employee)
        {
            OrderProvider.AddExpertiseOrderAgree(order, employee);
        }

        public InspectionOrder PrepareInspectionOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareInspectionOrder(DossierFile, step);
            OrderProvider.PrepareInspectionOrderDates(order, step);
            return order;
        }

        public void AddInspectionOrderAgree(InspectionOrder order, Employee employee)
        {
            OrderProvider.AddInspectionOrderAgree(order, employee);
        }

        public void AddInspectionOrderExpert(InspectionOrder order, Expert expert)
        {
            OrderProvider.AddInspectionOrderExpert(order, expert);
        }
        
        public StandartOrder PrepareReturnTaskOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.ReturnTask), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareTaskSubject(order.DetailList.Single(), DossierFile);
            OrderProvider.PrepareViolationComment(order.DetailList.Single(), DossierFile);
            return order;
        }

        public StandartOrder PrepareAcceptTaskOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.AcceptTask), step);
            order.FormatFields(DossierFile);
            return OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
        }
        
        public StandartOrder PrepareGrantLicenseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.GrantLicense), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareLicenseSubject(order.DetailList.Single(), DossierFile);
            OrderProvider.PrepareTaskSubjectComment(order.DetailList.Single(), DossierFile);
            return order;
        }

        public StandartOrder PrepareRenewalLicenseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.RenewalLicense), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareLicenseSubject(order.DetailList.Single(), DossierFile);
            OrderProvider.PrepareTaskSubjectComment(order.DetailList.Single(), DossierFile);
            return order;
        }

        public StandartOrder PrepareNotGrantLicenseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.NotGrantLicense), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareLicenseSubject(order.DetailList.Single(), DossierFile);
            return order;
        }

        public StandartOrder PrepareNotRenewalLicenseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.NotRenewalLicense), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareLicenseSubject(order.DetailList.Single(), DossierFile);
            return order;
        }

        public StandartOrder PrepareStopLicenseOrder(ScenarioStep step)
        {
            var order = OrderProvider.PrepareStandartOrder(DossierFile, GetOrderOption(StandartOrderType.StopLicense), step);
            order.FormatFields(DossierFile);
            order = OrderProvider.PrepareStandartLicensiarData(order, DictionaryManager);
            OrderProvider.PrepareDetails(order, DossierFile);
            OrderProvider.PrepareLicenseSubject(order.DetailList.Single(), DossierFile);
            OrderProvider.PrepareTaskSubjectComment(order.DetailList.Single(), DossierFile);
            return order;
        }

        private StandartOrderOption GetOrderOption(StandartOrderType standartOrderType)
        {
            return DictionaryManager.GetDictionary<StandartOrderOption>(t => t.OrderType == standartOrderType)
                                    .Single();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            DiActionContext.Dispose();
        }

        #endregion

    }
}
