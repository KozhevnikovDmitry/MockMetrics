using Common.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InventoryViewModel
{
    public class InventoryInfoVm : EntityInfoVm<DocumentInventory>
    {
        public InventoryInfoVm(IDomainDataMapper<DocumentInventory> entityMapper) 
            : base(entityMapper)
        {
        }

        #region Binding Properties

        public string RegNumber { get { return Entity.RegNumber.ToString(); } }

        public string HolderName { get { return Entity.LicenseHolder; } }

        public string LicensedActivityName { get { return Entity.LicensedActivity; } }

        public string AuthorPosition { get { return Entity.EmployeePosition; } }

        public string AuthorName { get { return Entity.EmployeeName; } }

        public string InventoryStamp { get { return Entity.Stamp.ToShortDateString(); } }

        public string Documents { get { return string.Format("Предоставлено документов: {0} шт.", Entity.ProvidedDocumentList.Count); } }

        #endregion
    }
}
