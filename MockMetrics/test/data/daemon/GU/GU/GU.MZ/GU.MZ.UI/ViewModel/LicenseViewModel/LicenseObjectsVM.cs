using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.Event;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.View.LicenseView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка объектов с номенклатурой лицензии
    /// </summary>
    public class LicenseObjectsVm : SmartListVm<LicenseObject>
    {
        public License License { get; set; }
        private IDialogUiFactory _uiFactory;
        private readonly ISmartValidateableVm<LicenseObject> _licenseObjectVm;

        public LicenseObjectsVm(IDialogUiFactory uiFactory, ISmartValidateableVm<LicenseObject> licenseObjectVm)
        {
            _uiFactory = uiFactory;
            _licenseObjectVm = licenseObjectVm;
        }

        protected override void AddItem()
        {
            try
            {
                var licenseObject = LicenseObject.CreateInstance();
                licenseObject.License = License;
                licenseObject.LicenseId = License.Id;
                _licenseObjectVm.Initialize(licenseObject);
                if (_uiFactory.ShowValidateableDialogView(new LicenseObjectView(), _licenseObjectVm, "Новый объект с номенклатурой"))
                {
                    Items.Add(licenseObject);
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
                _licenseObjectVm.Initialize((e.Result as LicenseObject).Clone());
                if (_uiFactory.ShowValidateableDialogView(new LicenseObjectView(), _licenseObjectVm, "Объект с номенклатурой"))
                {
                    _licenseObjectVm.Entity.CopyTo(e.Result as LicenseObject);
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
