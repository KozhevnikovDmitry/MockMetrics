using System.Windows.Controls;

using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс, представляющий Модель Представления для окна диалога с функционалом валидации данных.
    /// </summary>
    internal class ValidateableDialogVM : DialogVM
    {
        /// <summary>
        /// Класс, представляющий Модель Представления для окна диалога с функционалом валидации данных.
        /// </summary>
        /// <param name="view">View, отображаемое в диалоге</param>
        /// <param name="viewModel">ViewModel отображаемого View</param>
        public ValidateableDialogVM(UserControl view, IValidateableVM viewModel )
        {
            _viewModel = viewModel;
            view.DataContext = _viewModel;
            View = view;
        }

        /// <summary>
        /// ViewModel отображаемого View
        /// </summary>
        private readonly IValidateableVM _viewModel;     

        #region Binding Commands

        /// <summary>
        /// Проводит валидацию данных и, если всё хорошо, проставляет положиетельный результат диалога.
        /// </summary>
        protected override void OkMethod()
        {
            if (_viewModel.IsValid)
            {
                IsOkResult = true;
                CloseView = true;
            }
            else
            {
                _viewModel.RaiseIsValidChanged();
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
