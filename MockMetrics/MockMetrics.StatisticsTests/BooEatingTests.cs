using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Tests;
using NUnit.Framework;

namespace MockMetrics.StatisticsTests
{
    public class BooEatingTests : BaseFixture
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
            get { return @"D:\GitHub\mm_test\boo\src"; }
        }

        protected override string SolutionName
        {
            get { return @"Boo-VS2010.sln"; }
        }

        [TestCase(@"<Boo.Lang.Tests>")]
        public void Boo_Lang_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "0000_boo_lang");
        }

        [TestCase(@"<Boo.Lang.Parser.Tests>")]
        public void Boo_Lang_Parser_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "0000_boo_lang_parser");
        }

        [TestCase(@"<Boo.Lang.Runtime.Tests>")]
        public void Boo_Lang_Runtime_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "0000_boo_lang_runtime");
        }

        [TestCase(@"<BooCompiler.Tests>")]
        public void BooCompiler_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().Dump(snapshots, "0000_boocomplier");
        }
    }
}
