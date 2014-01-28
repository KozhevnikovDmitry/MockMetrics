using Common.Types;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseStatusItemVm : SmartListItemVm<LicenseStatus>
    {
        public override void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => Note);
        }

        #region Binding Properties

        public string Name
        {
            get { return Entity.LicenseStatusType.GetDescription(); }
        }

        public string Stamp
        {
            get { return string.Format("{0} {1}", Entity.Stamp.ToLongDateString(), Entity.Stamp.ToLongTimeString()); }
        }

        public string Note
        {
            get { return Entity.Note; }
        }


        #endregion
    }
}
