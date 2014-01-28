using System;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest
{
    /// <summary>
    /// Базовый класс для приёмочных фикстур MZ
    /// </summary>
    public class MzAcceptanceTests
    {

#if Integration
        protected bool IsTestTransactional = true;
#else
        protected bool IsTestTransactional = false;
#endif

        public ITestRoot Root { get; set; }

        protected IDictionaryManager DictionaryManager
        {
            get
            {
                return MzLogicFactory.GetDictionaryManager();
            }
        }

        /// <summary>
        /// Фабрика классов бизнес-логики предметной обалсти "Лицензирование" 
        /// </summary>
        protected MzTestBlFactory MzLogicFactory;

        /// <summary>
        /// Setup once before all test
        /// </summary>
        [TestFixtureSetUp]
        public virtual void FixtureSetup()
        {
            Root = new MzBlTestRoot(IsTestTransactional);
            Root.Register();
            MzLogicFactory = Root.Resolve();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Root.Release();
        }

        [SetUp]
        public virtual void Setup()
        {
            if (IsTestTransactional)
            {
                Root.BeginTransactionTest();
            }
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (IsTestTransactional)
            {
                Root.EndTransactionTest();
            }
        }
    }

    public class TestDbManager : IDomainDbManager
    {
        private IDomainDbManager _dbManager = new GumzDbManager();

        public void ChangeDbManager()
        {
            _dbManager.Dispose();
            _dbManager = new GumzDbManager();
        }

        public void ReallyDispose()
        {
            _dbManager.Dispose();
        }

        public void Dispose()
        {

        }

        public T RetrieveDomainObject<T>(object key) where T : IPersistentObject
        {
            return _dbManager.RetrieveDomainObject<T>(key);
        }

        public void SaveDomainObject<T>(T obj) where T : IPersistentObject
        {
            _dbManager.SaveDomainObject(obj);
        }

        public void SaveDomainObject<T>(T obj, bool forceSave) where T : IPersistentObject
        {
            _dbManager.SaveDomainObject(obj, forceSave);
        }

        public void BeginDomainTransaction()
        {
            _dbManager.BeginDomainTransaction();
        }

        public void RollbackDomainTransaction()
        {
            _dbManager.RollbackDomainTransaction();
        }

        public void CommitDomainTransaction()
        {
            _dbManager.CommitDomainTransaction();
        }

        public IQueryable<T> GetDomainTable<T>() where T : class, IDomainObject
        {
            return _dbManager.GetDomainTable<T>();
        }

        public IQueryable GetDomainTable(Type domainType)
        {
            return _dbManager.GetDomainTable(domainType);
        }
    }
}
