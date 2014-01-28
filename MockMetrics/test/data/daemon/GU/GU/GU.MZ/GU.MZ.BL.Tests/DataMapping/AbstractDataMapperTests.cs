using System;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.MZ.BL.Test.DataMappingTest;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на класс AbstractDataMapper
    /// </summary>
    [TestFixture]
    public class AbstractDataMapperTests : BaseTestFixture
    {
        /// <summary>
        /// тестируемый маппер
        /// </summary>
        private MapperTestImplementation<IPersistentObject> _mapperTestImplementation;

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
            _mockDb = new Mock<IDomainDbManager>();
            _mapperTestImplementation = new MapperTestImplementation<IPersistentObject>(TestObjectMother.GetTestDomainContext(_mockDb.Object));
            TestObjectMother.SetDomainKey(_mapperTestImplementation);
        }

        /// <summary>
        /// Тест на получение чистого(не IsDisrty) объекта
        /// </summary>
        [Test]
        public void RetrieveNotDirtyObjectTest()
        {
            // Act
            var obj = _mapperTestImplementation.Retrieve(0);

            // Assert
            _mapperTestImplementation.Result.Verify(r => r.AcceptChanges(), Times.Once());
        }

        /// <summary>
        /// Тест на получение чистого (не IsDirty) сохранённого тома лицензионного дела
        /// </summary>
        [Test]
        public void GetNotDirtySavedDossierFileTest()
        {
            // Arrange
            var obj = Mock.Of<IPersistentObject>();

            // Act
            var savedFile = _mapperTestImplementation.Save(obj, _mockDb.Object);

            // Assert
            _mapperTestImplementation.Result.Verify(r => r.AcceptChanges(), Times.Once());
        }

        /// <summary>
        /// Тест на штатное управление транзакциями при сохранении
        /// </summary>
        [Test]
        public void BeginCommitTransactionPerSaveTest()
        {
            // Arrange
            var obj = Mock.Of<IPersistentObject>();

            // Act
            var savedFile = _mapperTestImplementation.Save(obj, _mockDb.Object);

            // Assert
            _mockDb.Verify(db => db.BeginDomainTransaction(), Times.Once());
            _mockDb.Verify(db => db.CommitDomainTransaction(), Times.Once());
        }

        /// <summary>
        /// Тест на откат транзакции в случае ошибки при сохранении тома
        /// </summary>
        [Test]
        public void RollbackTransactionperSaveTest()
        {
            // Arrange
            var obj = Mock.Of<IPersistentObject>();
            _mockDb.Setup(db => db.BeginDomainTransaction()).Throws<Exception>();

            // Assert
            Assert.Throws<BLLException>(() => _mapperTestImplementation.Save(obj, _mockDb.Object));
            _mockDb.Verify(db => db.BeginDomainTransaction(), Times.Once());
            _mockDb.Verify(db => db.CommitDomainTransaction(), Times.Never());
            _mockDb.Verify(db => db.RollbackDomainTransaction(), Times.Once());
        }

        /// <summary>
        /// Тест на обработку TransactionControlException 
        /// </summary>
        [Test]
        public void TransactionControlExceptionHandleTest()
        {
            // Arrange
            var obj = Mock.Of<IPersistentObject>();
            _mockDb.Setup(db => db.CommitDomainTransaction()).Throws<TransactionControlException>();

            // Assert
            Assert.Throws<BLLException>(() => _mapperTestImplementation.Save(obj, _mockDb.Object));
            _mockDb.Verify(db => db.BeginDomainTransaction(), Times.Once());
            _mockDb.Verify(db => db.CommitDomainTransaction(), Times.Once());
            _mockDb.Verify(db => db.RollbackDomainTransaction(), Times.Never());
        }
    }
}
