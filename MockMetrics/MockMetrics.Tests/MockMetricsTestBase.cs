using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp;
using MockMetrics.Fake;
using NUnit.Framework;

namespace MockMetrics.Tests
{
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
            FakesElementProcessor.UnitTestDeclarations.Clear();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            FakesElementProcessor.UnitTestDeclarations.Clear();
        }

        [Test]
        [TestCase("FooTests.cs")]
        public void MockMetricsTest(string testName)
        {
            DoTestFiles(testName);
            var references = GetReferencedAssemblies();

            foreach (var unitTestDeclaration in FakesElementProcessor.UnitTestDeclarations)
            {
                var unitTestProcessor = new UnitTestProcessor();
                var snapshot = unitTestProcessor.EatUnitTest(unitTestDeclaration);
            }
        }
    }
}
