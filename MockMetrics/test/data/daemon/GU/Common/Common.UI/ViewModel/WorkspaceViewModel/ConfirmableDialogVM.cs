using System.Windows.Controls;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс ViewModel для окна диалога с функцией подтверждения.
    /// </summary>
    internal class ConfirmableDialogVM : DialogVM
    {
        /// <summary>
        /// Класс ViewModel для окна диалога с функцией подтверждения.
        /// </summary>
        /// <param name="view">View? отображаемое в диалоге</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        public ConfirmableDialogVM(UserControl view, IConfirmableVM viewModel)
        {
            _viewModel = viewModel;
            view.DataContext = _viewModel;
            View = view;
        }

        /// <summary>
        /// ViewModel отображаемого View
        /// </summary>
        private readonly IConfirmableVM _viewModel;

        #region Binding Commands


        /// <summary>
        /// Проводит процедуру подтверждения и, если всё хорошо, проставляет положиетельный результат диалога.
        /// </summary>
        protected override void OkMethod()
        {
            _viewModel.Confirm();
            if (_viewModel.IsConfirmed)
            {
                IsOkResult = true;
                CloseView = true;
            }
            else
            {
                _viewModel.ResetAfterFail();
            }
        }

        #endregion
    }
}
