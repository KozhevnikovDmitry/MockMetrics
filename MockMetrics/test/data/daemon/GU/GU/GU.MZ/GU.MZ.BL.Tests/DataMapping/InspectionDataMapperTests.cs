using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класса InspectionDataMapper
    /// </summary>
    [TestFixture]
    public class InspectionDataMapperTests : BaseTestFixture
    {
        private InspectionDataMapper _inspectionDataMapper;

        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private Mock<IDomainDbManager> _mockDb;
        
        /// <summary>
        /// Маппер экспертов
        /// </summary>
        private Mock<IDomainDataMapper<Expert>> _mockExpertMapper;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // мок менеджера базы
            _mockDb = new Mock<IDomainDbManager>();
            _mockDb.Setup(db => db.RetrieveDomainObject<Inspection>(It.IsAny<object>())).Returns(Inspection.CreateInstance());

            // мок маппера экспертов
            _mockExpertMapper = new Mock<IDomainDataMapper<Expert>>();

            _inspectionDataMapper = new InspectionDataMapper(TestObjectMother.GetTestDomainContext(_mockDb.Object), 
                                                                  _mockExpertMapper.Object);


            TestObjectMother.SetDomainKey(_inspectionDataMapper);
        }

        /// <summary>
        /// Тест на получение ассоциоации список привлечённых сотрудников
        /// </summary>
        [Test]
        public void RetrieveInspectionEmployeeListTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<Inspection>(It.IsAny<object>()))
                                       .Returns(Mock.Of<Inspection>(d => d.Id == 1));

            _mockDb.Setup(db => db.RetrieveDomainObject<InspectionEmployee>(It.IsAny<object>()))
                                       .Returns(Mock.Of<InspectionEmployee>(d => d.Id == 1));

            _mockDb.Setup(db => db.GetDomainTable<InspectionEmployee>())
                                       .Returns(new EnumerableQuery<InspectionEmployee>(new List<InspectionEmployee>
                                                                                                 {
                                                                                                     Mock.Of<InspectionEmployee>(t => t.InspectionId == 1),
                                                                                                     Mock.Of<InspectionEmployee>(t => t.InspectionId == 2)
                                                                                                 }));

            _mockDb.Setup(db => db.RetrieveDomainObject<Employee>(It.IsAny<object>()))
                                       .Returns(Mock.Of<Employee>(d => d.DbUserId == 1));

            // Act
            var obj = _inspectionDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(obj.InspectionEmployeeList);
            Assert.AreEqual(obj.InspectionEmployeeList.Count, 1);
            _mockDb.Verify(t => t.RetrieveDomainObject<Employee>(It.IsAny<object>()), Times.Once());
            _mockDb.Verify(t => t.RetrieveDomainObject<DbUser>(It.IsAny<object>()), Times.Once());
        }

        /// <summary>
        /// Тест на получение ассоциоации список привлечённых экспертов
        /// </summary>
        [Test]
        public void RetrieveInspectionExpertListTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<Inspection>(It.IsAny<object>()))
                                       .Returns(Mock.Of<Inspection>(d => d.Id == 1));

            _mockDb.Setup(db => db.RetrieveDomainObject<InspectionExpert>(It.IsAny<object>()))
                                       .Returns(Mock.Of<InspectionExpert>(d => d.Id == 1));

            _mockDb.Setup(db => db.GetDomainTable<InspectionExpert>())
                                       .Returns(new EnumerableQuery<InspectionExpert>(new List<InspectionExpert>
                                                                                                 {
                                                                                                     Mock.Of<InspectionExpert>(t => t.InspectionId == 1),
                                                                                                     Mock.Of<InspectionExpert>(t => t.InspectionId == 2)
                                                                                                 }));

            // Act
            var obj = _inspectionDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(obj.InspectionExpertList);
            Assert.AreEqual(obj.InspectionExpertList.Count, 1);
            _mockExpertMapper.Verify(t => t.Retrieve(It.IsAny<object>(), _mockDb.Object), Times.Once());
        }

        /// <summary>
        /// Тест на сохранение ассоциации список привлечённых сотрудников
        /// </summary>
        [Test]
        public void SaveInspectionEmployeeListTest()
        {
            // Arrange
            var obj = Inspection.CreateInstance();
            obj.InspectionEmployeeList = new EditableList<InspectionEmployee> { InspectionEmployee.CreateInstance() };

            // Act
            var savedInspection = _inspectionDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.False(savedInspection.InspectionEmployeeList.IsDirty);
            Assert.AreEqual(savedInspection.InspectionEmployeeList.Count, 1);
            Assert.AreEqual(savedInspection.InspectionEmployeeList.Single().InspectionId, savedInspection.Id);
            _mockDb.Verify(t => t.SaveDomainObject(It.IsAny<InspectionEmployee>()), Times.Once());
        }

        /// <summary>
        /// Тест на сохранение ассоциации список привлечённых сотрудников
        /// </summary>
        [Test]
        public void SaveInspectionExpertListTest()
        {
            // Arrange
            var obj = Inspection.CreateInstance();
            obj.InspectionExpertList = new EditableList<InspectionExpert> { InspectionExpert.CreateInstance() };

            // Act
            var savedInspection = _inspectionDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.False(savedInspection.InspectionExpertList.IsDirty);
            Assert.AreEqual(savedInspection.InspectionExpertList.Count, 1);
            Assert.AreEqual(savedInspection.InspectionExpertList.Single().InspectionId, savedInspection.Id);
            _mockDb.Verify(t => t.SaveDomainObject(It.IsAny<InspectionExpert>()), Times.Once());
        }

        /// <summary>
        /// Тест на удаление помеченных на удаление привлёченных сотрудников
        /// </summary>
        [Test]
        public void DeleteMarkDeletedInspectionEmployeesTest()
        {
            // Arrange
            var obj = Inspection.CreateInstance();
            var inspectionEmployee = InspectionEmployee.CreateInstance();

            obj.InspectionEmployeeList = new EditableList<InspectionEmployee> { InspectionEmployee.CreateInstance() };
            obj.InspectionEmployeeList.Add(inspectionEmployee);
            obj.InspectionEmployeeList.AcceptChanges();
            obj.InspectionEmployeeList.Remove(inspectionEmployee);

            // Act
            _inspectionDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.AreEqual(inspectionEmployee.PersistentState, PersistentState.NewDeleted);
            _mockDb.Verify(t => t.SaveDomainObject(inspectionEmployee), Times.Once());
        }

        /// <summary>
        /// Тест на удаление помеченных на удаление привлёченных экспертов
        /// </summary>
        [Test]
        public void DeleteMarkDeletedInspectionExpertsTest()
        {
            // Arrange
            var obj = Inspection.CreateInstance();
            var inspectionExpert = InspectionExpert.CreateInstance();

            obj.InspectionExpertList = new EditableList<InspectionExpert> { InspectionExpert.CreateInstance() };
            obj.InspectionExpertList.Add(inspectionExpert);
            obj.InspectionExpertList.AcceptChanges();
            obj.InspectionExpertList.Remove(inspectionExpert);

            // Act
            _inspectionDataMapper.Save(obj, _mockDb.Object);

            // Assert
            Assert.AreEqual(inspectionExpert.PersistentState, PersistentState.NewDeleted);
            _mockDb.Verify(t => t.SaveDomainObject(inspectionExpert), Times.Once());
        }
    }
}
