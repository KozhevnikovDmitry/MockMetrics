using System.Linq;
using BLToolkit.EditableObjects;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класса LicenseObjectMapper
    /// </summary>
    [TestFixture]
    public class LicenseObjectDataMapperTests : BaseTestFixture
    {
        /// <summary>
        /// Тестируемый маппер  
        /// </summary>
        private LicenseObjectDataMapper _licenseObjectMapper;

        #region Test Data

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
            _mockDb = TestObjectMother.GetStrictMockDbManager();
            _mockDb.Setup(db => db.SaveDomainObject<LicenseObject>(It.IsAny<LicenseObject>()));
            _mockDb.Setup(db => db.SaveDomainObject<ObjectSubactivity>(It.IsAny<ObjectSubactivity>()));
            _mockDb.Setup(db => db.SaveDomainObject<Address>(It.IsAny<Address>(), false));
            _mockDb.Setup(db => db.RetrieveDomainObject<LicenseObject>(It.IsAny<object>())).Returns(LicenseObject.CreateInstance());
            _mockDb.Setup(db => db.RetrieveDomainObject<ObjectSubactivity>(It.IsAny<object>())).Returns(ObjectSubactivity.CreateInstance());
            _mockDb.Setup(db => db.RetrieveDomainObject<Address>(It.IsAny<object>())).Returns(Address.CreateInstance());
            _mockDb.Setup(db => db.GetDomainTable<ObjectSubactivity>()).Returns(
                new EnumerableQuery<ObjectSubactivity>(new ObjectSubactivity[]
                    {
                        Mock.Of<ObjectSubactivity>(t => t.Id == 1 && t.LicenseObjectId == 1),
                        Mock.Of<ObjectSubactivity>(t => t.Id == 2 && t.LicenseObjectId == 1),
                        Mock.Of<ObjectSubactivity>(t => t.Id == 3 && t.LicenseObjectId == 2)
                    }));

            _licenseObjectMapper = BuildLicenseObjectDataMapper();
        }

        private LicenseObjectDataMapper BuildLicenseObjectDataMapper()
        {
            var mapper = new LicenseObjectDataMapper(TestObjectMother.GetTestDomainContext(_mockDb.Object));
            TestObjectMother.SetDomainKey(mapper);
            return mapper;
        }

        /// <summary>
        /// Тест на получение ассоциации Адрес
        /// </summary>
        [Test]
        public void RetrieveAddressTest()
        {
            // Act
            var obj = _licenseObjectMapper.Retrieve(0);

            // Assert   
            Assert.NotNull(obj.Address);
        }

        /// <summary>
        /// Тест на получение ассоциации списка поддеятельностей объекта
        /// </summary>
        [Test]
        public void RetrieveObjectSubactivityListTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();
            licenseObject.Id = 1;
            _mockDb.Setup(db => db.RetrieveDomainObject<LicenseObject>(It.IsAny<object>())).Returns(licenseObject);

            _licenseObjectMapper = BuildLicenseObjectDataMapper();

            // Act
            var obj = _licenseObjectMapper.Retrieve(1);

            // Assert
            Assert.AreEqual(obj.ObjectSubactivityList.Count, 2);
            _mockDb.Verify(t => t.RetrieveDomainObject<ObjectSubactivity>(It.IsAny<object>()), Times.Exactly(2));
        }


        /// <summary>
        /// Тест на сохранение ассоциации адреса
        /// </summary>
        [Test]
        public void SaveAddressTest()
        {
            // Arrange
            var obj = LicenseObject.CreateInstance();
            obj.Address = Address.CreateInstance();
            obj.Address.Id = 1;

            // Act
            var savedObject = _licenseObjectMapper.Save(obj);

            // Assert
            _mockDb.Verify(t => t.SaveDomainObject(It.IsAny<Address>(), false), Times.Once());
            Assert.AreEqual(savedObject.AddressId, savedObject.Address.Id);
            Assert.AreNotEqual(savedObject.AddressId, 0);
        }



        /// <summary>
        /// Тест на сохранение ассоциации списка поддеятельностей объекта
        /// </summary>
        [Test]
        public void SaveObjectSubastivityListTest()
        {
            // Arrange
            var obj = LicenseObject.CreateInstance();
            obj.Id = 1;
            obj.Address = Address.CreateInstance();
            obj.ObjectSubactivityList = new EditableList<ObjectSubactivity> { ObjectSubactivity.CreateInstance(), ObjectSubactivity.CreateInstance() };

            // Act
            var savedLicense = _licenseObjectMapper.Save(obj, _mockDb.Object);

            // Assert
            foreach (var objectSubactivity in savedLicense.ObjectSubactivityList)
            {
                Assert.AreEqual(objectSubactivity.LicenseObjectId, 1);
            }

            _mockDb.Verify(t => t.SaveDomainObject<ObjectSubactivity>(It.IsAny<ObjectSubactivity>()), Times.Exactly(2));
        }


        /// <summary>
        /// Тест на удаление помеченных на удаление поддеятельностей объекта
        /// </summary>
        [Test]
        public void DeleteMarkDeletedObjectSubactivitiesTest()
        {
            // Arrange
            var licenseObject = LicenseObject.CreateInstance();
            var objectSubactivity = ObjectSubactivity.CreateInstance();

            licenseObject.ObjectSubactivityList = new EditableList<ObjectSubactivity> { ObjectSubactivity.CreateInstance() };
            licenseObject.ObjectSubactivityList.Add(objectSubactivity);
            licenseObject.ObjectSubactivityList.AcceptChanges();
            licenseObject.ObjectSubactivityList.Remove(objectSubactivity);

            // Act
            _licenseObjectMapper.Save(licenseObject, _mockDb.Object);

            // Assert
            _mockDb.Verify(t => t.SaveDomainObject(It.Is<ObjectSubactivity>(s => s.PersistentState == PersistentState.NewDeleted)), Times.Once());
        }
    }
}
