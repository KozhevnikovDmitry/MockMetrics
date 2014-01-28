using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.MZ.BL.DomainLogic.Inspect;
using GU.MZ.BL.DomainLogic.Inspect.ExpertiseException;
using GU.MZ.BL.DomainLogic.Inspect.InspectException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Inspect
{
    /// <summary>
    /// Тесты на методы класса HolderInspecter
    /// </summary>
    [TestFixture]
    public class HolderInspecterTests : BaseTestFixture
    {
        [Test]
        public void PrepareHolderInspectionTest()
        {
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepInspection(scenarioStep) == fileStep.Inspection);

            var inspecter = new HolderInspecter();


            // Act
            var inspection = inspecter.PrepareHolderInspection(file, scenarioStep);

            // Assert
            Assert.AreEqual(inspection.Id, 1);
            Assert.AreEqual(inspection.StartStamp.Date, DateTime.Today.AddDays(2));
            Assert.AreEqual(inspection.ActStamp.Date, DateTime.Today);
            Assert.AreEqual(inspection.EndStamp.Date, DateTime.Today.AddDays(1));
            Assert.AreEqual(fileStep.Inspection, inspection);
            Assert.False(inspection.IsPassed);
            Assert.IsNotNull(inspection.InspectionEmployeeList);
            Assert.IsNotNull(inspection.InspectionExpertList);
        }

        [Test]
        public void PrepareMoreThanOneInspectionTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.StepInspection(It.IsAny<ScenarioStep>()) == Mock.Of<Inspection>());
            var inspecter = new HolderInspecter();

            // Assert
            Assert.Throws<PrepareMoreThanOneInspectionException>(() => inspecter.PrepareHolderInspection(file, Mock.Of<ScenarioStep>()));
        }

        [Test]
        public void GetAvailableEmployeesTest()
        {
            // Arrange
            var employee = Mock.Of<Employee>();
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Employee>() == new List<Employee> { employee });
            var inspection = Mock.Of<Inspection>(t => t.InspectionEmployeeList == new EditableList<InspectionEmployee>());
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();
            
            // Act
            var employees = inspecter.GetAvailableEmployees(dictMan, file, scenarioStep);

            // Assert
            Assert.AreEqual(employees.Single(), employee);
        }
        
        [Test]
        public void NoInspectionFoundPerGetAvailableEmployeesTest()
        {
            // Arrange
            var dictMan = Mock.Of<IDictionaryManager>();
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>();
            var inspecter = new HolderInspecter();

            // Assert
            Assert.Throws<NoInspectionFoundException>(() => inspecter.GetAvailableEmployees(dictMan, file, scenarioStep));
        }

        [Test]
        public void GetAvailableEmployeesExceptAlreadyAddedToInspectionTest()
        {
            // Arrange
            var employee = Mock.Of<Employee>(t => t.Id == 1);
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Employee>() == new List<Employee> { employee });
            var inspection = Mock.Of<Inspection>(t => t.InspectionEmployeeList == new EditableList<InspectionEmployee> { Mock.Of<InspectionEmployee>(i => i.EmployeeId == 1) });
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();

            // Act
            var employees = inspecter.GetAvailableEmployees(dictMan, file, scenarioStep);

            // Assert
            Assert.IsEmpty(employees);
        }

        [Test]
        public void GetAvailableExpertsTest()
        {
            // Arrange
            var expert = Mock.Of<Expert>();
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Expert>() == new List<Expert> { expert });
            var inspection = Mock.Of<Inspection>(t => t.InspectionExpertList == new EditableList<InspectionExpert>());
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();

            // Act
            var experts = inspecter.GetAvailableExperts(dictMan, file, scenarioStep);

            // Assert
            Assert.AreEqual(experts.Single(), expert);
        }

        [Test]
        public void NoInspectionFoundPerGetAvailableExpertsTest()
        {
            // Arrange
            var dictMan = Mock.Of<IDictionaryManager>();
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>();
            var inspecter = new HolderInspecter();

            // Assert
            Assert.Throws<NoInspectionFoundException>(() => inspecter.GetAvailableExperts(dictMan, file, scenarioStep));
        }

        [Test]
        public void GetAvailableExpertsExceptAlreadyAddedToInspectionTest()
        {
            // Arrange
            var expert = Mock.Of<Expert>(t => t.Id == 1);
            var dictMan = Mock.Of<IDictionaryManager>(t => t.GetDynamicDictionary<Expert>() == new List<Expert> { expert });
            var inspection = Mock.Of<Inspection>(t => t.InspectionExpertList == new EditableList<InspectionExpert> { Mock.Of<InspectionExpert>(i => i.ExpertId == 1) });
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();

            // Act
            var experts = inspecter.GetAvailableExperts(dictMan, file, scenarioStep);

            // Assert
            Assert.IsEmpty(experts);
        }

        [Test]
        public void AddInspectionEmployeeTest()
        {
            // Arrange
            var employeeList = new EditableList<InspectionEmployee>();
            var inspection = Mock.Of<Inspection>(t => t.Id == 1 && t.InspectionEmployeeList == employeeList);
            var employee = Mock.Of<Employee>(t => t.Id == 2);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();
           
            
            // Act
            var inspectionEmployee = inspecter.AddInspectionEmployee(employee, file, scenarioStep);

            // Assert
            Assert.AreEqual(inspectionEmployee.Employee, employee);
            Assert.AreEqual(inspectionEmployee.EmployeeId, 2);
            Assert.AreEqual(inspectionEmployee.InspectionId, 1);
            Assert.AreEqual(inspection.InspectionEmployeeList.Single(), inspectionEmployee);
        }
        
        [Test]
        public void AddInspectionExpertTest()
        {
            // Arrange
            var expertList = new EditableList<InspectionExpert>();
            var inspection = Mock.Of<Inspection>(t => t.Id == 1 && t.InspectionExpertList == expertList);
            var expert = Mock.Of<Expert>(t => t.Id == 2);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepInspection(scenarioStep) == inspection);
            var inspecter = new HolderInspecter();


            // Act
            var inspectionExpert = inspecter.AddInspectionExpert(expert, file, scenarioStep);

            // Assert
            Assert.AreEqual(inspectionExpert.Expert, expert);
            Assert.AreEqual(inspectionExpert.ExpertId, 2);
            Assert.AreEqual(inspectionExpert.InspectionId, 1);
            Assert.AreEqual(inspection.InspectionExpertList.Single(), inspectionExpert);
        }
        
        [Test]
        public void AddEmployeeBeforePrepareInspectionTest()
        {
            // Arrange
            var employee = Mock.Of<Employee>(t => t.Id == 2);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>();
            var inspecter = new HolderInspecter();


            // Act
            Assert.Throws<AddEmployeeBeforePrepareInspectionException>(
                () => inspecter.AddInspectionEmployee(employee, file, scenarioStep));
        }

        [Test]
        public void AddExpertBeforePrepareInspectionTest()
        {
            // Arrange
            var expert = Mock.Of<Expert>(t => t.Id == 2);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>();
            var inspecter = new HolderInspecter();


            // Assert
            Assert.Throws<AddExpertBeforePrepareInspectionException>(
                () => inspecter.AddInspectionExpert(expert, file, scenarioStep));
        }
    }
}