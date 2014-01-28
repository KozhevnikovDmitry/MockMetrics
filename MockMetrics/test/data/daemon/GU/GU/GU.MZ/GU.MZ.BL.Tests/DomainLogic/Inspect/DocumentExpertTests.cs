using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Inspect;
using GU.MZ.BL.DomainLogic.Inspect.ExpertiseException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Inspect
{
    /// <summary>
    /// Тесты на методы класса DocumentExpert
    /// </summary>
    [TestFixture]
    public class DocumentExpertTests
    {
        [Test]
        public void PrepareDocumentExpertiseTest()
        {
            var scenarioStep = Mock.Of<ScenarioStep>();
            var fileStep = Mock.Of<DossierFileScenarioStep>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.GetStep(scenarioStep) == fileStep
                                              && t.StepDocumentExpertise(scenarioStep) == fileStep.DocumentExpertise);

            var documentExpert = new DocumentExpert();


            // Act
            var expertise = documentExpert.PrepareExpertise(file, scenarioStep);

            // Assert
            Assert.AreEqual(expertise.Id, 1);
            Assert.AreEqual(expertise.StartStamp.Date, DateTime.Today.AddDays(2));
            Assert.AreEqual(expertise.ActStamp.Date, DateTime.Today);
            Assert.AreEqual(expertise.EndStamp.Date, DateTime.Today.AddDays(1));
            Assert.IsNotNull(expertise.ExperiseResultList);
            Assert.AreEqual(fileStep.DocumentExpertise, expertise);
        }

        [Test]
        public void PrepareMoreThanOneExpertiseTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.StepDocumentExpertise(It.IsAny<ScenarioStep>()) == Mock.Of<DocumentExpertise>());
            var documentExpert = new DocumentExpert();

            // Assert
            Assert.Throws<PrepareMoreThanOneExpertiseException>(() => documentExpert.PrepareExpertise(file, Mock.Of<ScenarioStep>()));
        }

        [Test]
        public void AddExpretiseResultTest()
        {
            // Arrange
            var dictManager = Mock.Of<IDictionaryManager>();
            var service = Mock.Of<Service>();
            var resultList = new EditableList<DocumentExpertiseResult>();
            var expertise = Mock.Of<DocumentExpertise>(t => t.Id == 1 && t.ExperiseResultList == resultList);
            var scenarioStep = Mock.Of<ScenarioStep>();
            var file = Mock.Of<DossierFile>(t => t.StepDocumentExpertise(scenarioStep) == expertise && t.Service == service);
            var expertedDocument = Mock.Of<ExpertedDocument>(t => t.Id == 2);
            var documentExpert = new Mock<DocumentExpert> { CallBase = true };
            documentExpert.Setup(t => t.GetAvailableDocs(dictManager, service))
                .Returns(new List<ExpertedDocument> {expertedDocument});

            // Act
            var result = documentExpert.Object.AddExpertiseResult(file, scenarioStep, expertedDocument, dictManager);

            // Assert
            Assert.AreEqual(result.DocumentExpertiseId, 1);
            Assert.AreEqual(result.IsDocumentValid, true);
            Assert.AreEqual(result.ExpertedDocumentId, 2);
            Assert.AreEqual(expertise.ExperiseResultList.Single(), result);
        }

        [Test]
        public void UnavailableDocumentToAddResultTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.Service == Mock.Of<Service>());
            var scenarioStep = Mock.Of<ScenarioStep>();
            var expertedDocument = Mock.Of<ExpertedDocument>();
            var dictManager = Mock.Of<IDictionaryManager>();
            var documentExpert = new Mock<DocumentExpert> { CallBase = true };
            documentExpert.Setup(t => t.GetAvailableDocs(It.IsAny<IDictionaryManager>(), It.IsAny<Service>()))
                .Returns(new List<ExpertedDocument>());

            // Assert
            Assert.Throws<UnavailableDocumentToAddResultException>(() => documentExpert.Object.AddExpertiseResult(file, scenarioStep, expertedDocument, dictManager));
        }

        [Test]
        public void AddResultBeforePrepareExpertiseTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.Service == Mock.Of<Service>());
            var scenarioStep = Mock.Of<ScenarioStep>();
            var expertedDocument = Mock.Of<ExpertedDocument>();
            var dictManager = Mock.Of<IDictionaryManager>();
            var documentExpert = new Mock<DocumentExpert> { CallBase = true };
            documentExpert.Setup(t => t.GetAvailableDocs(It.IsAny<IDictionaryManager>(), It.IsAny<Service>()))
                .Returns(new List<ExpertedDocument> { expertedDocument });
            
            // Assert
            Assert.Throws<AddResultBeforePrepareExpertiseException>(() => documentExpert.Object.AddExpertiseResult(file, scenarioStep, expertedDocument, dictManager));
        }

        [Test]
        public void GetAvailableDocsTest()
        {
            // Arrange
            var dictManager = Mock.Of<IDictionaryManager>(t => t.GetDictionary<ExpertedDocument>() ==
                        new List<ExpertedDocument>
                        {
                            Mock.Of<ExpertedDocument>(
                                e => e.ServiceId == 1 && e.Id == 100500),
                            Mock.Of<ExpertedDocument>(e => e.ServiceId == 2)
                        });
            var service = Mock.Of<Service>(t => t.Id == 1);
            var documentExpert = new DocumentExpert();

            // Act
            var docs = documentExpert.GetAvailableDocs(dictManager, service);

            // Assert
            Assert.AreEqual(docs.Single().Id, 100500);
        }
    }
}
