using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Standart
{
    public class StandartOrderDetailListVm : SmartListVm<StandartOrderDetail>
    {
        private StandartOrder _standartOrder;

        public StandartOrder StandartOrder
        {
            get { return _standartOrder; }
            set
            {
                _standartOrder = value;
                Title = _standartOrder.AnnexPreamble;
            }
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
                var orderDetail = StandartOrderDetail.CreateInstance();
                orderDetail.StandartOrder = StandartOrder;
                Items.Add(orderDetail);
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
