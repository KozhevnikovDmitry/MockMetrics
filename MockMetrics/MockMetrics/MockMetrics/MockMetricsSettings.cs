using JetBrains.Application.Settings;
using JetBrains.ReSharper.Settings;

namespace MockMetrics
{
    [SettingsKey(typeof(CodeInspectionSettings), "MockMetrics")]
    class MockMetricsSettings
    {
        [SettingsEntry("// Arrange", "Arrange section comment")]
        public readonly string ArrangeComment;

        [SettingsEntry("// Act", "Act section comment")]
        public readonly string ActComment;

        [SettingsEntry("// Assert", "Assert section comment")]
        public readonly string AssertComment;

        [SettingsEntry(10, "Arranged objects amount")]
        public readonly int ArrangeAmount;

        [SettingsEntry(1, "Performed calls amount")]
        public readonly int ActAmount;

        [SettingsEntry(1, "Asserted values amount")]
        public readonly int AssertAmount;
    }
}
