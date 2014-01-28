using System;
using System.Windows;

using SpecManager.UI;
using SpecManager.UI.View;
using SpecManager.UI.ViewModel;

namespace SpecManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly CompositionRoot _compositionRoot = new CompositionRoot();

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var mainView = new MainWindow { DataContext = this._compositionRoot.Resolve<MainVm>() };
                mainView.ShowDialog();
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }
    }
}
