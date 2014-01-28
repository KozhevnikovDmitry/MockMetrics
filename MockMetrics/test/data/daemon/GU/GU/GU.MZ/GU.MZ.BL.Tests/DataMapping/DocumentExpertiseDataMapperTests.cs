using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Inspect;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класс DocumentExpertiseDataMapper
    /// </summary>
    [TestFixture]
    public class DocumentExpertiseDataMapperTests : BaseTestFixture
    {
        private DocumentExpertiseDataMapper _expertiseDataMapper;

        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private Mock<IDomainDbManager> _mockDb;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // мок менеджера базы
            _mockDb = new Mock<IDomainDbManager>();
            _mockDb.Setup(db => db.RetrieveDomainObject<DocumentExpertise>(It.IsAny<object>())).Returns(DocumentExpertise.CreateInstance());

            _expertiseDataMapper = new DocumentExpertiseDataMapper(TestObjectMother.GetTestDomainContext(_mockDb.Object));
            TestObjectMother.SetDomainKey(_expertiseDataMapper);
        }

        /// <summary>
        /// Тест на получение ассоциоации список результатов проверки
        /// </summary>
        [Test]
        public void RetrieveExpertiseResultListTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DocumentExpertise>(It.IsAny<object>()))
                                       .Returns(Mock.Of<DocumentExpertise>(d => d.Id == 1));

            _mockDb.Setup(db => db.RetrieveDomainObject<DocumentExpertiseResult>(It.IsAny<object>()))
                                      .Returns(Mock.Of<DocumentExpertiseResult>(d => d.Id == 1));

            _mockDb.Setup(db => db.GetDomainTable<DocumentExpertiseResult>())
                                       .Returns(new EnumerableQuery<DocumentExpertiseResult>(new List<DocumentExpertiseResult>
                                                                                                 {
                                                                                                     Mock.Of<DocumentExpertiseResult>(t => t.DocumentExpertiseId == 1),
                                                                                                     Mock.Of<DocumentExpertiseResult>(t => t.DocumentExpertiseId == 2)
                                                                                                 }));

            // Act
            var obj = _expertiseDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(obj.ExperiseResultList);
            Assert.AreEqual(obj.ExperiseResultList.Count, 1);
            _mockDb.Verify(t => t.RetrieveDomainObject<DocumentExpertiseResult>(It.IsAny<object>()), Times.Once());
        }

        /// <summary>
        /// Тест на сохранение ассоциации список результатов проверки
        /// </summary>
        [Test]
        public void SaveExpertiseResultListTest()
        {
            // Arrange
            var obj = DocumentExpertise.CreateInstance();
            obj.ExperiseResultList = new EditableList<DocumentExpertiseResult> { DocumentExpertiseResult.CreateInstance() };

            // Act
            var savedDocumentExpertise = _expertiseDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.False(savedDocumentExpertise.ExperiseResultList.IsDirty);
            Assert.AreEqual(savedDocumentExpertise.ExperiseResultList.Count, 1);
            Assert.AreEqual(savedDocumentExpertise.ExperiseResultList.Single().DocumentExpertiseId, savedDocumentExpertise.Id);
            _mockDb.Verify(t => t.SaveDomainObject(It.IsAny<DocumentExpertiseResult>()), Times.Once());
        }

        /// <summary>
        /// Тест на удаление помеченных на удаление результатов документарной проверки
        /// </summary>
        [Test]
        public void DeleteMarkDeletedExpertiseResultsTest()
        {
            // Arrange
            var obj = DocumentExpertise.CreateInstance();
            var result = DocumentExpertiseResult.CreateInstance();

            obj.ExperiseResultList = new EditableList<DocumentExpertiseResult> { DocumentExpertiseResult.CreateInstance() };
            obj.ExperiseResultList.Add(result);
            obj.ExperiseResultList.AcceptChanges();
            obj.ExperiseResultList.Remove(result);

            // Act
            var savedDocumentExpertise = _expertiseDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.AreEqual(result.PersistentState, PersistentState.NewDeleted);
            _mockDb.Verify(t => t.SaveDomainObject(result), Times.Once());
        }
    }
}
