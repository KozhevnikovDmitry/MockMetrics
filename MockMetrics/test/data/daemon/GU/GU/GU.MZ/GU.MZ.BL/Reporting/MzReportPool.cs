using GU.MZ.BL.Reporting.Mapping;

namespace GU.MZ.BL.Reporting
{
    public class MzReportPool : IReportFacade
    {
        public FullActivityDataReport FullActivityDataReport { get; private set; }
        public StatementByServiceReport StatementByServiceReport { get; private set; }
        public LicenseByActivityReport LicenseByActivityReport { get; private set; }

        public MzReportPool(FullActivityDataReport fullActivityDataReport, 
                            StatementByServiceReport statementByServiceReport, 
                            LicenseByActivityReport licenseByActivityReport)
        {
            FullActivityDataReport = fullActivityDataReport;
            StatementByServiceReport = statementByServiceReport;
            LicenseByActivityReport = licenseByActivityReport;
        }
    }
}
