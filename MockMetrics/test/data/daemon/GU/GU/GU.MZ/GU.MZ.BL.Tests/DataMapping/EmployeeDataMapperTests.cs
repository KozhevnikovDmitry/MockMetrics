using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Person;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класса EmployeeDataMapper
    /// </summary>
    [TestFixture]
    public class EmployeeDataMapperTests : BaseTestFixture
    {
        private EmployeeDataMapper _employeeDataMapper;

        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private Mock<IDomainDbManager> _mockDb;

        /// <summary>
        /// Маппер томов дела
        /// </summary>
        private Mock<IDomainDataMapper<DossierFile>> _dossierFileMapper;

        /// <summary>
        /// Маппер томов дела
        /// </summary>
        private Mock<IDomainDataMapper<DbUser>> _dbUserMapper;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockDb = new Mock<IDomainDbManager>();
            _mockDb.Setup(db => db.RetrieveDomainObject<Employee>(It.IsAny<object>())).Returns(Employee.CreateInstance());
            _dossierFileMapper = new Mock<IDomainDataMapper<DossierFile>>(MockBehavior.Strict);
            _dossierFileMapper.Setup(dm => dm.Retrieve(It.IsAny<object>(), _mockDb.Object)).Returns(DossierFile.CreateInstance());

            _dbUserMapper = new Mock<IDomainDataMapper<DbUser>>(MockBehavior.Strict);
            _dbUserMapper.Setup(dm => dm.Retrieve(It.IsAny<object>(), _mockDb.Object)).Returns(Mock.Of<DbUser>(u => u.UserText == "UserText" && u.AppointText == "AppointText"));

            _employeeDataMapper = new EmployeeDataMapper(_dbUserMapper.Object, TestObjectMother.GetTestDomainContext(_mockDb.Object)) { DossierFileMapper = _dossierFileMapper.Object };
            TestObjectMother.SetDomainKey(_employeeDataMapper);
        }

        private EmployeeDataMapper BuildLicenseDossierDataMapper()
        {
            var employeeDataMapper = new EmployeeDataMapper(_dbUserMapper.Object, TestObjectMother.GetTestDomainContext(_mockDb.Object)) { DossierFileMapper = _dossierFileMapper.Object };
            TestObjectMother.SetDomainKey(employeeDataMapper);
            return employeeDataMapper;
        }

        /// <summary>
        /// Тест на получение ассоциации томА, за которые сотрудник ответсвеннен
        /// </summary>
        [Test]
        public void RetrieveNotNullDossierFileListTest()
        {
            // Arrange
            _mockDb.Setup(db => db.GetDomainTable<DossierFile>())
                   .Returns(new EnumerableQuery<DossierFile>(
                       new List<DossierFile> { Mock.Of<DossierFile>(t => t.Id == 1 && t.LicenseDossierId == 0 && t.CurrentStatus == DossierFileStatus.Active) }));
            _employeeDataMapper = BuildLicenseDossierDataMapper();

            // Act
            var employee = _employeeDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(employee.DossierFileList);
            Assert.AreEqual(employee.DossierFileList.Count, 1);
            _mockDb.Verify(t => t.RetrieveDomainObject<DossierFile>(It.IsAny<object>()), Times.Once());
        }

        /// <summary>
        /// Тест на получение ассоциации томА лицензионного дела
        /// </summary>
        [Test]
        public void RetrieveActiveDossierFileListTest()
        {
            // Arrange
            _mockDb.Setup(db => db.GetDomainTable<DossierFile>())
                   .Returns(new EnumerableQuery<DossierFile>(
                       new List<DossierFile> { Mock.Of<DossierFile>(t => t.Id == 1 && t.LicenseDossierId == 0 && t.CurrentStatus == DossierFileStatus.Closed),
                                               Mock.Of<DossierFile>(t => t.Id == 1 && t.LicenseDossierId == 0 && t.CurrentStatus == DossierFileStatus.Active),
                                               Mock.Of<DossierFile>(t => t.Id == 1 && t.LicenseDossierId == 0 && t.CurrentStatus == DossierFileStatus.Unbounded)}));
            _employeeDataMapper = BuildLicenseDossierDataMapper();

            // Act
            var employee = _employeeDataMapper.Retrieve(0);

            // Assert
            Assert.AreEqual(employee.DossierFileList.Count, 2);
            _mockDb.Verify(t => t.RetrieveDomainObject<DossierFile>(It.IsAny<object>()), Times.Exactly(2));
        }

        /// <summary>
        /// Тест на получение ассоциации Пользователь, под которым заведён сотрудник
        /// </summary>
        [Test]
        public void RetrieveNotNullDbUserTest()
        {
            // Arrange

            // Act
            var employee = _employeeDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(employee.DbUser);
            Assert.AreEqual(employee.Name, "UserText");
            Assert.AreEqual(employee.Position, "AppointText");
        }
        
        /// <summary>
        /// Тест на заполнение ассоциации Пользователь, под которым заведён сотрудник
        /// </summary>
        [Test]
        public void FillAssociationsNotNullDbUserTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DbUser>(It.IsAny<object>())).Returns(
                Mock.Of<DbUser>(u => u.UserText == "UserText" && u.AppointText == "AppointText"));
            _employeeDataMapper = BuildLicenseDossierDataMapper();
            var employee = Employee.CreateInstance();

            // Act
            _employeeDataMapper.FillAssociations(employee);

            // Assert
            Assert.NotNull(employee.DbUser);
            Assert.AreEqual(employee.Name, "UserText");
            Assert.AreEqual(employee.Position, "AppointText");
        }
    }
}
