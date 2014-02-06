using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class AcceptanceTests : BaseFixture
    {
        #region Test Environment

        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
        {
            return highlighting is MockMetricInfo;
        }

        [SetUp]
        public void Setup()
        {
            MockMetricsElementProcessor.Results.Clear();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            MockMetricsElementProcessor.Results.Clear();
        }

        protected override string RelativeTestDataPath
        {
            get { return @"daemon\PostGrad"; }
        }

        protected override string SolutionName
        {
            get { return @"PostGrad.sln"; }
        }

        #endregion

        #region AddInList Before

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\Before\LinkagerTests.cs")]
        public void AddInList_Before_LinkagerTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();
            
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 9, 11, 0, 1, 7, 5, 4, 0, 0 }), "LinkageExistingHolderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 4, 13, 0, 1, 6, 3, 4, 0, 0 }), "LinkageNewHolderTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 4, 13, 0, 1, 5, 4, 4, 0, 0 }), "NoExistingHolderForLightSceanrio");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 3, 15, 0, 1, 7, 5, 4, 0, 0 }), "SetupAvailableRequisitesForNewHolder");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 7, 17, 0, 1, 9, 8, 5, 0, 0 }), "SetupAvailableRequisitesForFullScenario");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 6, 16, 0, 1, 8, 8, 4, 0, 0 }), "SetupAvailableRequisitesForLightScenario");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 3, 14, 1, 1, 9, 6, 6, 0, 0 }), "AlwaysGetTheSameAvailableRequisistes");
            Assert.That(snapshots[7].Metrics().SequenceEqual(new[] { 8, 17, 0, 1, 8, 6, 6, 0, 0 }), "LinkageExistingDossierTest");
            Assert.That(snapshots[8].Metrics().SequenceEqual(new[] { 12, 18, 0, 1, 8, 8, 5, 0, 0 }), "LinkageNewDossierTest");
            Assert.That(snapshots[9].Metrics().SequenceEqual(new[] { 8, 18, 0, 1, 7, 9, 5, 0, 0 }), "NoExistingDossierForLightSceanrio");
            Assert.That(snapshots[10].Metrics().SequenceEqual(new[] { 4, 16, 0, 1, 6, 5, 4, 0, 0 }), "NoLinkageForNewLicenseTaskTest");
            Assert.That(snapshots[11].Metrics().SequenceEqual(new[] { 11, 24, 0, 1, 12, 15, 7, 0, 0 }), "LinkageLicenseTest");
            Assert.That(snapshots[12].Metrics().SequenceEqual(new[] { 10, 23, 0, 1, 11, 13, 7, 0, 0 }), "LinkageNoLicenseTest");
            Assert.That(snapshots[13].Metrics().SequenceEqual(new[] { 9, 21, 0, 1, 9, 13, 6, 0, 0 }), "LinkageLicenseTooMoreLicensesTest");
            Assert.That(snapshots[14].Metrics().SequenceEqual(new[] { 6, 13, 0, 1, 6, 8, 3, 0, 0 }), "GetIsHolderDataDoubtfullTest");
            Assert.That(snapshots[15].Metrics().SequenceEqual(new[] { 5, 18, 0, 1, 6, 4, 6, 0, 0 }), "SetupDossierFileTest");
        }

        #endregion

        #region AddInList After

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\CheckHolderdataAddInTests.cs")]
        public void PostGrad_AddInList_After_CheckHolderdataAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 4, 7, 0, 1, 5, 7, 1, 0, 0 }), "GetIsHolderDataDoubtfullTest");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkageDossierAddInTests.cs")]
        public void PostGrad_AddInList_After_LinkageDossierAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 6, 7, 0, 1, 5, 5, 2, 0, 0 }), "LinkageExistingDossierTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 10, 8, 0, 1, 5, 7, 1, 0, 0 }), "LinkageNewDossierTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 6, 8, 0, 1, 5, 8, 1, 0, 0 }), "NoExistingDossierForLightSceanrio");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkageHolderAddInTests.cs")]
        public void PostGrad_AddInList_After_LinkageHolderAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 2, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 7, 9, 0, 1, 6, 5, 3, 0, 0 }), "LinkageExistingHolderTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 1, 10, 0, 1, 4, 2, 3, 0, 0 }), "LinkageNewHolderTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 2, 10, 0, 1, 4, 3, 3, 0, 0 }), "NoExistingHolderForLightSceanrio");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkageLicenseAddInTests.cs")]
        public void PostGrad_AddInList_After_LinkageLicenseAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 0, 4, 0, 1, 2, 2, 0, 0, 0 }), "NoLinkageForNewLicenseTaskTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 1, 4, 0, 1, 2, 2, 0, 0, 0 }), "NoDossierLinkagedTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 7, 10, 0, 1, 8, 13, 1, 0, 0 }), "LinkageLicenseTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 6, 9, 0, 1, 7, 11, 1, 0, 0 }), "LinkageNoLicenseTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 5, 9, 0, 1, 7, 11, 1, 0, 0 }), "LinkageLicenseToMoreLicensesTest");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkageRequisitesAddInTests.cs")]
        public void PostGrad_AddInList_After_LinkageRequisitesAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 2, 7, 0, 1, 4, 5, 0, 0, 0 }), "SetupAvailableRequisitesForNewHolder");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 6, 9, 0, 1, 7, 8, 1, 0, 0 }), "SetupAvailableRequisitesForFullScenario");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 5, 8, 0, 1, 5, 8, 0, 0, 0 }), "SetupAvailableRequisitesForLightScenario");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 2, 6, 1, 1, 6, 6, 2, 0, 0 }), "AlwaysGetTheSameAvailableRequisistes");

        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkagerTests.cs")]
        public void PostGrad_AddInList_After_LinkagerTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 2, 3, 1, 1, 3, 0, 2, 0, 0 }), "LinkageTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 8, 4, 2, 1, 4, 2, 2, 2, 0 }), "LinkageBySortedAddinsTest");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\SetupFileAddInTests.cs")]
        public void PostGrad_AddInList_After_SetupFileAddInTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 0, 0, 0, 0, 0 }), "SortOrderTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 2, 3, 0, 0, 0 }), "NoLinkageForNewLicenseTaskTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 3, 5, 0, 1, 3, 3, 1, 0, 0 }), "SetupDossierFileTest");
        }

        #endregion

        #region DiActionContext Before

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\Before\ServiceResultGranterTests.cs")]
        public void DiActionContext_Before_ServiceResultGranterTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray(); 
            
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 6, 7, 0, 1, 4, 4, 1, 0, 0 }), "GrandLightScenarioServiceResultTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 5, 7, 0, 1, 3, 4, 1, 0, 0 }), "GrantLigntScenarioResultNotSingleResulstOnLightScenarioTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 9, 8, 0, 1, 5, 9, 1, 0, 0 }), "GrandFullScenarioPositiveServiceResultTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 10, 8, 0, 1, 5, 9, 1, 0, 0 }), "GrandFullScenarioNegativeServiceResultTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 4, 7, 0, 1, 3, 5, 1, 0, 0 }), "GrantFullScenarioResultNotSingleResulstOnLightScenarioTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 3, 8, 1, 1, 5, 2, 3, 0, 0 }), "SavePositiveServiceResultTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 3, 10, 1, 1, 5, 1, 3, 0, 0 }), "SaveNegativeServiceResultTest");
            Assert.That(snapshots[7].Metrics().SequenceEqual(new[] { 0, 7, 0, 1, 3, 1, 1, 0, 0 }), "GrantNewLicenseTest");
            Assert.That(snapshots[8].Metrics().SequenceEqual(new[] { 1, 6, 0, 1, 1, 1, 0, 0, 0 }), "GrantNewLicenseWrongStatusForGrantingTest");
            Assert.That(snapshots[9].Metrics().SequenceEqual(new[] { 0, 7, 0, 1, 3, 1, 1, 0, 0 }), "GrantStopLicenseTest");
            Assert.That(snapshots[10].Metrics().SequenceEqual(new[] { 1, 6, 0, 1, 1, 1, 0, 0, 0 }), "GrantStopLicenseWrongStatusForGrantingTest");
            Assert.That(snapshots[11].Metrics().SequenceEqual(new[] { 0, 7, 0, 1, 3, 1, 1, 0, 0 }), "GrantRenewalLicenseTest");
            Assert.That(snapshots[12].Metrics().SequenceEqual(new[] { 1, 6, 0, 1, 1, 1, 0, 0, 0 }), "GrantRenewalLicenseWrongStatusForGrantingTest");


        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\Before\SupervisionFacadeTests.cs")]
        public void DiActionContext_Before_SupervisionFacadeTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 2, 0, 1, 0, 0 }), "GrantServiseResultTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 1, 0, 1, 0, 0 }), "SaveServiceResultTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 2, 0, 1, 0, 0 }), "GrantNewLicenseTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 2, 0, 1, 0, 0 }), "GrantRenewalLicenseTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 2, 0, 1, 0, 0 }), "GrantStopLicenseTest");
        }

        #endregion

        #region DiActionContext After

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\After\ServiceResultGranterTests.cs")]
        public void DiActionContext_After_ServiceResultGranterTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 6, 3, 0, 1, 4, 4, 1, 0, 0 }), "GrandLightScenarioServiceResultTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 5, 3, 0, 1, 3, 4, 1, 0, 0 }), "GrantLigntScenarioResultNotSingleResulstOnLightScenarioTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 9, 4, 0, 1, 5, 9, 1, 0, 0 }), "GrandFullScenarioPositiveServiceResultTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 10, 4, 0, 1, 5, 9, 1, 0, 0 }), "GrandFullScenarioNegativeServiceResultTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 4, 3, 0, 1, 3, 5, 1, 0, 0 }), "GrantFullScenarioResultNotSingleResulstOnLightScenarioTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 3, 5, 1, 1, 5, 2, 3, 0, 0 }), "SavePositiveServiceResultTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 3, 7, 1, 1, 5, 1, 3, 0, 0 }), "SaveNegativeServiceResultTest");
            Assert.That(snapshots[7].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 3, 1, 1, 0, 0 }), "GrantNewLicenseTest");
            Assert.That(snapshots[8].Metrics().SequenceEqual(new[] { 1, 2, 0, 1, 1, 1, 0, 0, 0 }), "GrantNewLicenseWrongStatusForGrantingTest");
            Assert.That(snapshots[9].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 3, 1, 1, 0, 0 }), "GrantStopLicenseTest");
            Assert.That(snapshots[10].Metrics().SequenceEqual(new[] { 1, 2, 0, 1, 1, 1, 0, 0, 0 }), "GrantStopLicenseWrongStatusForGrantingTest");
            Assert.That(snapshots[11].Metrics().SequenceEqual(new[] { 0, 3, 0, 1, 3, 1, 1, 0, 0 }), "GrantRenewalLicenseTest");
            Assert.That(snapshots[12].Metrics().SequenceEqual(new[] { 1, 2, 0, 1, 1, 1, 0, 0, 0 }), "GrantRenewalLicenseWrongStatusForGrantingTest");
        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\After\SupervisionFacadeTests.cs")]
        public void DiActionContext_After_SupervisionFacadeTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 2, 0, 1, 0, 0 }), "GrantServiseResultTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 1, 0, 1, 0, 0 }), "SaveServiceResultTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 2, 0, 1, 0, 0 }), "GrantNewLicenseTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 2, 0, 1, 0, 0 }), "GrantRenewalLicenseTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 2, 0, 1, 0, 0 }), "GrantStopLicenseTest");
        }

        #endregion

        #region InitializedObject Before

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject\Before\CacheRepositoryTests.cs")]
        public void InitializedObject_Before_CacheRepositoryTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray(); 
            
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 3, 4, 0, 1, 1, 0, 3, 0, 0 }), "CtorTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 5, 1, 0, 1, 2, 0, 3, 0, 0 }), "GetCasheTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 4, 3, 2, 2, 4, 0, 5, 0, 1 }), "GetCacheFromMergedRepositoryTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 3, 2, 2, 2, 2, 0, 4, 0, 1 }), "DictionaryNotFoundExceptionTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 7, 3, 0, 1, 3, 0, 4, 0, 0 }), "GetCashedItemTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 6, 2, 0, 1, 1, 0, 3, 0, 0 }), "DictionaryItemNotFoundTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 3, 2, 0, 1, 1, 0, 3, 0, 0 }), "MergeTest");

        }

        #endregion

        #region InitializedObject After

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject\After\CacheRepositoryTests.cs")]
        public void InitializedObject_After_CacheRepositoryTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 3, 4, 0, 1, 1, 0, 3, 0, 0 }), "InitializeTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 2, 0, 0, 1, 1, 0, 0, 0, 0 }), "GetCasheTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 1, 2, 2, 2, 3, 0, 2, 0, 1 }), "GetCacheFromMergedRepositoryTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 0, 1, 2, 2, 1, 0, 1, 0, 1 }), "DictionaryNotFoundExceptionTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 4, 2, 0, 1, 2, 0, 1, 0, 0 }), "GetCashedItemTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 3, 1, 0, 1, 0, 0, 0, 0, 0 }), "DictionaryItemNotFoundTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 0, 1, 0, 1, 0, 0, 0, 0, 0 }), "MergeTest");
        }

        #endregion

        #region StepByStep Before

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep\Before\DossierFileBuilderTests.cs")]
        public void StepByStep_Before_DossierFileBuilderTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray(); 
            
            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 10, 13, 0, 1, 8, 6, 4, 0, 0 }), "FromTaskTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 9, 13, 0, 1, 7, 5, 4, 0, 0 }), "WithInventoryRegNumberTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 8, 13, 0, 1, 7, 5, 4, 0, 0 }), "CreateDateIsTodayTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 10, 13, 0, 1, 8, 6, 4, 0, 0 }), "ToEmployeeTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 13, 12, 1, 1, 7, 5, 5, 0, 0 }), "WithAcceptedStatusTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 13, 12, 1, 1, 7, 5, 5, 0, 0 }), "WithRejectedStatusTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 16, 13, 0, 1, 9, 7, 4, 0, 0 }), "AddProvidedDocumentTest");
            Assert.That(snapshots[7].Metrics().SequenceEqual(new[] { 9, 5, 0, 1, 1, 1, 0, 0, 0 }), "CantAddProvidedDocumentWithWrongDataTest");
            Assert.That(snapshots[8].Metrics().SequenceEqual(new[] { 3, 7, 0, 1, 1, 0, 1, 0, 0 }), "CantSetStatusTest");
            Assert.That(snapshots[9].Metrics().SequenceEqual(new[] { 4, 6, 0, 1, 1, 0, 1, 0, 0 }), "IncompleteDataWithNullTaskTest");
            Assert.That(snapshots[10].Metrics().SequenceEqual(new[] { 4, 6, 0, 1, 1, 0, 1, 0, 0 }), "IncompleteDataWithNullEmployeeTest");
            Assert.That(snapshots[11].Metrics().SequenceEqual(new[] { 3, 7, 0, 1, 1, 0, 1, 0, 0 }), "IncompleteDataWithNullInventoryRegNumberTest");
            Assert.That(snapshots[12].Metrics().SequenceEqual(new[] { 3, 7, 0, 1, 1, 0, 1, 0, 0 }), "IncompleteDataWithNullNoticeForRejectedStatusTest");
            Assert.That(snapshots[13].Metrics().SequenceEqual(new[] { 11, 13, 0, 1, 8, 7, 4, 0, 0 }), "CompleteDataForAcceptedTest");
            Assert.That(snapshots[14].Metrics().SequenceEqual(new[] { 11, 13, 0, 1, 8, 7, 4, 0, 0 }), "CompleteDataForRejectedStatusTest");
            Assert.That(snapshots[15].Metrics().SequenceEqual(new[] { 11, 13, 0, 1, 9, 7, 4, 0, 0 }), "BuildFileCreateDateIsTodayTest");
            Assert.That(snapshots[16].Metrics().SequenceEqual(new[] { 12, 13, 0, 1, 9, 7, 4, 0, 0 }), "BuildFileCurrentStatusIsUnboundTest");
            Assert.That(snapshots[17].Metrics().SequenceEqual(new[] { 11, 13, 0, 1, 9, 7, 4, 0, 0 }), "BuildFileSetupLicensedActivityTest");
            Assert.That(snapshots[18].Metrics().SequenceEqual(new[] { 13, 13, 0, 1, 9, 7, 4, 0, 0 }), "BuildFileSetupScenarioTest");
            Assert.That(snapshots[19].Metrics().SequenceEqual(new[] { 17, 13, 0, 1, 9, 8, 4, 0, 0 }), "BuildFileAddFisrtStepsTest");
            Assert.That(snapshots[20].Metrics().SequenceEqual(new[] { 19, 11, 0, 1, 10, 10, 4, 0, 0 }), "BuildFileSetupInventoryDataTest");

        }

        #endregion

        #region StepByStep After

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep\After\DossierFileBuildingFacadeTests.cs")]
        public void StepByStep_After_DossierFileBuilderTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 1, 5, 0, 1, 1, 1, 0, 0, 0 }), "FromTaskTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 2, 4, 0, 1, 0, 0, 0, 0, 0 }), "WithInventoryRegNumberTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 0, 5, 0, 1, 0, 0, 0, 0, 0 }), "ToEmployeeTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 3, 4, 0, 1, 0, 0, 0, 0, 0 }), "WithAcceptedStatusTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 3, 4, 0, 1, 0, 0, 0, 0, 0 }), "WithRejectedStatusTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 5, 4, 0, 1, 0, 0, 0, 0, 0 }), "AddProvidedDocumentTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 6, 4, 0, 1, 0, 0, 0, 0, 0 }), "CantAddProvidedDocumentWithWrongDataTest");

            
        }

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep\After\BuildingFileStepsTests.cs")]
        public void StepByStep_After_BuildingFileStepsTests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values.ToArray();

            Assert.That(snapshots[0].Metrics().SequenceEqual(new[] { 2, 2, 0, 1, 2, 1, 0, 0, 0 }), "PrepareDossierFileTest");
            Assert.That(snapshots[1].Metrics().SequenceEqual(new[] { 8, 2, 1, 1, 3, 1, 2, 0, 0 }), "SetupStatusTest");
            Assert.That(snapshots[2].Metrics().SequenceEqual(new[] { 2, 4, 0, 1, 1, 0, 1, 0, 0 }), "CantSetStatusTest");
            Assert.That(snapshots[3].Metrics().SequenceEqual(new[] { 0, 0, 0, 1, 1, 0, 0, 0, 0 }), "IncompleteDataWithNullTaskTest");
            Assert.That(snapshots[4].Metrics().SequenceEqual(new[] { 0, 1, 0, 1, 1, 0, 0, 0, 0 }), "IncompleteDataWithNullTaskStatusTest");
            Assert.That(snapshots[5].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 1, 0, 0, 0, 0 }), "IncompleteDataWithNullEmployeeTest");
            Assert.That(snapshots[6].Metrics().SequenceEqual(new[] { 1, 2, 0, 1, 1, 0, 0, 0, 0 }), "IncompleteDataWithNullInventoryRegNumberTest");
            Assert.That(snapshots[7].Metrics().SequenceEqual(new[] { 1, 1, 0, 1, 1, 0, 0, 0, 0 }), "IncompleteDataWithNullNoticeForRejectedStatusTest");
            Assert.That(snapshots[8].Metrics().SequenceEqual(new[] { 2, 2, 0, 1, 1, 0, 0, 0, 0 }), "CompleteDataForAcceptedTest");
            Assert.That(snapshots[9].Metrics().SequenceEqual(new[] { 2, 1, 0, 1, 1, 0, 0, 0, 0 }), "CompleteDataForRejectedStatusTest");
            Assert.That(snapshots[10].Metrics().SequenceEqual(new[] { 3, 3, 0, 1, 3, 2, 1, 0, 0 }), "SetupActivityTest");
            Assert.That(snapshots[11].Metrics().SequenceEqual(new[] { 2, 3, 0, 1, 1, 1, 0, 0, 0 }), "SetupEmployeeTest");
            Assert.That(snapshots[12].Metrics().SequenceEqual(new[] { 10, 7, 0, 1, 5, 7, 1, 0, 0 }), "SetupScenarioTest");
            Assert.That(snapshots[13].Metrics().SequenceEqual(new[] { 11, 4, 0, 1, 4, 7, 0, 0, 0 }), "BuildFileAddFisrtStepsTest");
            Assert.That(snapshots[14].Metrics().SequenceEqual(new[] { 11, 6, 0, 1, 4, 6, 1, 0, 0 }), "BuildFileSetupInventoryDataTest");
        }

        #endregion
    }
}
