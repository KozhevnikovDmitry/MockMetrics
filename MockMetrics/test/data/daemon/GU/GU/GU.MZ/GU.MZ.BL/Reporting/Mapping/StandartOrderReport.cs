using System;
using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Типовой приказ"
    /// </summary>
    public class StandartOrderReport : BaseReport
    {

        public StandartOrder StandartOrder { get; set; }

        private readonly IDomainDataMapper<StandartOrder> _orderMapper;
        private readonly IDomainDataMapper<DossierFile> _fileMapper;
        private readonly Func<IDomainDbManager> _getDb;

        public StandartOrderReport(IDomainDataMapper<StandartOrder> orderMapper, IDomainDataMapper<DossierFile> fileMapper, Func<IDomainDbManager> getDb)
        {
            _orderMapper = orderMapper;
            _fileMapper = fileMapper;
            _getDb = getDb;
            ViewPath = "Reporting/View/GU.MZ/StandartOrderReport.mrt";
        }

        public override object RetrieveData()
        {
            using (var db = _getDb())
            {
                var dossierFile = db.GetDomainTable<DossierFileScenarioStep>()
                                    .Where(t => t.Id == StandartOrder.FileScenarioStepId)
                                    .Select(t => t.DossierFile)
                                    .Single();

                _fileMapper.FillAssociations(dossierFile, db);

                var order = _orderMapper.Retrieve(StandartOrder.Id, db);
                order.FormatFields(dossierFile);
                return order;
            }
        }
    }
}
