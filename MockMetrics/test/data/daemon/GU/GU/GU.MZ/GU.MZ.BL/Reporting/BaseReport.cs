using Common.BL.ReportMapping;

namespace GU.MZ.BL.Reporting
{
    public abstract class BaseReport : IReport
    {
        protected BaseReport()
        {
            DataAlias = "data";
        }

        public string ViewPath { get; protected set; }

        public string DataAlias { get; protected set; }

        public abstract object RetrieveData();
    }
}
