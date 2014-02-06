using System;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class StatisticsTests : BaseFixture
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

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\Before", Ignore = true)]
        public void AddInList_Before_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "add_in_list_before");
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After", Ignore = true)]
        public void AddInList_After_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "add_in_list_after");
        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\Before", Ignore = true)]
        public void DiActionContext_Before_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "di_action_context_before");
        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext\After", Ignore = true)]
        public void DiActionContext_After_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "di_action_context_after");
        }

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject\Before", Ignore = true)]
        public void InitializedObject_Before_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "initialized_object_before");
        }

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject\After", Ignore = true)]
        public void InitializedObject_After_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "initialized_object_after");
        }

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep\Before", Ignore = true)]
        public void StepByStep_Before_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "step_by_step_before");
        }

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep\After", Ignore = true)]
        public void StepByStep_After_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = MockMetricsElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count);
            new SnapshotDump().DumpAssert(snapshots, "step_by_step_after");
        }
    }
}