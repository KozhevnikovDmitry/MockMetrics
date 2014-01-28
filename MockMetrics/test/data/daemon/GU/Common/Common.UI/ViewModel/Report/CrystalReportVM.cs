using System.Collections.Generic;

namespace Common.UI.ViewModel.Report
{
    internal class CrystalReportVM : AbstractReportVM
    {
        public CrystalReportVM(string reportPath,
                               Dictionary<string, object> parameters)
        {
            //var viewer = new Common.CrystalReports.CRViewerControl();
            //viewer.LoadReport(reportPath, parameters);
            //WindowsFormsHost.Child = viewer;
        }               
    }
}
