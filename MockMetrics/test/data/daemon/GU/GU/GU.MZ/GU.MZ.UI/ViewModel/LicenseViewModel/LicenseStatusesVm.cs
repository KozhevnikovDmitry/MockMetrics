using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.Event;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.View.LicenseView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseStatusesVm : SmartListVm<LicenseStatus>
    {
        public License License { get; set; }
        private IDialogUiFactory _uiFactory;
        private readonly ISmartValidateableVm<LicenseStatus> _licenseStatusVm;

        public LicenseStatusesVm(IDialogUiFactory uiFactory, ISmartValidateableVm<LicenseStatus> licenseStatusVm)
        {
            _uiFactory = uiFactory;
            _licenseStatusVm = licenseStatusVm;
        }

        protected override void SetListOptions()
        {
            base.SetListOptions();
            SortProperties = new List<string> { Util.GetPropertyName(() => LicenseStatus.CreateInstance().Stamp) };
        }

        protected override void AddItem()
        {
            try
            {
                var licenseStatus = LicenseStatus.CreateInstance();
                licenseStatus.Stamp = DateTime.Now;
                licenseStatus.LicenseStatusType = LicenseStatusType.Active;
                _licenseStatusVm.Initialize(licenseStatus);
                if (_uiFactory.ShowValidateableDialogView(new LicenseStatusView(), _licenseStatusVm, "Новый статус лицензии"))
                {
                    licenseStatus.LicenseId = License.Id;
                    Items.Add(licenseStatus);
                    License.CurrentStatus = licenseStatus.LicenseStatusType;
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
                _licenseStatusVm.Initialize((e.Result as LicenseStatus).Clone());
                if (_uiFactory.ShowValidateableDialogView(new LicenseStatusView(), _licenseStatusVm, "Статус лицензии"))
                {
                    _licenseStatusVm.Entity.CopyTo(e.Result as LicenseStatus);
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

        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                if (NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить статус лицензии") == MessageBoxResult.Yes)
                {
                    Items.Remove(e.Result);
                    if (License.LicenseStatusList.Any())
                    {
                        License.CurrentStatus = License.CurrentLicenseStatus.LicenseStatusType;
                    }
                    else
                    {
                        License.CurrentStatus = LicenseStatusType.Project;
                    }
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
