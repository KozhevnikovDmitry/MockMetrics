using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Common.DA.ProviderConfiguration;
using Common.Types;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;

using GU.BL;
using GU.UI.View.TaskView;
using GU.ZLP.View;
using GU.ZLP.ViewModel;

namespace GU.ZLP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ProviderConfigurationFactory confFactory = new ProviderConfigurationFactory();
            var config = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
            try
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Authentificator auth = new Authentificator(config);
                auth.AuthentificateUser();
                if (auth.IsPassed)
                {
                    SplashWindow splash = new SplashWindow()
                    {
                        DataContext = new SplashVM() { ApplicationName = auth.AgencyName }
                    };

                    Task init = new Task(() =>
                    {
                        GuFacade.InitializeCore(config);

                        this.InitializeUI();

                        Action close = new Action(() => splash.Close());
                        Application.Current.Dispatcher.BeginInvoke(close, null);
                    });
                    init.Start();
                    splash.ShowDialog();

                    init.Wait();                                    

                    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    MainWindow main = new MainWindow();
                    main.DataContext = new MainVM();
                    main.ShowDialog();

                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
                Application.Current.Shutdown();
            }
        }

        private void InitializeUI()
        {
            // Собираем dm-сборки
            var dmAssemlies = new List<Assembly> { Assembly.UnsafeLoadFrom("GU.DataModel.dll") };

            // Регистрируем типы для UI
            UIFacade.InitializeUI(GuFacade.GetLogicContainer());
            UIFacade.RegisterVMTypes(new List<Assembly> { Assembly.UnsafeLoadFrom("GU.UI.dll") });
            UIFacade.RegisterSearchProperties(dmAssemlies);

            // Сборка UI
            UIFacade.RegisterVMTypes(new List<Assembly> { Assembly.UnsafeLoadFrom("GU.ZLP.exe") });

            // Регистрируем view для редактирования доменных объектов
            UIFacade.RegisterEditableView<TaskDockableView, GU.DataModel.Task>();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                LoggerContainer.LogError(e.Exception);
            }
            catch (Exception)
            {
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
            catch (Exception)
            { 
            }
        }
    }
}
