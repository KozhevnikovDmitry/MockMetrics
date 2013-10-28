using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;
using JetBrains.ReSharper.TestFramework;
using MockMetrics.Eating;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    [TestReferences(TEST_DATA + @"\nunit.framework.dll")]
    [TestReferences(PRODUCT_INSTALLATION + @"\MockMetrics.Fake.dll")]
    public class MockMetricsTestBase : CSharpHighlightingTestNet4Base
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
        {
            return highlighting is FakeHighlighting;
        }

        protected override string RelativeTestDataPath
        {
            get { return @"foo"; }
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

        [TestCase("FooTests.cs")]
        public void MockMetricsTest(string testName)
        {
            DoTestFiles(testName);
            var snapshot = FakesElementProcessor.Results.Single().Value;

            Assert.AreEqual(snapshot.Stubs.Count, 1);
            Assert.AreEqual(snapshot.Targets.Count, 1);
            Assert.AreEqual(snapshot.TargetCalls.Count, 1);
            Assert.AreEqual(snapshot.Asserts.Count, 1);
        }
    }
}
