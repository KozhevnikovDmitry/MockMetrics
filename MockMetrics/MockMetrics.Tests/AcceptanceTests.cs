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

        [TestCase(@"<PostGrad.BL.Tests>\AddInList")]
        public void PostGrad_AddInList_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            Console.WriteLine(Enumerable.ToArray(FakesElementProcessor.Results.Values).Count());
        }

        [TestCase(@"<PostGrad.BL.Tests>\StepByStep")]
        public void PostGrad_StepByStep_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            Console.WriteLine(Enumerable.ToArray(FakesElementProcessor.Results.Values).Count());
        }

        [TestCase(@"<PostGrad.BL.Tests>\InitializedObject")]
        public void PostGrad_InitializedObject_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            Console.WriteLine(Enumerable.ToArray(FakesElementProcessor.Results.Values).Count());
        }

        [TestCase(@"<PostGrad.BL.Tests>\DiActionContext")]
        public void PostGrad_DiActionContext_Tests(string testName)
        {
            DoMultipleTestFiles(testName);
            Console.WriteLine(Enumerable.ToArray(FakesElementProcessor.Results.Values).Count());
        }
    }
}
