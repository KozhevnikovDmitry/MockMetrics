using System;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Dossier;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Reporting.Mapping
{
    /// <summary>
    /// Тесты на класс InventoryReport
    /// </summary>
    [TestFixture]
    public class InventoryReportTests : BaseTestFixture
    {
        /// <summary>
        /// Тест получение корректного списка документов для описи
        /// </summary>
        [Test]
        public void CorrectDocListGenerateTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t => 
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument> { Mock.Of<ProvidedDocument>() }));

            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.InventDocumentList.Count, 1);
        }

        /// <summary>
        /// Тест на корректность заполнения регистрационного номера описи
        /// </summary>
        [Test]
        public void CorrectInventoryRegNumberTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t =>
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument>()
                                                    && d.RegNumber == 10));

            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.InventoryRegNumber, "000010");
        }

        /// <summary>
        /// Тест на корректность заполнения даты составления описи
        /// </summary>
        [Test]
        public void CorrectInventoryStampTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t =>
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument>()
                                                    && d.Stamp == DateTime.Today));

            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.InventoryStamp, DateTime.Today);
        }

        /// <summary>
        /// Тест на корректность заполнения лицензируемого вида деятельности
        /// </summary>
        [Test]
        public void CorrectInventoryLicensedActivityTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t =>
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument>()
                                                    && d.LicensedActivity == "LicensedActivity"));
            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.LicensedActivityName, "LicensedActivity");
        }

        /// <summary>
        /// Тест на корректность заполнения наименование заявителя
        /// </summary>
        [Test]
        public void CorrectInventoryHolderNameTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t =>
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument>()
                                                    && d.LicenseHolder == "Лицензиат"));

            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.HolderName, "Лицензиат");
        }

        /// <summary>
        /// Тест на корректность заполнения данных ответсвенного исполнителя
        /// </summary>
        [Test]
        public void CorrectInventoryAuthorDataTest()
        {
            // Arrange
            var mapper = Mock.Of<IDomainDataMapper<DocumentInventory>>(t =>
                t.Retrieve(1)
                    == Mock.Of<DocumentInventory>(d => d.ProvidedDocumentList == new EditableList<ProvidedDocument>()
                                                    && d.EmployeeName == "Name"
                                                    && d.EmployeePosition == "Position"));
            var report = new InventoryReport(mapper)
            {
                InventoryId = 1
            };

            // Act
            dynamic inventory = report.RetrieveData();

            // Assert
            Assert.AreEqual(inventory.AuthorName, "Name");
            Assert.AreEqual(inventory.AuthorPosition, "Position");
        }
    }
}
