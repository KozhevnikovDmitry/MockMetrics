using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class GuEatingTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting,
            IContextBoundSettingsStore settingsstore)
        {
            return highlighting is FakeHighlighting;
        }

        [SetUp]
        public void Setup()
        {
            FakesElementProcessor.Results.Clear();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            FakesElementProcessor.Results.Clear();
        }

        protected override string RelativeTestDataPath
        {
            get { return @"D:\GitHub\GU\GU\GU.MZ"; }
        }

        protected override string SolutionName
        {
            get { return @"GU.MZ.sln"; }
        }

        [TestCase(@"<GU.MZ.BL.Tests>\DataMapping\LicenseHolderDataMapperTests")]
        public void GU_MZ_BL_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "gu_mz_bl");
        }
    }
}
