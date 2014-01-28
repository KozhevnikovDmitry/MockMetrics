using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Приказ о проведении выездной проверки"
    /// </summary>
    public class InspectionOrderReport : IReport
    {
        private readonly IDomainDataMapper<InspectionOrder> _orderMapper;

        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }

        public int InspectionOrderId { get; set; }

        public InspectionOrderReport(IDomainDataMapper<InspectionOrder> orderMapper)
        {
            _orderMapper = orderMapper;
            ViewPath = "Reporting/View/GU.MZ/InspectionOrderReport.mrt";
            DataAlias = "data";
        }

        public object RetrieveData()
        {
            return _orderMapper.Retrieve(InspectionOrderId);
        }
    }
}
