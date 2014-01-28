using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.BL.Extensions;
using GU.MZ.BL.DomainLogic.Order;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Order
{
    /// <summary>
    /// Тесты на методы класса OrderProvider
    /// </summary>
    [TestFixture]
    public class OrderProviderTests
    {
        #region Expertise

        [Test]
        public void PrepareExpertiseOrderTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepExpertiseOrder(scenarioStep) == fileStep.ExpertiseOrder
                                              && t.EmployeeName == "EmployeeName"
                                              && t.EmployeePosition == "EmployeePosition"
                                              && t.EmployeeContacts == "EmployeeContacts"
                                              && t.TaskId == 100500
                                              && t.TaskStamp == DateTime.Today
                                              && t.HolderFullName == "HolderFullName"
                                              && t.HolderAddress == "HolderAddress"
                                              && t.LicensedActivity == Mock.Of<LicensedActivity>(l => l.BlankName == "BlankName" && l.AdditionalInfo == "AdditionalInfo")
                                              && t.HolderInn == "HolderInn");

            var provider = new OrderProvider();

            // Act
            var order = provider.PrepareExpertiseOrder(file, scenarioStep);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(order.Id, 1);
            Assert.AreEqual(order.RegNumber, string.Empty);
            Assert.AreEqual(order.Stamp.Date, DateTime.Today);
            Assert.AreEqual(fileStep.ExpertiseOrder, order);
            Assert.AreEqual(order.EmployeeName, "EmployeeName");
            Assert.AreEqual(order.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(order.EmployeeContacts, "EmployeeContacts");
            Assert.AreEqual(order.TaskId, 100500);
            Assert.AreEqual(order.TaskStamp, DateTime.Today);
            Assert.AreEqual(order.FullName, "HolderFullName");
            Assert.AreEqual(order.Inn, "HolderInn");
            Assert.AreEqual(order.Address, "HolderAddress");
            Assert.AreEqual(order.LicensedActivity, "BlankName");
            Assert.AreEqual(order.ActivityAdditionalInfo, "AdditionalInfo");
        }

        [Test]
        public void PrepareExpertiseOrderDatesTest()
        {
            // Arrange
            var steps = new List<ScenarioStep>();
            var scenarioStep = Mock.Of<ScenarioStep>(t => t.Scenario == Mock.Of<Scenario>(s => s.ScenarioStepList == steps));
            steps.Add(scenarioStep); 
            steps.Add(Mock.Of<ScenarioStep>(t => t.DueDays == 10));
            var order = Mock.Of<ExpertiseOrder>(t => t.Stamp == DateTime.Today);
            var provider = new OrderProvider();

            // Act
            provider.PrepareExpertiseOrderDates(order, scenarioStep);

            // Assert
            Assert.AreEqual(order.DueDays, 10);
            Assert.AreEqual(order.StartDate, DateTime.Today.AddDays(1));
            Assert.AreEqual(order.DueDate, DateTime.Today.AddDays(1).AddWorkingDays(10));
        }

        [Test]
        public void AddExpertiseOrderAgreeTest()
        {
            // Arrange
            var list = new EditableList<ExpertiseOrderAgree>();
            var employee = Mock.Of<Employee>(t => t.Name == "Name" && t.Position == "Position");
            var order = Mock.Of<ExpertiseOrder>(t => t.ExpertiseOrderAgreeList == list);
            var provider = new OrderProvider();

            // Act
            provider.AddExpertiseOrderAgree(order, employee);

            // Assert
            Assert.AreEqual(order.ExpertiseOrderAgreeList.Single().EmployeeName, "Name");
            Assert.AreEqual(order.ExpertiseOrderAgreeList.Single().EmployeePosition, "Position");
        }

        #endregion

        #region Inspection

        [Test]
        public void PrepareInspectionOrderTest()
        {
            // Arrange
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepInspectionOrder(scenarioStep) == fileStep.InspectionOrder
                                              && t.EmployeeName == "EmployeeName"
                                              && t.EmployeePosition == "EmployeePosition"
                                              && t.EmployeeContacts == "EmployeeContacts"
                                              && t.TaskId == 100500
                                              && t.TaskStamp == DateTime.Today
                                              && t.HolderFullName == "HolderFullName"
                                              && t.HolderAddress == "HolderAddress"
                                              && t.LicensedActivity == Mock.Of<LicensedActivity>(l => l.BlankName == "BlankName" && l.AdditionalInfo == "AdditionalInfo")
                                              && t.HolderInn == "HolderInn");

            var provider = new OrderProvider();

            // Act
            var order = provider.PrepareInspectionOrder(file, scenarioStep);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(order.Id, 1);
            Assert.AreEqual(order.RegNumber, string.Empty);
            Assert.AreEqual(order.Stamp.Date, DateTime.Today);
            Assert.AreEqual(fileStep.InspectionOrder, order);
            Assert.AreEqual(order.EmployeeName, "EmployeeName");
            Assert.AreEqual(order.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(order.EmployeeContacts, "EmployeeContacts");
            Assert.AreEqual(order.TaskId, 100500);
            Assert.AreEqual(order.TaskStamp, DateTime.Today);
            Assert.AreEqual(order.FullName, "HolderFullName");
            Assert.AreEqual(order.Inn, "HolderInn");
            Assert.AreEqual(order.Address, "HolderAddress");
            Assert.AreEqual(order.LicensedActivity, "BlankName");
            Assert.AreEqual(order.ActivityAdditionalInfo, "AdditionalInfo");
        }

        [Test]
        public void PrepareInspectionOrderDatesTest()
        {
            // Arrange
            var steps = new List<ScenarioStep>();
            var scenarioStep = Mock.Of<ScenarioStep>(t => t.Scenario == Mock.Of<Scenario>(s => s.ScenarioStepList == steps));
            steps.Add(scenarioStep);
            steps.Add(Mock.Of<ScenarioStep>(t => t.DueDays == 10));
            var order = Mock.Of<InspectionOrder>(t => t.Stamp == DateTime.Today);
            var provider = new OrderProvider();

            // Act
            provider.PrepareInspectionOrderDates(order, scenarioStep);

            // Assert
            Assert.AreEqual(order.DueDays, 10);
            Assert.AreEqual(order.StartDate, DateTime.Today.AddDays(1));
            Assert.AreEqual(order.DueDate, DateTime.Today.AddDays(1).AddWorkingDays(10));
        }

        [Test]
        public void AddInspectionOrderAgreeTest()
        {
            // Arrange
            var list = new EditableList<InspectionOrderAgree>();
            var employee = Mock.Of<Employee>(t => t.Name == "Name" && t.Position == "Position");
            var order = Mock.Of<InspectionOrder>(t => t.InspectionOrderAgreeList == list);
            var provider = new OrderProvider();

            // Act
            provider.AddInspectionOrderAgree(order, employee);

            // Assert
            Assert.AreEqual(order.InspectionOrderAgreeList.Single().EmployeeName, "Name");
            Assert.AreEqual(order.InspectionOrderAgreeList.Single().EmployeePosition, "Position");
        }

        [Test]
        public void AddInspectionOrderExpertTest()
        {
            // Arrange
            var list = new EditableList<InspectionOrderExpert>();
            var expert = Mock.Of<Expert>(t => t.ExpertState.GetName() == "Name" && t.ExpertState.GetWorkdata() == "Position");
            var order = Mock.Of<InspectionOrder>(t => t.InspectionOrderExpertList == list);
            var provider = new OrderProvider();

            // Act
            provider.AddInspectionOrderExpert(order, expert);

            // Assert
            Assert.AreEqual(order.InspectionOrderExpertList.Single().ExpertName, "Name");
            Assert.AreEqual(order.InspectionOrderExpertList.Single().ExpertPosition, "Position");
        }

        #endregion
        
        #region Standart

        [Test]
        public void PrepareStandartOrderTest()
        {
            // Arrange
            var licActivity =
                Mock.Of<LicensedActivity>(t => t.AdditionalInfo == "AdditionalInfo  " && t.BlankName == "BlankName");
            var option = Mock.Of<StandartOrderOption>(t => t.Id == 2);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.LicensedActivity == licActivity
                                              && t.EmployeeName == "EmployeeName"
                                              && t.EmployeePosition == "EmployeePosition"
                                              && t.EmployeeContacts == "EmployeeContacts");

            var provider = new OrderProvider();

            // Act
            var order = provider.PrepareStandartOrder(file, option, scenarioStep);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(order.RegNumber, string.Empty);
            Assert.AreEqual(order.Stamp.Date, DateTime.Today);
            Assert.AreEqual(order.EmployeeName, "EmployeeName");
            Assert.AreEqual(order.EmployeePosition, "EmployeePosition");
            Assert.AreEqual(order.EmployeeContacts, "EmployeeContacts");
            Assert.AreEqual(order.OrderOption, option);
            Assert.AreEqual(order.OrderOptionId, 2);
            Assert.AreEqual(order.FileScenarioStep, fileStep);
            Assert.AreEqual(order.FileScenarioStepId, 1);
            Assert.AreEqual(fileStep.StandartOrderList.Single(), order);
            Assert.AreEqual(order.ActivityInfo, "BlankName AdditionalInfo");
        }

        [Test]
        public void PrepareStandartLicensiarDataTest()
        {
            // Arrange
            var order = Mock.Of<StandartOrder>();
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Employee>() == new List<Employee>
            {
                Mock.Of<Employee>(e => e.ChiefId == 1),
                Mock.Of<Employee>(e => e.ChiefId == 2),
                Mock.Of<Employee>(e => e.Name == "Name" && e.Position == "Position"),
            });
            var provider = new OrderProvider();

            // Act
            order = provider.PrepareStandartLicensiarData(order, dictMan);

            // Assert
            Assert.AreEqual(order.LicensiarHeadName, "Name");
            Assert.AreEqual(order.LicensiarHeadPosition, "Position");
        }

        [Test]
        public void PrepareStandartLicensiarDataForNotFoundedChiefTest()
        {
            // Arrange
            var order = Mock.Of<StandartOrder>();
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Employee>() == new List<Employee>
            {
                Mock.Of<Employee>(e => e.ChiefId == 1)
            });
            var provider = new OrderProvider();

            // Act
            order = provider.PrepareStandartLicensiarData(order, dictMan);

            // Assert
            Assert.AreEqual(order.LicensiarHeadName, string.Empty);
            Assert.AreEqual(order.LicensiarHeadPosition, string.Empty);
        }

        [Test]
        public void PrepareStandartDetailsTest()
        {
            // Arrange
            var order = Mock.Of<StandartOrder>(t => t.Id == 1);
            var dossierFile = Mock.Of<DossierFile>(t => t.HolderAddress == "HolderAddress"
                                                     && t.HolderFullName == "HolderFullName"
                                                     && t.HolderFirmName == "HolderFirmName"
                                                     && t.HolderShortName == "HolderShortName"
                                                     && t.HolderInn == "Inn"
                                                     && t.HolderOgrn == "Ogrn");
            var provider = new OrderProvider();

            // Act
            order = provider.PrepareDetails(order, dossierFile);

            // Assert
            Assert.IsNotNull(order.DetailList.Single());
            var detail = order.DetailList.Single();

            Assert.AreEqual(detail.Address, "HolderAddress");
            Assert.AreEqual(detail.FullName, "HolderFullName");
            Assert.AreEqual(detail.ShortName, "HolderShortName");
            Assert.AreEqual(detail.FirmName, "HolderFirmName");
            Assert.AreEqual(detail.Inn, "Inn");
            Assert.AreEqual(detail.Ogrn, "Ogrn");
            Assert.AreEqual(detail.StandartOrder, order);
        }

        [Test]
        public void AddStandartOrderAgreeTest()
        {
            // Arrange
            var list = new EditableList<StandartOrderAgree>();
            var employee = Mock.Of<Employee>(t => t.Name == "Name" && t.Position == "Position");
            var order = Mock.Of<StandartOrder>(t => t.AgreeList == list);
            var provider = new OrderProvider();

            // Act
            provider.AddStandartOrderAgree(order, employee);

            // Assert
            Assert.AreEqual(order.AgreeList.Single().EmployeeName, "Name");
            Assert.AreEqual(order.AgreeList.Single().EmployeePosition, "Position");
        }

        [Test]
        public void PrepareTaskSubjectTest()
        {
            // Arrange
            var orderDetail = Mock.Of<StandartOrderDetail>();
            var file = Mock.Of<DossierFile>(t => t.TaskId == 1 && t.TaskStamp == DateTime.Today);
            var provider = new OrderProvider();

            // Act
            provider.PrepareTaskSubject(orderDetail, file);

            // Assert
            Assert.AreEqual(orderDetail.SubjectId, "1");
            Assert.AreEqual(orderDetail.SubjectStamp, DateTime.Today);
        }

        [Test]
        public void PrepareTaskLicenseTest()
        {
            // Arrange
            var orderDetail = Mock.Of<StandartOrderDetail>();
            var file = Mock.Of<DossierFile>(t => t.LicenseRegNumber == "1" && t.LicenseGrantDate == DateTime.Today);
            var provider = new OrderProvider();

            // Act
            provider.PrepareLicenseSubject(orderDetail, file);

            // Assert
            Assert.AreEqual(orderDetail.SubjectId, "1");
            Assert.AreEqual(orderDetail.SubjectStamp, DateTime.Today);
        }

        [Test]
        public void PrepareTaskSubjectCommentTest()
        {
            // Arrange
            var orderDetail = Mock.Of<StandartOrderDetail>(t => t.DetailComment == "DetailCommentPattern {0} {1}");
            var file = Mock.Of<DossierFile>(t => t.TaskId == 1 && t.TaskStamp == DateTime.Today);
            var provider = new OrderProvider();

            // Act
            provider.PrepareTaskSubjectComment(orderDetail, file);

            // Assert
            Assert.AreEqual(orderDetail.Comment, string.Format("DetailCommentPattern {0} {1}", 1, DateTime.Today.ToShortDateString()));
        }

        [Test]
        public void PrepareViolationCommentTest()
        {
            // Arrange
            var violationNotice = Mock.Of<ViolationNotice>(t => t.Violations == "Violations");
            var orderDetail = Mock.Of<StandartOrderDetail>(t => t.DetailComment == "DetailCommentPattern: ");
            var file = Mock.Of<DossierFile>(t => t.GetViolationNotice() == violationNotice);
            var provider = new OrderProvider();

            // Act
            provider.PrepareViolationComment(orderDetail, file);

            // Assert
            Assert.AreEqual(orderDetail.Comment, "DetailCommentPattern: Violations");
        }

        #endregion
    }
}
