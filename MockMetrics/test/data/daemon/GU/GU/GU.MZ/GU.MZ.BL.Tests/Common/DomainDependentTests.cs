using System;
using Common.BL.DomainContext;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Common
{
    [TestFixture]
    public class DomainDependentTests
    {
        [Test]
        public void CorrectTransactionPerAccept()
        {
            // Arrange
            var db = new Mock<IDomainDbManager>();
            var tdd = new TestDomainDependent(new StubDbCtx(db.Object));

            // Act
            tdd.TransactionWrapper(manager => { });

            // Assert
            db.Verify(t => t.BeginDomainTransaction(), Times.Once());
            db.Verify(t => t.CommitDomainTransaction(), Times.Once());
        }

        [Test]
        public void FailedWithExceptionTransactionPerAccept()
        {
            // Arrange
            var exception = new Exception();
            var db = new Mock<IDomainDbManager>();
            var tdd = new TestDomainDependent(new StubDbCtx(db.Object));

            // Act
            try
            {
                tdd.TransactionWrapper(manager => { throw exception; });
            }
            catch (BLLException ex)
            {
                Assert.AreEqual(ex.InnerException, exception);
            }

            // Assert
            db.Verify(t => t.BeginDomainTransaction(), Times.Once());
            db.Verify(t => t.RollbackDomainTransaction(), Times.Once());
        }

        [Test]
        public void FailedWithBllExceptionTransactionPerAccept()
        {
            // Arrange
            var exception = new TransactionControlException();
            var db = new Mock<IDomainDbManager>();
            var tdd = new TestDomainDependent(new StubDbCtx(db.Object));
            
            // Act
            try
            {
                tdd.TransactionWrapper(manager => { throw exception; });
            }
            catch (BLLException ex)
            {
                Assert.AreEqual(ex.InnerException, exception);
            }

            // Assert
            db.Verify(t => t.BeginDomainTransaction(), Times.Once());
            db.Verify(t => t.RollbackDomainTransaction(), Times.Never());
        }

        [Test]
        public void FailedWithTransactionControlExceptionTransactionPerAccept()
        {
            // Arrange
            var exception = new BLLException();
            var db = new Mock<IDomainDbManager>();
            var tdd = new TestDomainDependent(new StubDbCtx(db.Object));
            
            // Act
            try
            {
                tdd.TransactionWrapper(manager => { throw exception; });
            }
            catch (BLLException ex)
            {
                Assert.AreEqual(ex, exception);
            }

            // Assert
            db.Verify(t => t.BeginDomainTransaction(), Times.Once());
            db.Verify(t => t.RollbackDomainTransaction(), Times.Never());
        }
    }

    public class TestDomainDependent : DomainDependent
    {
        public TestDomainDependent(IDomainContext domainContext)
            : base(domainContext)
        {

        }

        public new IDomainDbManager GetDbManager()
        {
            return base.GetDbManager();
        }

        /// <summary>
        /// Обёртка для выполнения действия внутри транзакции 
        /// </summary>
        /// <param name="action">Дейсвтие, включающее запись в БД</param>
        public new void TransactionWrapper(Action<IDomainDbManager> action)
        {
            base.TransactionWrapper(action);
        }
    }
}