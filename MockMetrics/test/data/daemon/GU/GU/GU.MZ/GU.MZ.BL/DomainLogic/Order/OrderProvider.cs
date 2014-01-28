using System;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.Types;
using GU.BL.Extensions;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DomainLogic.Order
{
    /// <summary>
    /// Класс, ответственный за подготовку приказов
    /// </summary>
    public class OrderProvider
    {
        #region ExpertiseOrder

        public ExpertiseOrder PrepareExpertiseOrder(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            var order = ExpertiseOrder.CreateInstance();
            var step = dossierFile.GetStep(scenarioStep);

            order.Id = step.Id;
            order.Stamp = DateTime.Now;
            order.RegNumber = string.Empty;
            order.EmployeeName = dossierFile.EmployeeName;
            order.EmployeePosition = dossierFile.EmployeePosition;
            order.EmployeeContacts = dossierFile.EmployeeContacts;
            order.TaskId = dossierFile.TaskId;
            order.TaskStamp = dossierFile.TaskStamp ?? DateTime.Today;
            order.FullName = dossierFile.HolderFullName;
            order.Inn = dossierFile.HolderInn;
            order.Address = dossierFile.HolderAddress;
            order.LicensedActivity = dossierFile.LicensedActivity.BlankName;
            order.ActivityAdditionalInfo = dossierFile.LicensedActivity.AdditionalInfo;
            step.ExpertiseOrder = order;

            return order;
        }

        public void PrepareExpertiseOrderDates(ExpertiseOrder order, ScenarioStep scenarioStep)
        {
            order.DueDays = scenarioStep.Scenario.ScenarioStepList.Next(scenarioStep).DueDays;
            order.StartDate = order.Stamp.AddDays(1);
            order.DueDate = order.StartDate.AddWorkingDays(order.DueDays);
        }

        public void AddExpertiseOrderAgree(ExpertiseOrder order, Employee employee)
        {
            var agree = ExpertiseOrderAgree.CreateInstance();
            agree.EmployeeName = employee.Name;
            agree.EmployeePosition = employee.Position;
            order.ExpertiseOrderAgreeList.Add(agree);
        }

        #endregion


        #region InspectionOrder

        public InspectionOrder PrepareInspectionOrder(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            var order = InspectionOrder.CreateInstance();
            var step = dossierFile.GetStep(scenarioStep);

            order.Id = step.Id;
            order.Stamp = DateTime.Now;
            order.RegNumber = string.Empty;
            order.EmployeeName = dossierFile.EmployeeName;
            order.EmployeePosition = dossierFile.EmployeePosition;
            order.EmployeeContacts = dossierFile.EmployeeContacts;
            order.TaskId = dossierFile.TaskId;
            order.TaskStamp = dossierFile.TaskStamp ?? DateTime.Today;
            order.FullName = dossierFile.HolderFullName;
            order.Inn = dossierFile.HolderInn;
            order.Address = dossierFile.HolderAddress;
            order.LicensedActivity = dossierFile.LicensedActivity.BlankName;
            order.ActivityAdditionalInfo = dossierFile.LicensedActivity.AdditionalInfo;
            step.InspectionOrder = order;

            return order;
        }

        public void PrepareInspectionOrderDates(InspectionOrder order, ScenarioStep scenarioStep)
        {
            order.DueDays = scenarioStep.Scenario.ScenarioStepList.Next(scenarioStep).DueDays;
            order.StartDate = order.Stamp.AddDays(1);
            order.DueDate = order.StartDate.AddWorkingDays(order.DueDays);
        }

        public void AddInspectionOrderAgree(InspectionOrder order, Employee employee)
        {
            var agree = InspectionOrderAgree.CreateInstance();
            agree.EmployeeName = employee.Name;
            agree.EmployeePosition = employee.Position;
            order.InspectionOrderAgreeList.Add(agree);
        }

        public void AddInspectionOrderExpert(InspectionOrder order, Expert expert)
        {
            var orderExpert = InspectionOrderExpert.CreateInstance();
            orderExpert.ExpertName = expert.ExpertState.GetName();
            orderExpert.ExpertPosition = expert.ExpertState.GetWorkdata();
            order.InspectionOrderExpertList.Add(orderExpert);
        }

        #endregion


        #region StandartOrder

        public StandartOrder PrepareStandartOrder(DossierFile dossierFile, StandartOrderOption orderOption, ScenarioStep step)
        {
            var fileStep = dossierFile.GetStep(step);
            var order = StandartOrder.CreateInstance();
            order.Stamp = DateTime.Now;
            order.RegNumber = string.Empty;
            order.EmployeeName = dossierFile.EmployeeName;
            order.EmployeePosition = dossierFile.EmployeePosition;
            order.EmployeeContacts = dossierFile.EmployeeContacts;
            order.OrderOption = orderOption;
            order.OrderOptionId = orderOption.Id;
            order.ActivityInfo =
                string.Format("{0} {1}", dossierFile.LicensedActivity.BlankName,
                    dossierFile.LicensedActivity.AdditionalInfo).Trim();
            order.FileScenarioStepId = fileStep.Id;
            order.FileScenarioStep = fileStep;
            fileStep.StandartOrderList.Add(order);
            return order;
        }

        public StandartOrder PrepareStandartLicensiarData(StandartOrder order, IDictionaryManager dictionaryManager)
        {
            var chief = dictionaryManager.GetDynamicDictionary<Employee>().FirstOrDefault(t => !t.ChiefId.HasValue);

            if (chief == null)
            {
                order.LicensiarHeadName = string.Empty;
                order.LicensiarHeadPosition = string.Empty;
            }
            else
            {
                order.LicensiarHeadName = chief.Name;
                order.LicensiarHeadPosition = chief.Position;
            }

            return order;
        }

        public StandartOrder PrepareDetails(StandartOrder order, DossierFile dossierFile)
        {
            var detail = StandartOrderDetail.CreateInstance();
            detail.Address = dossierFile.HolderAddress;
            detail.FullName = dossierFile.HolderFullName;
            detail.ShortName = dossierFile.HolderShortName;
            detail.FirmName = dossierFile.HolderFirmName;
            detail.Inn = dossierFile.HolderInn;
            detail.Ogrn = dossierFile.HolderOgrn;
            order.DetailList.Add(detail);
            detail.StandartOrder = order;
            return order;
        }

        public StandartOrderDetail PrepareTaskSubject(StandartOrderDetail detail, DossierFile dossierFile)
        {
            detail.SubjectId = dossierFile.TaskId.ToString();
            detail.SubjectStamp = dossierFile.TaskStamp;
            return detail;
        }

        public StandartOrderDetail PrepareLicenseSubject(StandartOrderDetail detail, DossierFile dossierFile)
        {
            detail.SubjectId = dossierFile.LicenseRegNumber;
            detail.SubjectStamp = dossierFile.LicenseGrantDate;
            return detail;
        }

        public StandartOrderDetail PrepareTaskSubjectComment(StandartOrderDetail detail, DossierFile dossierFile)
        {
            detail.Comment = string.Format(detail.DetailComment, dossierFile.TaskId,
                dossierFile.TaskStamp.GetValueOrDefault().ToShortDateString());
            return detail;
        }

        public StandartOrderDetail PrepareViolationComment(StandartOrderDetail detail, DossierFile dossierFile)
        {
            ViolationNotice notice = dossierFile.GetViolationNotice();

            if (notice == null)
                return detail;

            detail.Comment = detail.DetailComment + notice.Violations;
            return detail;
        }
        
        public void AddStandartOrderAgree(StandartOrder order, Employee employee)
        {
            var agree = StandartOrderAgree.CreateInstance();
            agree.EmployeeName = employee.Name;
            agree.EmployeePosition = employee.Position;
            order.AgreeList.Add(agree);
        }

        #endregion
    }
}
