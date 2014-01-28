using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModelViewModel
{
    public class ClaimCategoryVM : AbstractListVM<ClaimCategory>
    {
        private readonly Claim _claim;

        public ClaimCategoryVM(Claim claim)
            : base(claim.ClaimCategories)
        {
            _claim = claim;
        }

        protected override void AddItem()
        {
            _claim.ClaimCategories.Add(ClaimCategory.CreateInstance());
        }

        /// <summary>
        /// Удаление уведомления из списка уведомлений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                var dlr =
                    NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить категорию учета?");
                if (dlr == MessageBoxResult.Yes)
                {
                    _claim.ClaimCategories.Remove((ClaimCategory)e.Result);
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
