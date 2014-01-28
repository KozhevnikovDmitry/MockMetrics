using System;

using NUnit.Framework;

namespace SpecManager.BL.Test.Integration
{
    [TestFixture]
    public class IntegrationFixture
    {
        protected TestCompositionRoot CompositionRoot;

        /// <summary>
        /// Setup once before all test
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this.CompositionRoot = new TestCompositionRoot();
        }

        /// <summary>
        /// Teardown once after all tests
        /// </summary>
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            this.CompositionRoot.Container.Dispose();
        }

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.CompositionRoot.SetTransaction();
            Console.WriteLine("Начата транзакция теста");
        }

        /// <summary>
        /// Teardown after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Откатываем транзакцию теста");
            this.CompositionRoot.RollbackTransaction();
            Console.WriteLine("Откачена транзакция теста");
        }
    }
}
