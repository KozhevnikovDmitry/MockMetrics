using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerBaseRegVM : AbstractListVM<DeclarerBaseRegItem>
    {
        private readonly Claim _claim;

        public DeclarerBaseRegVM(Claim claim) : base(claim.DeclarerBaseReg.BaseRegItems)
        {
            _claim = claim;
        }

        /// <summary>
        /// Добавить основание указанное заявителем
        /// </summary>
        protected override void AddItem()
        {
            _claim.DeclarerBaseReg.BaseRegItems.Add(DeclarerBaseRegItem.CreateInstance());
        }

        /// <summary>
        /// Удалить основание указанное заявителем
        /// </summary>
        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                var dlr =
                    NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить причину?");
                if (dlr == MessageBoxResult.Yes)
                {
                    _claim.DeclarerBaseReg.BaseRegItems.Remove((DeclarerBaseRegItem)e.Result);
                }

            }
            catch (BLLException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }
    }
}
