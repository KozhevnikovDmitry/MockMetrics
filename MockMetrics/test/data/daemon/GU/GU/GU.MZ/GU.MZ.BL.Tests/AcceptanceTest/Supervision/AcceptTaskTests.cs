using System.Linq;
using Common.DA;
using GU.DataModel;
using GU.MZ.DataModel.Person;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на логику приёма заявки к рассмотрению
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class AcceptTaskTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на принятие к рассмотрению новой заявки с полным сценарием ведения
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(11)]
        [TestCase(12)]
        public void AcceptFullScenariedTaskTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            var dossierFile = ActAccepting(task);

            // Assert
            Assert.AreEqual(dossierFile.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.Employee.Id, DictionaryManager.GetDictionary<Employee>().First().Id,
                            "Resposible employee assert");

            Assert.AreEqual(dossierFile.Task.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.TaskState, TaskStatusType.Accepted,
                            "Task accepted status assert");

            Assert.NotNull(dossierFile.DocumentInventory,
                           "DocumentInventory saved");

            Assert.AreEqual(dossierFile.DocumentInventory.ProvidedDocumentList.Count, 3,
                           "DocumentInventory has contains 3 documents");

            Assert.Null(dossierFile.LicenseDossierId,
                        "LicenseDossier key null assert");
        }

        /// <summary>
        /// Тест на принятие к рассмотрению новой заявки с облегчённым сценарием ведения
        /// </summary>
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void AcceptLightScenariedTaskTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            var dossierFile = ActAccepting(task);

            // Assert
            Assert.AreEqual(dossierFile.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.Employee.Id, DictionaryManager.GetDictionary<Employee>().First().Id,
                            "Resposible employee assert");

            Assert.AreEqual(dossierFile.Task.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.TaskState, TaskStatusType.Accepted,
                            "Task accepted status assert");

            Assert.NotNull(dossierFile.DocumentInventory,
                           "DocumentInventory saved");

            Assert.AreEqual(dossierFile.DocumentInventory.ProvidedDocumentList.Count, 3,
                           "DocumentInventory has contains 3 documents");

            Assert.Null(dossierFile.LicenseDossierId,
                        "LicenseDossier key null assert");
        }

    }
}
