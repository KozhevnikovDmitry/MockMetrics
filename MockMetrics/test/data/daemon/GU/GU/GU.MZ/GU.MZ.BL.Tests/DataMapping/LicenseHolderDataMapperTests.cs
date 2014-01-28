using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты для класса LicenseHolderMapper.
    /// </summary>
    [TestFixture]
    public class LicenseHolderDataMapperTests : BaseTestFixture
    {
        /// <summary>
        /// Тестируемый маппер лицензиатов
        /// </summary>
        private LicenseHolderDataMapper _licenseHolderMapper;

        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private Mock<IDomainDbManager> _mockDb;

        private Mock<IDomainDataMapper<HolderRequisites>> _requisitesMapper;

            #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockDb = TestObjectMother.GetStrictMockDbManager();
            _mockDb.Setup(db => db.SaveDomainObject(It.IsAny<LicenseHolder>()));
            _mockDb.Setup(db => db.RetrieveDomainObject<LicenseHolder>(It.IsAny<object>())).Returns(LicenseHolder.CreateInstance());
            _mockDb.Setup(db => db.RetrieveDomainObject<HolderRequisites>(It.IsAny<object>())).Returns(HolderRequisites.CreateInstance());
            _mockDb.Setup(db => db.GetDomainTable<HolderRequisites>()).Returns(new EnumerableQuery<HolderRequisites>(new List<HolderRequisites> { HolderRequisites.CreateInstance() }));

            _requisitesMapper = new Mock<IDomainDataMapper<HolderRequisites>>();
            _requisitesMapper.Setup(rm => rm.Retrieve(It.IsAny<object>(), It.IsAny<IDomainDbManager>())).Returns(HolderRequisites.CreateInstance());
            _requisitesMapper.Setup(rm => rm.Save(It.IsAny<HolderRequisites>(), It.IsAny<IDomainDbManager>(), false)).Returns(HolderRequisites.CreateInstance());
            _licenseHolderMapper = BuildLicenseHolderDataMapper();
        }

        private LicenseHolderDataMapper BuildLicenseHolderDataMapper()
        {
            var mapper = new LicenseHolderDataMapper(_requisitesMapper.Object, TestObjectMother.GetTestDomainContext(_mockDb.Object));
            TestObjectMother.SetDomainKey(mapper);
            return mapper;
        }

        #region Retrieve Tests
        
        /// <summary>
        /// Тест на получение ассоциации адрес
        /// </summary>
        [Test]
        public void RetrieveNotNullRequisitesListTest()
        {
            // Act
            var obj = _licenseHolderMapper.Retrieve(0);

            // Assert
            Assert.NotNull(obj.RequisitesList);
            Assert.AreNotEqual(obj.RequisitesList.Count, 0);
        }

        /// <summary>
        /// Тест на проставление ассоциации Лицензиат у всех реквизитов
        /// </summary>
        [Test]
        public void RetrieveNotNullHolderPerRequisitesTest()
        {
            // Act
            var obj = _licenseHolderMapper.Retrieve(0);

            // Assert
            foreach (var holderRequisites in obj.RequisitesList)
            {
                Assert.AreEqual(holderRequisites.LicenseHolderId, obj.Id);
                Assert.AreSame(holderRequisites.LicenseHolder, obj);
            }
        }

        #endregion

        #region Save Tests

        /// <summary>
        /// Тест на сохранение ассоциации Реквизиты
        /// </summary>
        [Test]
        public void SaveRequisitesTest()
        {
            // Arrange
            var obj = LicenseHolder.CreateInstance();
            obj.Id = 1;
            var requisites = HolderRequisites.CreateInstance();
            requisites.LicenseHolder = obj;
            obj.RequisitesList = new EditableList<HolderRequisites> { requisites };

            _requisitesMapper.Setup(rm => rm.Save(It.IsAny<HolderRequisites>(), It.IsAny<IDomainDbManager>(), false)).
                Returns(Mock.Of<HolderRequisites>(r => r.Id == 1 && r.LicenseHolderId == obj.Id));

            _licenseHolderMapper = BuildLicenseHolderDataMapper();

            // Act
            var savedHolder = _licenseHolderMapper.Save(Mock.Of<LicenseHolder>(t => t.Clone() == obj), _mockDb.Object);

            // Assert
            foreach (var holderRequisites in savedHolder.RequisitesList)
            {
                Assert.AreEqual(holderRequisites.LicenseHolderId, savedHolder.Id);
                Assert.AreNotEqual(holderRequisites.Id, 0);
            }
        }
        
        #endregion

        #region FillAssociations Tests

        /// <summary>
        /// Тест на заполнении ассоциации реквизиты лицензиата
        /// </summary>
        [Test]
        public void NotNullFillAssociationsRequisitesListTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 1);

            _mockDb.Setup(db => db.GetDomainTable<HolderRequisites>())
                        .Returns((new List<HolderRequisites> { Mock.Of<HolderRequisites>(t => t.LicenseHolderId == 1) }).AsQueryable());

            // Act
            _licenseHolderMapper.FillAssociations(holder);

            // Assert
            Assert.NotNull(holder.RequisitesList);
            Assert.AreNotEqual(holder.RequisitesList.Count, 0);
            _requisitesMapper.Verify(t => t.Retrieve(It.IsAny<object>(), It.IsAny<IDomainDbManager>()), Times.Once());
        }

        /// <summary>
        /// Тест на проставление обратной ссылки на лицензиата у ассоциированных реквизитов при вызове FillAssociations
        /// </summary>
        [Test]
        public void NotNullFillAssociationsRequisitesHolderTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 1);

            _mockDb.Setup(db => db.GetDomainTable<HolderRequisites>())
                        .Returns((new List<HolderRequisites> { Mock.Of<HolderRequisites>(t => t.LicenseHolderId == 1) }).AsQueryable());

            // Act
            _licenseHolderMapper.FillAssociations(holder);

            // Assert
            foreach (var holderRequisites in holder.RequisitesList)
            {
                Assert.NotNull(holderRequisites.LicenseHolder);
            }
        }
        
        #endregion
    }
}
