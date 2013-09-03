using System.Windows.Forms;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.ReSharper.Features.Environment.Options.Inspections;
using JetBrains.ReSharper.Features.Environment.Src.Options;
using JetBrains.UI.Application;
using JetBrains.UI.Options;
using JetBrains.UI.Options.Helpers;

namespace MockMetrics
{
    [OptionsPage(PID, "MockMetrics", typeof(FeaturesEnvironmentOptionsThemedIcons.GeneratedCode), ParentId = CodeInspectionPage.PID)]
    public class MockMetricsOptionPage : AStackPanelOptionsPage
    {
        private readonly Lifetime myLifetime;
        private readonly OptionsSettingsSmartContext mySettings;
        private const string PID = "MockMetrics";

        public MockMetricsOptionPage(Lifetime lifetime, UIApplication environment, OptionsSettingsSmartContext settings)
            : base(lifetime, environment, PID)
        {
            myLifetime = lifetime;
            mySettings = settings;
            InitControls();
        }

        private void InitControls()
        {
            var arrangeAmountSpin = GetAmountSpin();
            var actAmountSpin = GetAmountSpin();
            var assertAmountSpin = GetAmountSpin();

            var arrangeCommentBox = GetCommentTextBox();
            var actCommentBox = GetCommentTextBox();
            var assertCommentBox = GetCommentTextBox();

            var stack1 = new Controls.VertStackPanel(Environment) {Width = 200};
            var stack2 = new Controls.VertStackPanel(Environment) { Width = 200 };

            var stack = new Controls.HorzStackPanel(Environment);


            stack1.Controls.Add(GetLabel("Arranged objects amount"));
            stack2.Controls.Add(arrangeAmountSpin);

            stack1.Controls.Add(GetLabel("Performed calls amount"));
            stack2.Controls.Add(actAmountSpin);

            stack1.Controls.Add(GetLabel("Asserted values amount"));
            stack2.Controls.Add(assertAmountSpin);

            stack1.Controls.Add(GetLabel("Arrange section comment"));
            stack2.Controls.Add(arrangeCommentBox);

            stack1.Controls.Add(GetLabel("Act section comment"));
            stack2.Controls.Add(actCommentBox);

            stack1.Controls.Add(GetLabel("Assert section comment"));
            stack2.Controls.Add(assertCommentBox);

            stack.Controls.Add(stack1);
            stack.Controls.Add(stack2);

            Controls.Add(GetLabel("MockMetrics options"));
            Controls.Add(JetBrains.UI.Options.Helpers.Controls.Separator.DefaultHeight);
            Controls.Add(stack);

            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.ArrangeAmount, arrangeAmountSpin.IntegerValue);
            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.ActAmount, actAmountSpin.IntegerValue);
            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.AssertAmount, assertAmountSpin.IntegerValue);

            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.ArrangeComment, arrangeCommentBox.Text);
            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.ActComment, actCommentBox.Text);
            mySettings.SetBinding(myLifetime, (MockMetricsSettings s) => s.AssertComment, assertCommentBox.Text);
        }

        private Controls.Label GetLabel(string text)
        {
            var label = new Controls.Label(text)
            {
                Width = 175,
                Margin = new Padding(2,1,2,2)
            };
            return label;
        }

        private Controls.Spin GetAmountSpin()
        {
            var spin = new Controls.Spin
            {
                Maximum = new decimal(new[] {500, 0, 0, 0}),
                Minimum = new decimal(new[] {1, 0, 0, 0}),
                Value = new decimal(new[] {1, 0, 0, 0}),
                Width = 200,
                Margin = new Padding(2)
            };
            return spin;
        }

        private Controls.EditBox GetCommentTextBox()
        {
            var editBox = new Controls.EditBox
            {
                Width = 200,
                AutoSize = false,
                Margin = new Padding(2)
            };
            return editBox;
        }
    }
}

