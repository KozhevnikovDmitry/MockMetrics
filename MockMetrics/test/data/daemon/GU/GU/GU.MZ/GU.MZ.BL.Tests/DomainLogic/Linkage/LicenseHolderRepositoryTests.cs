using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    /// <summary>
    /// Тесты на класс LicenseHolderRepository
    /// </summary>
    [TestFixture]
    public class LicenseHolderRepositoryTests
    {

        private LicenseHolderRepository _holderRepository;

        #region TestData

        private LicenseHolder _newLicenseHolder;

        private LicenseHolder _savedLicenseHolder;

        private LicenseHolder _existingLicenseHolder;

        /// <summary>
        /// Тестовый менеджер базы данных
        /// </summary>
        private IDomainDbManager _mockDb;

        /// <summary>
        /// Мок маппера лицензиатов
        /// </summary>
        private Mock<IDomainDataMapper<LicenseHolder>> _mockHolderMapper;

        /// <summary>
        /// Мок маппера лицензиатов
        /// </summary>
        private Mock<IDomainDataMapper<HolderRequisites>> _mockRequisitesMapper;
        
        #endregion
        
        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _newLicenseHolder = LicenseHolder.CreateInstance();
            _newLicenseHolder.Inn = "100500100500";
            _newLicenseHolder.Ogrn = "100500100500";
            
            _savedLicenseHolder = LicenseHolder.CreateInstance();
            _savedLicenseHolder.Inn = "100500100500";
            _savedLicenseHolder.Ogrn = "100500100500";
            _savedLicenseHolder.PersistentState = PersistentState.Old;

            _existingLicenseHolder = LicenseHolder.CreateInstance();
            _existingLicenseHolder.Id = 1;
            _existingLicenseHolder.Inn = "500100500100";
            _existingLicenseHolder.Ogrn = "500100500100";

            // мок менеджера базы
            _mockDb =
                Mock.Of<IDomainDbManager>(
                    db => db.GetDomainTable<LicenseHolder>()
                    == new EnumerableQuery<LicenseHolder>(new List<LicenseHolder> { _existingLicenseHolder }));

            // мок маппера лицензиатов
            _mockHolderMapper = new Mock<IDomainDataMapper<LicenseHolder>>();
            _mockHolderMapper.Setup(hm => hm.Save(_newLicenseHolder, _mockDb, false)).Returns(_savedLicenseHolder);
            _mockHolderMapper.Setup(hm => hm.Retrieve(1, _mockDb)).Returns(_existingLicenseHolder);

            // мок маппера реквизитов
            _mockRequisitesMapper = new Mock<IDomainDataMapper<HolderRequisites>>();
            _mockRequisitesMapper.Setup(rm => rm.Save(It.IsAny<HolderRequisites>(), It.IsAny<IDomainDbManager>(), false))
                    .Returns(Mock.Of<HolderRequisites>(hr => hr.Id == 1));

            // тестируемый объект
            _holderRepository = new LicenseHolderRepository(_mockHolderMapper.Object, _mockRequisitesMapper.Object);
        }

        /// <summary>
        /// Тест на добавление нового лицензиата в реестр
        /// </summary>
        [Test]
        public void AddNewLicenseHolderRegistrTest()
        {
            // Act
            LicenseHolder savedHolder = _holderRepository.AddNewLicenseHolder(_newLicenseHolder, _mockDb);

            // Assert
            Assert.AreEqual(savedHolder.PersistentState, PersistentState.Old);
        }

        /// <summary>
        /// Тест на корректное сохранение лицензиата в базу данных
        /// </summary>
        [Test]
        public void CorrectSaveHolderTest()
        {
            // Act
            _holderRepository.AddNewLicenseHolder(_newLicenseHolder, _mockDb);

            // Assert
            _mockHolderMapper.Verify(hm => hm.Save(_newLicenseHolder, _mockDb, false), Times.Once());
        }

        /// <summary>
        /// Тест на попытку добавления лицензиата с инн и огрн существующего лицензиата
        /// </summary>
        [Test]
        public void AddNewLicenseHolderWithDuplicatingTest()
        {
            // Arrange
            _newLicenseHolder.Inn = "500100500100";
            _newLicenseHolder.Ogrn = "500100500100";

            // Assert
            Assert.Throws<DuplicateLicenseHolderException>(() => _holderRepository.AddNewLicenseHolder(_newLicenseHolder, _mockDb));
        }

        /// <summary>
        /// Тесты на метод HolderExists
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        [TestCase("500100500100", "500100500100", Result = true)]
        [TestCase("WRONG", "500100500100", Result = true)]
        [TestCase("500100500100", "WRONG", Result = true)]
        [TestCase("WRONG", "WRONG", Result = false)]
        public bool HolderExistsTest(string inn, string ogrn)
        {
            // Assert
            return _holderRepository.HolderExists(inn, ogrn, _mockDb);
        }

        /// <summary>
        /// Тест на получение лицензиата из реестра по ИНН и OGRN
        /// </summary>
        [TestCase("500100500100", "500100500100", Result = true)]
        [TestCase("WRONG", "500100500100", Result = true)]
        [TestCase("500100500100", "WRONG", Result = true)]
        [TestCase("WRONG", "WRONG", ExpectedException = typeof(NoHolderFoundInRegistrException))]
        public bool GetLicenseHolderTest(string inn, string ogrn)
        {
            // Assert - проверяем, что возвращается существующий лицензиат 
            return _holderRepository.GetLicenseHolder(inn, ogrn, _mockDb) == _existingLicenseHolder;
        }

        /// <summary>
        /// Тест на корректное сохранение реквизитов лицензиата
        /// </summary>
        [Test]
        public void SaveRequisistesForHolderTest()
        {
            // Arrange
            var requisites = HolderRequisites.CreateInstance();

            // Act
            var savedRequisites = _holderRepository.SaveLicenseHolderRequisites(requisites, _mockDb);

            // Assert
            _mockRequisitesMapper.Verify(hm => hm.Save(requisites, _mockDb, false), Times.Once());
            Assert.AreEqual(savedRequisites.Id, 1);
        }

        /// <summary>
        /// Тест на ситуацию, в которой поиск лицензиата по ИНН и ОГРН выдаёт несколько результатов
        /// </summary>
        [Test]
        public void MultipleLicenseHolderByInnAndOgrnTest()
        {
            // Arrange
            _mockDb =
                Mock.Of<IDomainDbManager>(
                    db => db.GetDomainTable<LicenseHolder>()
                    == new EnumerableQuery<LicenseHolder>(new List<LicenseHolder> { _existingLicenseHolder, _existingLicenseHolder  }));

            // Assert
            Assert.Throws<MultipleHolderFoundInRegistrException>(
               () => _holderRepository.GetLicenseHolder(_existingLicenseHolder.Inn, _existingLicenseHolder.Ogrn, _mockDb));
        }
    }
}
