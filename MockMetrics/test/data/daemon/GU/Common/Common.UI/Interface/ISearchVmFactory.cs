using System.Windows;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI
{
    public interface ISearchVmFactory
    {
        ISearchResultVM<T> GetSearchResultVm<T>(T result) where T : IDomainObject;

        ISearchVM<T> GetSearchVm<T>() where T : IPersistentObject;
    }

    public interface ISearchDialogFactory : ISearchVmFactory
    {
        bool ShowSearchDialogView(ISearchVM viewModel,
                                  string displayName,
                                  ResizeMode resizeMode = ResizeMode.NoResize,
                                  SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
                                  Size? size = null,
                                  Size? minSize = null);
    }
}