using System.Windows;
using System.Windows.Controls;
using Common.BL.Validation;
using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI
{
    public interface IValidateableUiFactory
    {
        IDomainValidateableVM<T> GetDomainValidateableVm<T>(T entity) where T : IDomainObject;

        void ShowValidationErrorsView(ValidationErrorInfo validationErrorInfo);

        bool ShowValidateableDialogView(UserControl view,
                                        IValidateableVM viewModel,
                                        string displayName,
                                        ResizeMode resizeMode = ResizeMode.NoResize,
                                        SizeToContent sizeToContent = SizeToContent.WidthAndHeight);
    }

    public interface IValidateableDialogUiFactory : IValidateableUiFactory, IDialogUiFactory
    {

    }
}