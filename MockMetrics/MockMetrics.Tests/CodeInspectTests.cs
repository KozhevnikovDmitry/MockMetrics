using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using NUnit.Framework;

namespace MockMetrics.Tests
{
    public class CodeInspectTests : BaseFixture
    {
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
            get { return @"daemon\Tested"; }
        }

        protected override string SolutionName
        {
            get { return @"Tested.sln"; }
        }

        [Test]
        public void CodeInspectTest()
        {
            // Arrange
            
            // Act

            // Assert
        }
    }
}
