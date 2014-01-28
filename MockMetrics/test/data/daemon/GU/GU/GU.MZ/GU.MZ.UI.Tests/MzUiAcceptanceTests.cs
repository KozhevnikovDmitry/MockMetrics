using System;
using Autofac;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using GU.MZ.BL.Tests;
using GU.MZ.BL.Tests.AcceptanceTest;
using GU.MZ.DataModel.Licensing;
using NUnit.Framework;

namespace GU.MZ.UI.Tests
{
    public class MzUiAcceptanceTests
    {
#if Integration
        protected bool IsTestTransactional = true;
#else
        protected bool IsTestTransactional = false;
#endif

        public MzUiTestRoot Root { get; set; }

        protected IDictionaryManager DictionaryManager;

        protected TestDbManager DbForTest;

        /// <summary>
        /// Фабрика классов бизнес-логики предметной обалсти "Лицензирование" 
        /// </summary>
        protected MzTestBlFactory MzLogicFactory;
        /// <summary>
        /// Фабрика классов бизнес-логики предметной обалсти "Лицензирование" 
        /// </summary>
        protected MzUiFactory MzUiFactory;

        /// <summary>
        /// Setup once before all test
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Root = new MzUiTestRoot(IsTestTransactional);
            Root.Register();
            var container = Root.Resolve();
            MzUiFactory = container.Resolve<MzUiFactory>();
            MzLogicFactory = container.Resolve<MzTestBlFactory>();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Root.Release();
        }

        [SetUp]
        public void Setup()
        {
            if (IsTestTransactional)
            {
                DbForTest = MzLogicFactory.IocContainer.Resolve<Func<IDomainDbManager>>()() as TestDbManager;
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (IsTestTransactional)
            {
                DbForTest.RollbackDomainTransaction();
                DbForTest.ReallyDispose();
            }
        }
    }
}
