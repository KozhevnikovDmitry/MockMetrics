using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Common.UI.ViewModel.Interfaces;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.Interface
{
    public interface IDialogUiFactory
    {
        void ShowToolView(UserControl view,
            NotificationObject viewModel,
            string displayName);

        bool ShowDialogView(UserControl view,
            INotifyPropertyChanged viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            bool showInTaskbar = false);

         bool ShowValidateableDialogView(UserControl view,
            IValidateableVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight);

         bool ShowConfirmableDialogView(UserControl view,
            IConfirmableVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            bool showInTaskbar = false);

         bool ShowSearchDialogView(ISearchVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            Size? size = null,
            Size? minSize = null);
    }
}