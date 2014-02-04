using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestFixture]
    public class IntegrationTests : BaseFixture
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
        /// Test#0
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\PrimitiveVariableTests.cs")]
        public void PrimitiveVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Librarians.Count, 4);
        }

        /// <summary>
        /// Test#0.1
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\EnumOccurencesTests.cs")]
        public void EnumOccurencesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#1
        /// </summary>
        [TestCase(@"<Tested.Tests>\FooTests\SimpleVariablesTests.cs")]
        public void SimpleVariablesTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 1, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Mocks services");
        }

        /// <summary>
        /// Test#2
        /// </summary>
        [TestCase(@"<Tested.Tests>\FooTests\ExpressionVariableTests.cs")]
        public void ExpressionVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Assert services");
        }

        /// <summary>
        /// Test#3
        /// </summary>
        [TestCase(@"<Tested.Tests>\FooTests\StubTests.cs")]
        public void StubTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 1, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 0, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Assert services");
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

            Assert.AreEqual(snapshot.Stubs.Count(), 3, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 3, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets"); 
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 2, "Assert services");
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
            Assert.AreEqual(snapshot.Librarians.Count(), 5, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 3, "Assert services");
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

            Assert.AreEqual(snapshot.Stubs.Count(), 4, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 2, "Assert services");
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

        }

        /// <summary>
        /// Test#8
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\MoqSyntaxTests.cs")]
        public void MoqSyntaxTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 12, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 26, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 2, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 21, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 12, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 9, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 2, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 2, "Assert fake exception");

        }

        /// <summary>
        /// Test#9
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\InternalVariableTests.cs")]
        public void InternalVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#10
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\InternalMultiDeclaredVariableTests.cs")]
        public void InternalMultiDeclaredVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }
        
        /// <summary>
        /// Test#11
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\InternalFieldVariableTests.cs")]
        public void InternalFieldVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 1, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#12
        /// </summary>
        [TestCase(@"<Tested.Tests>\AggregatorTests\LocalVariableFromInternalMethodTests.cs")]
        public void LocalVariableFromInternalMethodTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 1, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 1, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#13
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\EatingForeachVaribaleBeforeBodyTests.cs")]
        public void EatingForeachVaribaleBeforeBodyTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 3, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#14
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\EatingCatchVaribaleBeforeBodyTests.cs")]
        public void EatingCatchVaribaleBeforeBodyTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#15
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\DynamicArgumentForOverloadMethodTests.cs")]
        public void DynamicArgumentForOverloadMethodTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 2, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        /// <summary>
        /// Test#16
        /// </summary>
        [TestCase(@"<Tested.Tests>\SimpleTests\EatingForVariableTests.cs")]
        public void EatingForVariableTests(string testName)
        {
            DoTestFiles(testName);
            var snapshot = Enumerable.ToArray(FakesElementProcessor.Results.Values)[0];
            Console.WriteLine(snapshot);

            Assert.AreEqual(snapshot.Stubs.Count(), 0, "Assert stubs");
            Assert.AreEqual(snapshot.Librarians.Count(), 5, "Assert library");
            Assert.AreEqual(snapshot.Targets.Count(), 0, "Assert targets");
            Assert.AreEqual(snapshot.Mocks.Count(), 0, "Assert mocks");
            Assert.AreEqual(snapshot.Services.Count(), 0, "Assert services");

            Assert.AreEqual(snapshot.FakeProperties.Count(), 0, "Assert fake properties");
            Assert.AreEqual(snapshot.FakeMethods.Count(), 0, "Assert fake methods");
            Assert.AreEqual(snapshot.FakeCallbacks.Count(), 0, "Assert fake calbacks");
            Assert.AreEqual(snapshot.FakeExceptions.Count(), 0, "Assert fake exception");
        }

        
    }
}
