using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класса FileScenarioStepDataMapper
    /// </summary>
    [TestFixture]
    public class FileScenarioStepDataMapperTests : BaseTestFixture
    {
        /// <summary>
        /// Тестируемый маппер этапов
        /// </summary>
        private FileScenarioStepDataMapper _fileScenarioStepDataMapper;

        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private Mock<IDomainDbManager> _mockDb;
        
        /// <summary>
        /// Мок маппера документарных проверок
        /// </summary>
        private Mock<IDomainDataMapper<DocumentExpertise>> _mockExpertiseMapper;

        /// <summary>
        /// Мок маппера выездных проверок
        /// </summary>
        private Mock<IDomainDataMapper<Inspection>> _mockInspectionMapper;

        private IDictionaryManager _dictionaryManager;

        #endregion

       
        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {

            // мок менеджера базы
            _mockDb = new Mock<IDomainDbManager>();
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(DossierFileScenarioStep.CreateInstance());
            _mockDb.Setup(db => db.RetrieveDomainObject<Notice>(It.IsAny<object>())).Returns(Notice.CreateInstance());
            _mockDb.Setup(db => db.RetrieveDomainObject<ViolationResolveInfo>(It.IsAny<object>())).Returns(ViolationResolveInfo.CreateInstance);

            // мок маппера документарных проверок
            _mockExpertiseMapper = new Mock<IDomainDataMapper<DocumentExpertise>>();
            _mockExpertiseMapper.Setup(t => t.Retrieve(It.IsAny<object>(), _mockDb.Object))
                                .Returns(DocumentExpertise.CreateInstance());

            // мок маппера выездных проверок
            _mockInspectionMapper = new Mock<IDomainDataMapper<Inspection>>();
            _mockInspectionMapper.Setup(t => t.Retrieve(It.IsAny<object>(), _mockDb.Object))
                                .Returns(Inspection.CreateInstance());



            _dictionaryManager = Mock.Of<IDictionaryManager>();

            _fileScenarioStepDataMapper = CreateFileScenarioStepDataMapper();
        }

        private FileScenarioStepDataMapper CreateFileScenarioStepDataMapper()
        {
            var fileScenarioStepDataMapper =
                new FileScenarioStepDataMapper(
                    TestObjectMother.GetTestDomainContext(_mockDb.Object),
                    _mockExpertiseMapper.Object,
                    _mockInspectionMapper.Object,
                    Mock.Of<IDomainDataMapper<StandartOrder>>(),
                    Mock.Of<IDomainDataMapper<ExpertiseOrder>>(),
                    Mock.Of<IDomainDataMapper<InspectionOrder>>());
            TestObjectMother.SetDomainKey(fileScenarioStepDataMapper);
            return fileScenarioStepDataMapper;
        }


        /// <summary>
        /// Тест на получение ассоциации уведомление
        /// </summary>
        [Test]
        public void RetrieveNoticeTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1)); 
            _mockDb.Setup(db => db.GetDomainTable<Notice>())
                                        .Returns(new EnumerableQuery<Notice>(new List<Notice>
                                                                            {
                                                                                Mock.Of<Notice>(t => t.Id == 1)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(fileStep.Notice);
            _mockDb.Verify(t => t.RetrieveDomainObject<Notice>(It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Тест на получение null-ассоциации уведомление
        /// </summary>
        [Test]
        public void RetrieveNullNoticeTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<Notice>())
                                        .Returns(new EnumerableQuery<Notice>(new List<Notice>
                                                                            {
                                                                                Mock.Of<Notice>(t => t.Id == 33)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.Null(fileStep.Notice);
        }

        /// <summary>
        /// Тест на получение ассоциации документарная проверка
        /// </summary>
        [Test]
        public void RetrieveExpertiseTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<DocumentExpertise>())
                                        .Returns(new EnumerableQuery<DocumentExpertise>(new List<DocumentExpertise>
                                                                            {
                                                                                Mock.Of<DocumentExpertise>(t => t.Id == 1)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(fileStep.DocumentExpertise);
            _mockExpertiseMapper.Verify(t => t.Retrieve(It.IsAny<int>(), _mockDb.Object), Times.Once());
        }

        /// <summary>
        /// Тест на получение null-ассоциации документарная проверка
        /// </summary>
        [Test]
        public void RetrieveNullExpertiseTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<DocumentExpertise>())
                                        .Returns(new EnumerableQuery<DocumentExpertise>(new List<DocumentExpertise>
                                                                            {
                                                                                Mock.Of<DocumentExpertise>(t => t.Id == 33)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.Null(fileStep.DocumentExpertise);
        }
        
        /// <summary>
        /// Тест на получение ассоциации выездная проверка
        /// </summary>
        [Test]
        public void RetrieveInspectionTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<Inspection>())
                                        .Returns(new EnumerableQuery<Inspection>(new List<Inspection>
                                                                            {
                                                                                Mock.Of<Inspection>(t => t.Id == 1)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(fileStep.Inspection);
            _mockInspectionMapper.Verify(t => t.Retrieve(It.IsAny<int>(), _mockDb.Object), Times.Once());
        }

        /// <summary>
        /// Тест на получение null-ассоциации выездная проверка
        /// </summary>
        [Test]
        public void RetrieveNullInspectionTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<Inspection>())
                                        .Returns(new EnumerableQuery<Inspection>(new List<Inspection>
                                                                            {
                                                                                Mock.Of<Inspection>(t => t.Id == 33)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.Null(fileStep.Inspection);
        }


        /// <summary>
        /// Тест на получение ассоциации информация об устранении нарушений
        /// </summary>
        [Test]
        public void RetrieveViolationResolveInfoTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<ViolationResolveInfo>())
                                        .Returns(new EnumerableQuery<ViolationResolveInfo>(new List<ViolationResolveInfo>
                                                                            {
                                                                                Mock.Of<ViolationResolveInfo>(t => t.Id == 1)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.NotNull(fileStep.ViolationResolveInfo);
        }

        /// <summary>
        /// Тест на получение null-ассоциации информация об устранении нарушений
        /// </summary>
        [Test]
        public void RetrieveNullViolationResolveInfoTest()
        {
            // Arrange 
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            _mockDb.Setup(db => db.GetDomainTable<ViolationResolveInfo>())
                                        .Returns(new EnumerableQuery<ViolationResolveInfo>(new List<ViolationResolveInfo>
                                                                            {
                                                                                Mock.Of<ViolationResolveInfo>(t => t.Id == 33)
                                                                            }));

            // Act
            var fileStep = _fileScenarioStepDataMapper.Retrieve(0);

            // Assert
            Assert.Null(fileStep.ViolationResolveInfo);
        }

        /// <summary>
        /// Тесты на сохранение ассоциации уведомление
        /// </summary>
        [Test]
        public void SaveNoticeTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            var fielStep = DossierFileScenarioStep.CreateInstance();
            fielStep.Notice = Notice.CreateInstance();

            // Act
            var savedFileStep = _fileScenarioStepDataMapper.Save(fielStep, _mockDb.Object);

            // Assert
            Assert.False(savedFileStep.Notice.IsDirty);
            Assert.NotNull(savedFileStep.Notice);
            Assert.AreEqual(savedFileStep.Notice.Id, savedFileStep.Id);
        }

        /// <summary>
        /// Тесты на сохранение ассоциации документарная проверка
        /// </summary>
        [Test]
        public void SaveExpertiseTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            var fielStep = DossierFileScenarioStep.CreateInstance();
            fielStep.DocumentExpertise = DocumentExpertise.CreateInstance();

            _mockExpertiseMapper.Setup(t => t.Save(It.IsAny<DocumentExpertise>(), _mockDb.Object, false))
                                .Returns(DocumentExpertise.CreateInstance());

            _fileScenarioStepDataMapper = CreateFileScenarioStepDataMapper();


            // Act
            var savedFileStep = _fileScenarioStepDataMapper.Save(fielStep, _mockDb.Object);

            // Assert
            _mockExpertiseMapper.Verify(t => t.Save(It.IsAny<DocumentExpertise>(), _mockDb.Object, false), Times.Once()); 
            Assert.False(savedFileStep.DocumentExpertise.IsDirty);
            Assert.NotNull(savedFileStep.DocumentExpertise);
        }

        /// <summary>
        /// Тесты на сохранение ассоциации выездная проверка
        /// </summary>
        [Test]
        public void SaveInspectionTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            var fielStep = DossierFileScenarioStep.CreateInstance();
            fielStep.Inspection = Inspection.CreateInstance();

            _mockInspectionMapper.Setup(t => t.Save(It.IsAny<Inspection>(), _mockDb.Object, false))
                                 .Returns(Inspection.CreateInstance());

            _fileScenarioStepDataMapper = CreateFileScenarioStepDataMapper();


            // Act
            var savedFileStep = _fileScenarioStepDataMapper.Save(fielStep, _mockDb.Object);

            // Assert
            _mockInspectionMapper.Verify(t => t.Save(It.IsAny<Inspection>(), _mockDb.Object, false), Times.Once());
            Assert.False(savedFileStep.Inspection.IsDirty);
            Assert.NotNull(savedFileStep.Inspection);
        }
        
        /// <summary>
        /// Тесты на сохранение ассоциации информация об устранении нарушений
        /// </summary>
        [Test]
        public void SaveViolationResolveInfoTest()
        {
            // Arrange
            _mockDb.Setup(db => db.RetrieveDomainObject<DossierFileScenarioStep>(It.IsAny<object>())).Returns(Mock.Of<DossierFileScenarioStep>(t => t.Id == 1));
            var fielStep = DossierFileScenarioStep.CreateInstance();
            fielStep.ViolationResolveInfo = ViolationResolveInfo.CreateInstance();


            _fileScenarioStepDataMapper = CreateFileScenarioStepDataMapper();


            // Act
            var savedFileStep = _fileScenarioStepDataMapper.Save(fielStep, _mockDb.Object);

            // Assert
            Assert.False(savedFileStep.ViolationResolveInfo.IsDirty);
            Assert.NotNull(savedFileStep.ViolationResolveInfo);
            Assert.AreEqual(savedFileStep.ViolationResolveInfo.Id, savedFileStep.Id);
            _mockDb.Verify(t => t.SaveDomainObject<ViolationResolveInfo>(It.IsAny<ViolationResolveInfo>()));
        }
    }
}
