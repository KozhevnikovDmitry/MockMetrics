using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.Event;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using GU.MZ.UI.View.LicenseView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseRequisitesListVm : SmartListVm<LicenseRequisites>
    {
        private IDialogUiFactory _uiFactory;
        private readonly ISmartValidateableVm<LicenseRequisites> _licRequisitesVm;

        public LicenseRequisitesListVm(IDialogUiFactory uiFactory, ISmartValidateableVm<LicenseRequisites> licRequisitesVm)
        {
            _uiFactory = uiFactory;
            _licRequisitesVm = licRequisitesVm;
        }

        protected override void AddItem()
        {
            try
            {
                var licenseRequisites = LicenseRequisites.CreateInstance();
                licenseRequisites.CreateDate = DateTime.Now;
                licenseRequisites.JurRequisites = JurRequisites.CreateInstance();
                licenseRequisites.Address = Address.CreateInstance();
                _licRequisitesVm.Initialize(licenseRequisites);
                if (_uiFactory.ShowValidateableDialogView(new LicenseRequisitesView(), _licRequisitesVm, "Новые реквизиты лицензии"))
                {
                    Items.Add(licenseRequisites);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        protected override void OnOpenItemRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                _licRequisitesVm.Initialize((e.Result as LicenseRequisites).Clone());
                if (_uiFactory.ShowValidateableDialogView(new LicenseRequisitesView(), _licRequisitesVm, "Реквизиты лицензии"))
                {
                    _licRequisitesVm.Entity.CopyTo(e.Result as LicenseRequisites);
                    ListItemVMs.Single(t => t.Item.Equals(e.Result)).OnDisplayPropertyChanged();
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }
    }
}
