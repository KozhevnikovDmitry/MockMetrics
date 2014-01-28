using System;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
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
    /// Тесты на класс DossierFileRegistr
    /// </summary>
    [TestFixture]
    public class DossierFileRepositoryTests : BaseTestFixture
    {
        #region Fakes

        private LicenseHolderRepository _stubHolderRepository;

        private LicenseDossierRepository _stubDossierRepository;

        private IDomainDataMapper<DossierFile> _stubFileMapper;

        private IDomainDataMapper<DocumentInventory> _stubInventoryMapper;

        private Func<IDomainDbManager> _stubGetDb;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // дефолтные стабы
            _stubHolderRepository = Mock.Of<LicenseHolderRepository>();
            _stubDossierRepository = Mock.Of<LicenseDossierRepository>();
            _stubFileMapper = Mock.Of<IDomainDataMapper<DossierFile>>();
            _stubInventoryMapper = Mock.Of<IDomainDataMapper<DocumentInventory>>();
            _stubGetDb = Mock.Of<IDomainDbManager>;
        }

        /// <summary>
        /// Тест на сохранение тома через маппер
        /// </summary>
        [Test]
        public void SaveLinkagedNewDossierFileForNewHolderTest()
        {
            // Arrange
            var savedFile = Mock.Of<DossierFile>();

            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseHolder == holder);
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged && t.IsDossierNew && t.IsHolderNew && t.LicenseDossier == dossier);

            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(t => t.AddNewLicenseHolder(holder, It.IsAny<IDomainDbManager>()) == holder),
                                                 Mock.Of<LicenseDossierRepository>(t => t.AddNewLicenseDossier(dossier, It.IsAny<IDomainDbManager>()) == dossier),
                                                 Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(dossierFile, It.IsAny<IDomainDbManager>(), false) == savedFile),
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            var savedDossierFile = registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            Assert.AreEqual(savedDossierFile, savedFile);
        }

        /// <summary>
        /// Тест на попытку сохранения непривязанного тома лицензионного дела
        /// </summary>
        [Test]
        public void SaveUnlinkagedDossierFileTest()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged == false);
            var registr = new DossierFileRepository(_stubHolderRepository,
                                                 _stubDossierRepository,
                                                 _stubFileMapper,
                                                 _stubInventoryMapper,
                                                 _stubGetDb);
            // Assert
            Assert.Throws<SaveUnlinkagedDossierFileException>(() =>
                registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile)));
        }

        /// <summary>
        /// Тест на проставление внешних ключей нового дела нового лицензиате при сохранении
        /// </summary>
        [Test]
        public void SetupLicenseDossierConstraintsPerSavingHolder()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.Id == 100500 && t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseHolder == holder);
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged && t.IsDossierNew && t.IsHolderNew && t.LicenseDossier == dossier);

            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(t => t.AddNewLicenseHolder(holder, It.IsAny<IDomainDbManager>()) == holder),
                                                 Mock.Of<LicenseDossierRepository>(t => t.AddNewLicenseDossier(dossier, It.IsAny<IDomainDbManager>()) == dossier),
                                                 Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(dossierFile, It.IsAny<IDomainDbManager>(), false) == dossierFile),
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            var savedFile = registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            Assert.AreEqual(savedFile.LicenseDossier.LicenseHolderId, 100500);
        }

        /// <summary>
        /// Тест на проставление внешних ключей тома нового дела нового лицензиатепри сохранении
        /// </summary>
        [Test]
        public void SetupDossierFileConstraintsPerSave()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>(t => t.RequisitesList == new EditableList<HolderRequisites> { Mock.Of<HolderRequisites>() });
            var dossier = Mock.Of<LicenseDossier>(t => t.Id == 100500 && t.LicenseHolder == holder);
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged && t.IsDossierNew && t.IsHolderNew && t.LicenseDossier == dossier);

            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(t => t.AddNewLicenseHolder(holder, It.IsAny<IDomainDbManager>()) == holder),
                                                 Mock.Of<LicenseDossierRepository>(t => t.AddNewLicenseDossier(dossier, It.IsAny<IDomainDbManager>()) == dossier),
                                                 Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(dossierFile, It.IsAny<IDomainDbManager>(), false) == dossierFile),
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            var savedFile = registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            Assert.AreEqual(savedFile.LicenseDossierId, 100500);
        }

        /// <summary>
        /// Тест на отсутствие сохранения существуюшего лицензиата
        /// </summary>
        [Test]
        public void NotSaveExistingLicenseHolderPerSave()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>();
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged && t.IsDossierNew && t.IsHolderNew == false && t.LicenseDossier == dossier);

            var mockHolderRegistr = new Mock<LicenseHolderRepository>();

            var registr = new DossierFileRepository(mockHolderRegistr.Object,
                                                 Mock.Of<LicenseDossierRepository>(t => t.AddNewLicenseDossier(dossier, It.IsAny<IDomainDbManager>()) == dossier),
                                                 _stubFileMapper,
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            mockHolderRegistr.Verify(t => t.AddNewLicenseHolder(It.IsAny<LicenseHolder>(), It.IsAny<IDomainDbManager>()), Times.Never());
        }

        /// <summary>
        /// Тест на отсутствие сохранения существуюшего дела
        /// </summary>
        [Test]
        public void NotSaveExistingDossierPerSave()
        {
            // Arrange
            var dossierFile = Mock.Of<DossierFile>(t => t.IsLinkaged && t.IsDossierNew == false);

            var mockHolderRegistr = new Mock<LicenseHolderRepository>();
            var mockDossierRegistr = new Mock<LicenseDossierRepository>();

            var registr = new DossierFileRepository(mockHolderRegistr.Object,
                                                 mockDossierRegistr.Object,
                                                 _stubFileMapper,
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            mockHolderRegistr.Verify(t => t.AddNewLicenseHolder(It.IsAny<LicenseHolder>(), It.IsAny<IDomainDbManager>()), Times.Never());
            mockDossierRegistr.Verify(t => t.AddNewLicenseDossier(It.IsAny<LicenseDossier>(), It.IsAny<IDomainDbManager>()), Times.Never());
        }

        /// <summary>
        /// Тест на отсутствие сохранения существуюшего лицензиата
        /// </summary>
        [Test]
        public void NotSaveExistingHolderRequisitesPerSave()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>();
            var dossierFile =
                Mock.Of<DossierFile>(
                    t =>
                    t.IsLinkaged && t.IsDossierNew && t.IsHolderNew == false && t.IsLinkagedWithNewRequisites == false && t.LicenseDossier == dossier);

            var mockHolderRegistr = new Mock<LicenseHolderRepository>();

            var registr = new DossierFileRepository(mockHolderRegistr.Object,
                                                 Mock.Of<LicenseDossierRepository>(t => t.AddNewLicenseDossier(dossier, It.IsAny<IDomainDbManager>()) == dossier),
                                                 _stubFileMapper,
                                                 _stubInventoryMapper,
                                                 _stubGetDb);

            // Act
            registr.SaveLinkagedDossierFile(Mock.Of<DossierFile>(t => t.Clone() == dossierFile));

            // Assert
            mockHolderRegistr.Verify(t => t.AddNewLicenseHolder(It.IsAny<LicenseHolder>(), It.IsAny<IDomainDbManager>()), Times.Never());
        }
        
        /// <summary>
        /// Тест на сохранение прототипа тома при приёме
        /// </summary>
        [Test]
        public void SaveDossierFilePerAcceptTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var inventory = Mock.Of<DocumentInventory>();
            var file = Mock.Of<DossierFile>(t => t.DocumentInventory == inventory);
            var mockFileMapper = new Mock<IDomainDataMapper<DossierFile>>();
            mockFileMapper.Setup(t => t.Save(file, db, false)).Returns(Mock.Of<DossierFile>);
            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(),
                                                 Mock.Of<LicenseDossierRepository>(),
                                                 mockFileMapper.Object,
                                                 Mock.Of<IDomainDataMapper<DocumentInventory>>(t => t.Save(inventory, db, false) == inventory),
                                                 () => db);

            // Act
            registr.AcceptDossierFile(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            mockFileMapper.Verify(t => t.Save(file, db, false), Times.Once());
        }

        /// <summary>
        /// Тест на сохранение описи представленных документов при приёме прототипа
        /// </summary>
        [Test]
        public void SaveInventoryPerAcceptTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var inventory = Mock.Of<DocumentInventory>();
            var file = Mock.Of<DossierFile>(t => t.DocumentInventory == inventory);
            var mockInventoryMapper = new Mock<IDomainDataMapper<DocumentInventory>>();
            mockInventoryMapper.Setup(t => t.Save(inventory, db, false)).Returns(inventory);
            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(),
                                                 Mock.Of<LicenseDossierRepository>(),
                                                 Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(file, db, false) == Mock.Of<DossierFile>()),
                                                 mockInventoryMapper.Object,
                                                 () => db);

            // Act
            registr.AcceptDossierFile(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            mockInventoryMapper.Verify(t => t.Save(inventory, db, false), Times.Once());
        }

        [Test]
        public void SetupDossierFileInventoryConstraintPerSave()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var inventory = Mock.Of<DocumentInventory>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.DocumentInventory == inventory);
            var registr = new DossierFileRepository(Mock.Of<LicenseHolderRepository>(),
                                                 Mock.Of<LicenseDossierRepository>(),
                                                 Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Save(file, db, false) == file),
                                                 Mock.Of<IDomainDataMapper<DocumentInventory>>(t => t.Save(inventory, db, false) == inventory),
                                                 () => db);

            // Act
            var result = registr.AcceptDossierFile(Mock.Of<DossierFile>(t => t.Clone() == file));

            // Assert
            Assert.AreEqual(result.DocumentInventory, inventory);
        }
    }
}
