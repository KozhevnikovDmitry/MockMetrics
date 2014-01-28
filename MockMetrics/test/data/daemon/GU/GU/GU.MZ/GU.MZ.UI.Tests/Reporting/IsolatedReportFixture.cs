using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Common.BL.ReportMapping;
using Common.UI;

namespace GU.MZ.UI.Tests.Reporting
{
    public class IsolatedReportFixture
    {
        protected void ShowReport(IReport report, string title = "Тестовый отчёт")
        {
            var uiFactory = new UiFactory(null);
            var viewer = uiFactory.GetReportPresenter(report);

            try
            {
                uiFactory.ShowDialogView(
                    viewer,
                    viewer.DataContext as INotifyPropertyChanged,
                    title,
                    ResizeMode.CanResize,
                    SizeToContent.WidthAndHeight,
                    true);
            }
            catch (InvalidComObjectException)
            {
                throw;
            }
        }
    }
}