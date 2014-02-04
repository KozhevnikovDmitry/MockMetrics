using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Tests;
using NUnit.Framework;

namespace MockMetrics.StatisticsTests
{
    public class AutofacEatingTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting,
            IContextBoundSettingsStore settingsstore)
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
            get { return @"D:\GitHub\mm_test\Autofac"; }
        }

        protected override string SolutionName
        {
            get { return @"Autofac.sln"; }
        }

        [TestCase(@"<Autofac.Tests")]
        [TestCase(@"<Autofac.Extras.Tests")]
        public void Autofac_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "autofac_tests");
        }
    }
}
