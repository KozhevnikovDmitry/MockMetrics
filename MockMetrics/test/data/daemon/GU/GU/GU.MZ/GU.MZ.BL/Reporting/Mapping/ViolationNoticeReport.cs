using System;
using Common.BL.ReportMapping;
using Common.DA.Interface;
using GU.MZ.DataModel.Notifying;

namespace GU.MZ.BL.Reporting.Mapping
{
    public class ViolationNoticeReport : IReport
    {
        private readonly Func<IDomainDbManager> _getDb;

        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }

        public int ViolationNoticeId { get; set; }

        protected ViolationNoticeReport()
        {
            ViewPath = "Reporting/View/GU.MZ/ViolationNoticeReport.mrt";
            DataAlias = "data";
        }

        public ViolationNoticeReport(Func<IDomainDbManager> getDb)
            :this()
        {
            _getDb = getDb;
        }

        public virtual object RetrieveData()
        {
            return _getDb().RetrieveDomainObject<ViolationNotice>(ViolationNoticeId);
        }
    }
}