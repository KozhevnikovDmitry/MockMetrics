using GU.MZ.BL.Reporting.Mapping;

namespace GU.MZ.BL.Reporting
{
    public interface IReportFacade
    {
        FullActivityDataReport FullActivityDataReport { get; }
        StatementByServiceReport StatementByServiceReport { get; }
        LicenseByActivityReport LicenseByActivityReport { get; }
    }
}