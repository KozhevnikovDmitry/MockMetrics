using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    /// <summary>
    /// Тесты на класс LicenseDossierRegistr
    /// </summary>
    [TestFixture]
    public class LicenseDossierRegistrTests
    {
        /// <summary>
        /// Тестируемый реестр лицензионных дел.
        /// </summary>
        private LicenseDossierRepository _dossierRepository;

        #region Test data

        /// <summary>
        /// Тестовый менеджер базы данных
        /// </summary>
        private IDomainDbManager _mockDb;

        /// <summary>
        /// Мок маппера лицензионных дел
        /// </summary>
        private Mock<IDomainDataMapper<LicenseDossier>> _mockDossierMapper;

        /// <summary>
        /// Новое дело
        /// </summary>
        private LicenseDossier _newDossier;

        /// <summary>
        /// Существующее дело
        /// </summary>
        private LicenseDossier _existingDossier;

        /// <summary>
        /// Сохранённое дело
        /// </summary>
        private LicenseDossier _savedDossier;

        /// <summary>
        /// Существующий лицензиат
        /// </summary>
        private LicenseHolder _existingLicenseHolder;
        
        #endregion

        /// <summary>
        /// Настройки перед каждым тестом
        /// </summary>  
        [SetUp]
        public void SetupTest()
        {
            _newDossier = LicenseDossier.CreateInstance();
            _newDossier.LicensedActivityId = 1;
            _newDossier.LicenseHolderId = 1;

            _existingDossier = LicenseDossier.CreateInstance();
            _existingDossier.LicensedActivityId = 2;
            _existingDossier.LicenseHolderId = 1;
            _existingDossier.Id = 2;
            _existingDossier.IsActive = true;

            _savedDossier = LicenseDossier.CreateInstance();
            _savedDossier.LicensedActivityId = 1;
            _savedDossier.PersistentState = PersistentState.Old;
            _savedDossier.Id = 1;
            _savedDossier.IsActive = true;

            _existingLicenseHolder = LicenseHolder.CreateInstance();
            _existingLicenseHolder.Id = 1;

            _mockDb =
                Mock.Of<IDomainDbManager>(db => 
                    db.GetDomainTable<LicenseHolder>()
                    == new EnumerableQuery<LicenseHolder>(new List<LicenseHolder> { _existingLicenseHolder }) 
                    &&
                    db.GetDomainTable<LicenseDossier>()
                    == new EnumerableQuery<LicenseDossier>(new List<LicenseDossier> { _existingDossier }));

            _mockDossierMapper = new Mock<IDomainDataMapper<LicenseDossier>>();
            _mockDossierMapper.Setup(dm => dm.Save(_newDossier, _mockDb, false)).Returns(_savedDossier);
            _mockDossierMapper.Setup(dm => dm.Retrieve(_existingDossier.Id, _mockDb)).Returns(_existingDossier);

            _dossierRepository = new LicenseDossierRepository(_mockDossierMapper.Object, () => _mockDb);
        }

        /// <summary>
        /// Тест на добавление нового лицензионного дела в реестр
        /// </summary>
        [Test]
        public void AddNewLicenseDossierTest()
        {
            // Act
            var savedDossier = _dossierRepository.AddNewLicenseDossier(_newDossier, _mockDb);

            // Assert
            Assert.AreEqual(savedDossier.PersistentState, PersistentState.Old);
        }

        /// <summary>
        /// Тест на корректное сохранение дела в базу данных
        /// </summary>
        [Test]
        public void CorrectSaveDossierTest()
        {
            // Act
            _dossierRepository.AddNewLicenseDossier(_newDossier, _mockDb);

            // Assert
            _mockDossierMapper.Verify(dm => dm.Save(_newDossier, _mockDb, false), Times.Once());
        }

        /// <summary>
        /// Тесты на проверку наличия лицензионного дела по лиц. деятельности и лицензиату
        /// </summary>
        [TestCase(1, 1, true, Result = false)]
        [TestCase(2, 1, true, Result = true)]
        [TestCase(2, 1, false, Result = false)]
        [TestCase(1, 1, false, Result = false)]
        [TestCase(2, 1, false, Result = false)]
        [TestCase(1, 2, true, Result = false)]
        [TestCase(1, 2, false, Result = false)]
        public bool DossierExistsTest(int licensedActivityId, int licenseHolderId, bool isActive)
        {
            // Arrange
            _existingDossier.IsActive = isActive;

            // Assert
            return _dossierRepository.DossierExists(licensedActivityId, licenseHolderId, _mockDb);
        }

        /// <summary>
        /// Тесты на добавление лицензионного дела 
        /// </summary>
        [TestCase(1, 1, true, Result = true)]
        [TestCase(1, 0, true, ExpectedException = typeof(NoBoundedLicenseHolderException))]
        [TestCase(2, 1, true, ExpectedException = typeof(DuplicateDossierException))]
        [TestCase(2, 1, false, Result = true)]
        public bool AddDossierTest(int licensedActivityId, int licenseHolderId, bool isActive)
        {
            // Arrange
            _newDossier.LicensedActivityId = licensedActivityId;
            _newDossier.LicenseHolderId = licenseHolderId;
            _existingDossier.IsActive = isActive;

            // Assert
            return _dossierRepository.AddNewLicenseDossier(_newDossier, _mockDb) == _savedDossier;
        }

        /// <summary>
        /// Тесты на получение лицензионного дела по виду деятельности и лицензиату
        /// </summary>
        [TestCase(0, 1, ExpectedException = typeof(NoBoundedLicenseHolderException))]
        [TestCase(2, 0, ExpectedException = typeof(NoBoundedLicenseHolderException))]
        [TestCase(2, 1, Result = true)]
        public bool GetLicenseDossierTest(int licensedActivityId, int licenseHolderId)
        {
            // Act
            var dossier = _dossierRepository.GetLicenseDossier(licensedActivityId, licenseHolderId, _mockDb);

            // Assert
            return dossier == _existingDossier;
        }

        /// <summary>
        /// Тест на получение следующего рег номера тома для нового лицензионного дела
        /// </summary>
        [Test]
        public void GetNextFileRegNumberForNewDossierTest()
        {
            // Act
            int regNumber = _dossierRepository.GetNextFileRegNumber(LicenseDossier.CreateInstance(), _mockDb);

            // Assert
            Assert.AreEqual(regNumber, 1);
        }

        /// <summary>
        /// Тест на получение следующего рег номера старого лицензионного дела
        /// </summary>
        [Test]
        public void GetNextFileRegNumberForOldDossierTest()
        {
            // Arrange
            _mockDb = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<DossierFile>() == 
                                                new EnumerableQuery<DossierFile>(new List<DossierFile>
                                                    {
                                                        Mock.Of<DossierFile>(d => d.LicenseDossierId == 1 && d.RegNumber == 1),
                                                        Mock.Of<DossierFile>(d => d.LicenseDossierId == 1 && d.RegNumber == 100500),
                                                        Mock.Of<DossierFile>(d => d.LicenseDossierId == 2 && d.RegNumber == 500100),
                                                    }));

            // Act
            int regNumber = _dossierRepository.GetNextFileRegNumber(Mock.Of<LicenseDossier>(t => t.Id == 1), _mockDb);

            // Assert
            Assert.AreEqual(regNumber, 100501);
        }

        /// <summary>
        /// Тест на получение следующего рег номера старого лицензионного дела без томов
        /// </summary>
        [Test]
        public void GetNextFileRegNumberEquals1WhenNoFilesTest()
        {
            // Assert
            Assert.AreEqual(_dossierRepository.GetNextFileRegNumber(Mock.Of<LicenseDossier>(t => t.Id == 1), _mockDb), 1);
        }
    }
}
