﻿using System;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using MockMetrics.Eating;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestFixture]
    public class SimpleMockMetricsTests : CSharpHighlightingWithinSolutionTestBase
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
    }

    //[TestReferences(TEST_DATA + @"\nunit.framework.dll")]
    //[TestReferences(PRODUCT_INSTALLATION + @"\MockMetrics.Fake.dll")]
    //public class SimpleMockMetricsTests : CSharpHighlightingTestNet4Base
    //{
    //    protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
    //    {
    //        return highlighting is FakeHighlighting;
    //    }

    //    protected override string RelativeTestDataPath
    //    {
    //        get { return @"foo"; }
    //    }

    //    [TestFixtureSetUpAttribute]
    //    public void FixtureSetup()
    //    {
    //        FakesElementProcessor.Results.Clear();
    //    }

    //    [SetUp]
    //    public void Setup()
    //    {
    //        FakesElementProcessor.Results.Clear();
    //    }

    //    [TearDown]
    //    public override void TearDown()
    //    {
    //        base.TearDown();
    //        FakesElementProcessor.Results.Clear();
    //    }

    //    [TestCase("SimpleTests.cs")]
    //    public void MockMetricsTest(string testName)
    //    {
    //        DoTestFiles(testName);

    //        Assert_SimpleVariablesTest(Enumerable.ToArray(FakesElementProcessor.Results.Values)[0]);
    //        Assert_ExpressionVariablesTest(Enumerable.ToArray(FakesElementProcessor.Results.Values)[1]);
    //    }

    //    private void Assert_SimpleVariablesTest(Snapshot snapshot)
    //    {
    //        Assert.AreEqual(snapshot.Stubs.Count, 1);
    //        Assert.AreEqual(snapshot.Targets.Count, 1);
    //        Assert.AreEqual(snapshot.TargetCalls.Count, 1);
    //        Assert.AreEqual(snapshot.Asserts.Count, 1);
    //    }

    //    private void Assert_ExpressionVariablesTest(Snapshot snapshot)
    //    {
    //        Assert.AreEqual(snapshot.Stubs.Count, 2);
    //        Assert.AreEqual(snapshot.Targets.Count, 1);
    //        Assert.AreEqual(snapshot.TargetCalls.Count, 1);
    //        Assert.AreEqual(snapshot.Asserts.Count, 1);
    //    }
    //}
}
