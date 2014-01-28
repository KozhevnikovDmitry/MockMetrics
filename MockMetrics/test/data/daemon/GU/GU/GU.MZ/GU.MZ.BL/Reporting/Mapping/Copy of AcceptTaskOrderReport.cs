using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Приказ о проведении документарной проверки"
    /// </summary>
    public class ExpertiseOrderReport : IReport
    {
        private readonly IDomainDataMapper<ExpertiseOrder> _orderMapper;

        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }

        public int ExpertiseOrderId { get; set; }

        public ExpertiseOrderReport(IDomainDataMapper<ExpertiseOrder> orderMapper)
        {
            _orderMapper = orderMapper;
            ViewPath = "Reporting/View/GU.MZ/ExpertiseOrderReport.mrt";
            DataAlias = "data";
        }

        public object RetrieveData()
        {
            return _orderMapper.Retrieve(ExpertiseOrderId);
        }
    }
}
