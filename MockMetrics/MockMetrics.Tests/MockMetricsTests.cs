using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Eating;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestFixture]
    public class MockMetricsTests : CSharpHighlightingWithinSolutionTestBase
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
            get { return @"daemon\Tested"; }
        }

        protected override string SolutionName
        {
            get { return @"Tested.sln"; }
        }

        [Test]
        [TestCase(@"<Tested.Tests>\SimpleVariablesTests.cs")]
        public void SimpleVariablesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count, 1, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count, 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count, 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count, 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count, 1, "Assert asserts");
        }

        [Test]
        [TestCase(@"<Tested.Tests>\ExpressionVariableTests.cs")]
        public void ExpressionVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count, 2, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count, 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count, 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count, 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count, 1, "Assert asserts");
        }

        [Test]
        [TestCase(@"<Tested.Tests>\StubTests.cs")]
        public void StubTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count, 1, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count, 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count, 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count, 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count, 1, "Assert asserts");
        }
    }
}
