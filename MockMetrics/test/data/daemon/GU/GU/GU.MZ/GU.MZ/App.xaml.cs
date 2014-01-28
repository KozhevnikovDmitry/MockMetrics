using System;
using System.Windows;
using System.Windows.Threading;
using Common.Types;
using Common.UI;

namespace GU.MZ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += Application_DispatcherUnhandledException;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); 
            var root = new CompositionRoot();
            try
            {
                root.Register();
                root.Resolve().Start();
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
            finally
            {
                root.Release();
                Shutdown();
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                LoggerContainer.LogError(e.Exception);
            }
            finally
            {
                e.Handled = true;
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                LoggerContainer.LogError((Exception)e.ExceptionObject);
            }
            finally
            {

            }

        }
    }
}
