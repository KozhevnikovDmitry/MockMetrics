using System.Linq;

using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using GU.MZ.BL.Reporting.Data;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Опись предоставленных документов"
    /// </summary>
    public class InventoryReport : IReport
    {
        public string ViewPath { get; private set; }

        public string DataAlias { get; private set; }

        public int InventoryId { get; set; }
        
        /// <summary>
        /// Маппер описи
        /// </summary>
        private readonly IDomainDataMapper<DocumentInventory> _inventoryMapper;

        /// <summary>
        /// Класс отчёт "Опись предоставленных документов"
        /// </summary>
        /// <param name="inventoryMapper">Маппер описи</param>
        public InventoryReport(IDomainDataMapper<DocumentInventory> inventoryMapper)
        {
            _inventoryMapper = inventoryMapper;
            ViewPath = "Reporting/View/GU.MZ/AcceptTaskInventoryReport.mrt";
            DataAlias = "data";
        }

        public object RetrieveData()
        {
            var inventory = _inventoryMapper.Retrieve(InventoryId);

            var data = new AcceptTaskInventory
                {
                    InventoryRegNumber = inventory.RegNumber.ToString().PadLeft(6, '0'),
                    InventoryStamp = inventory.Stamp,

                    InventDocumentList = inventory.ProvidedDocumentList
                                                  .Select(t => new AcceptTaskInventory.InventDocument { DocumentName = t.Name, Count = t.Quantity })
                                                  .ToList(),

                    LicensedActivityName = inventory.LicensedActivity,
                    HolderName = inventory.LicenseHolder,
                    AuthorName = inventory.EmployeeName,
                    AuthorPosition = inventory.EmployeePosition
                };

            return data;
        }
    }
}
