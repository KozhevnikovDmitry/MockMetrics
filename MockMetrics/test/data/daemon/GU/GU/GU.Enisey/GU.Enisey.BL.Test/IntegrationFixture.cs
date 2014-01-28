using Common.DA.ProviderConfiguration;
using NUnit.Framework;

namespace GU.Enisey.BL.Test
{
    [TestFixture]
    public class IntegrationFixture
    {
        /// <summary>
        /// Инициализирует DA и BL
        /// </summary>
        private void InitializeEnisey()
        {
            var conf = new PostgreConfiguration("Postgre", "Server={0};Port={1};Database={2};User Id={3};Password={4};", "172.25.253.154", 5432, "gosus") { User = "test_guenisey", Password = "test" };
            EniseyFacade.Initialize(conf);
        }

        /// <summary>
        /// Setup once before all test
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            InitializeEnisey();
        }

        /// <summary>
        /// Teardown once after all tests
        /// </summary>
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            
        }

       
    }
}
