using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class AcceptanceTests : BaseFixture
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
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
            get { return @"daemon\PostGrad"; }
        }

        protected override string SolutionName
        {
            get { return @"PostGrad.sln"; }
        }

        [TestCase(@"<PostGrad.BL.Tests>\AddInList\After\LinkagerTests.cs")]
        public void PostGrad_AddInList_After_LinakgerTests_Tests(string testName)
        {
            DoTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "add_in_list");
        }


        [TestCase(@"<PostGrad.BL.Tests>\AddInList")]
        public void PostGrad_AddInList_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "add_in_list");
        }

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep")]
        public void PostGrad_StepByStep_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "step_by_step");
        }

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject")]
        public void PostGrad_InitializedObject_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "initialized_object");
        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext")]
        public void PostGrad_DiActionContext_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            var snapshots = FakesElementProcessor.Results.Values;
            Console.WriteLine(snapshots.Count());
            new SnapshotDump().Dump(snapshots, "di_action_context");
        }
    }
}
