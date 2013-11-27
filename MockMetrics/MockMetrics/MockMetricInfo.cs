using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using MockMetrics;
using MockMetrics.Eating;
using MockMetrics.Eating.MetricMeasure;

[assembly: RegisterConfigurableSeverity(MockMetricInfo.SeverityId,
  null,
  HighlightingGroupIds.CodeSmell,
  "MockMetricInfo",
  "MockMetricInfo",
  Severity.WARNING,
  false)]
namespace MockMetrics
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
    public class MockMetricInfo : IHighlighting
    {
        public const string SeverityId = "MockMetricInfo";

        private readonly ISnapshot _snapshot;

        public MockMetricInfo(ISnapshot snapshot)
        {
            _snapshot = snapshot;
        }

        public string ToolTip
        {
            get { return _snapshot.ToString(); }
        }

        public string ErrorStripeToolTip
        {
            get { return _snapshot.ToString(); }
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
