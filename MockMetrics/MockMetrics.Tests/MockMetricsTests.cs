using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
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

        /// <summary>
        /// Test#1
        /// </summary>
        [TestCase(@"<Tested.Tests>\Foo\SimpleVariablesTests.cs")]
        public void SimpleVariablesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 1, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 1, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }

        /// <summary>
        /// Test#2
        /// </summary>
        [TestCase(@"<Tested.Tests>\Foo\ExpressionVariableTests.cs")]
        public void ExpressionVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 1, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 1, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }

        /// <summary>
        /// Test#3
        /// </summary>
        [TestCase(@"<Tested.Tests>\Foo\StubTests.cs")]
        public void StubTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 1, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 1, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }

        /// <summary>
        /// Test#4
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\MoqStubTests.cs")]
        public void MoqStubTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 2, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 2, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 2, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 2, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }
        
        /// <summary>
        /// Test#5
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\MoqAssignmentStubsTests.cs")]
        public void MoqAssignmentStubsTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 2, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 2, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 2, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 2, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }

        /// <summary>
        /// Test#6
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\VariablesWithInitializersTests.cs")]
        public void VariablesWithInitializersTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 3, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 1, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 1, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }


        /// <summary>
        /// Test#7
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\NestedMoqStubsTests.cs")]
        public void NestedMoqStubsTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 4, "Assert stubs");
            Assert.AreEqual(snapshot.Results.Count(), 1, "Assert results");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.TargetCalls.Count(), 0, "Assert targetCalls");
            Assert.AreEqual(snapshot.Asserts.Count(), 2, "Assert asserts");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Mocks targets");
        }
    }
}
