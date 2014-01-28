using System;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using Moq;

namespace GU.MZ.BL.Tests
{
    /// <summary>
    /// Класс, порождающий тестовые объекты.
    /// </summary>
    public class TestObjectMother
    {
        #region Data Access objects

        private readonly string _domainKey = "testDomainKey";

        /// <summary>
        /// Возвращает тестовый доменный контекст
        /// </summary>
        /// <returns>Тестовый доменный контекст</returns>
        public IDomainContext GetTestDomainContext(IDomainDbManager db)
        {
            return Mock.Of<IDomainContext>(dc => dc.GetDbManager(_domainKey) == db);
        }

        /// <summary>
        /// Устанавливает значение доменного ключа для доменно-зависимого объекта.
        /// </summary>
        /// <param name="domainDependent">Доменно-зависимый объект</param>
        public void SetDomainKey(IDomainDependent domainDependent)
        {
            domainDependent.DomainKey = _domainKey;
        }

        public Mock<IDomainDbManager> GetStrictMockDbManager()
        {
            var mockDb = new Mock<IDomainDbManager>(MockBehavior.Strict);
            mockDb.Setup(db => db.BeginDomainTransaction());
            mockDb.Setup(db => db.CommitDomainTransaction());
            mockDb.Setup(db => db.RollbackDomainTransaction());
            mockDb.Setup(db => db.Dispose());
            return mockDb;
        }

        #endregion
    }
}
