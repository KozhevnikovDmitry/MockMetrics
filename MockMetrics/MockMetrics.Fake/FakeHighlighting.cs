using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using MockMetrics.Fake;

[assembly: RegisterConfigurableSeverity(FakeHighlighting.SeverityId,
  null,
  HighlightingGroupIds.CodeSmell,
  "FakeHighlighting",
  "FakeHighlighting",
  Severity.WARNING,
  false)]

namespace MockMetrics.Fake
{
    [ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
    public class FakeHighlighting : IHighlighting
    {
        public const string SeverityId = "FakeHighlighting";

        public bool IsValid()
        {
            return true;
        }

        public string ToolTip { get; private set; }
        public string ErrorStripeToolTip { get; private set; }
        public int NavigationOffsetPatch { get; private set; }
    }
}
