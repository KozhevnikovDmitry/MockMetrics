using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class NoticeVM : AbstractListVM<Notice>
    {
        private readonly Claim _claim;

        /// <summary>
        /// Класс представления списка уведомлений
        /// </summary>
        /// <param name="claim"></param>
        public NoticeVM(Claim claim)
            : base(claim.Notices)
        {
            _claim = claim;
        }
       
        /// <summary>
        /// Добавление нового уведомления
        /// </summary>
        protected override void AddItem()
        {
            _claim.Notices.Add(Notice.CreateInstance());
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
                    NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить информацию о уведомнелии №" +
                                                    ((Notice)e.Result).DocumentNumber + " от" +
                                                    ((Notice)e.Result).DocumentDate.ToString("dd.MM.yyyy")  + " ?");
                if (dlr == MessageBoxResult.Yes)
                {
                    _claim.Notices.Remove((Notice)e.Result);
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
