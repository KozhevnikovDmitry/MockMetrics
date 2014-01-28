using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseRequisitesListItemVm : SmartListItemVm<LicenseRequisites>
    {
        public override void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => Address);
            RaisePropertyChanged(() => Stamp);
        }

        #region Binding Properties

        public string Name
        {
            get { return Entity.ToString(); }
        }

        public string Address
        {
            get { return Entity.Address.ToLongString(); }
        }

        public string Stamp
        {
            get { return string.Format("{0} {1}", Entity.CreateDate.ToLongDateString(), Entity.CreateDate.ToLongTimeString()); }
        }

        #endregion
    }
}
