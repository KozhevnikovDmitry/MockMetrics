using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.View.OrderView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionHolderAddressListVm : SmartListVm<InspectionHolderAddress>
    {
        public InspectionOrder InspectionOrder { get; set; }
        private readonly IDialogUiFactory _uiFactory;
        private readonly AddHolderAddressForOrderVm _addressForOrderVm;

        public InspectionHolderAddressListVm(IDialogUiFactory uiFactory, AddHolderAddressForOrderVm addressForOrderVm)
        {
            _uiFactory = uiFactory;
            _addressForOrderVm = addressForOrderVm;
        }


        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = true;
            CanRemoveItems = true;
        }

        protected override void AddItem()
        {
            try
            {
                _addressForOrderVm.Initialize(InspectionOrder);
                var addView = new AddHolderAddressForOrderView { DataContext = _addressForOrderVm };
                if (_uiFactory.ShowDialogView(addView, _addressForOrderVm, "Добавить адрес"))
                {
                    var address = InspectionHolderAddress.CreateInstance();
                    address.Address = _addressForOrderVm.Address;
                    Items.Add(address);
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
