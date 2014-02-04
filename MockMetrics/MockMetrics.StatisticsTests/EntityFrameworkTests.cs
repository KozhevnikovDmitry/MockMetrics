using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Tests;
using NUnit.Framework;

namespace MockMetrics.StatisticsTests
{
    public class EntityFrameworkTests : BaseFixture
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
            get { return @"D:\GitHub\mm_test\EntityFramework"; }
        }

        protected override string SolutionName
        {
            get { return @"EntityFramework.sln"; }
        }

        [TestCase(@"<UnitTests>")]
        public void Entity_Framework_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "enitity_framework_tests");
        }
    }
}