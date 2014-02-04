using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Tests;
using NUnit.Framework;

namespace MockMetrics.StatisticsTests
{
    public class NhibernateTests : BaseFixture
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
            get { return @"D:\GitHub\mm_test\Nhibernate\src"; }
        }

        protected override string SolutionName
        {
            get { return @"NHibernate.sln"; }
        }

        [TestCase(@"<NHibernate.Test>")]
        public void Nhibernate_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "nhibernate_tests");
        }
    }
}