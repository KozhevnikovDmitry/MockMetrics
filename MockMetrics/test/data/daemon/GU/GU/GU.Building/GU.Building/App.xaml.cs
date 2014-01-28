using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Common.DA.ProviderConfiguration;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using GU.BL;
using GU.Building.BL;
using GU.Building.View;
using GU.Building.ViewModel;
using GU.UI.View.TaskView;
using GU.UI.View.UserView;

namespace GU.Building
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                var confFactory = new ProviderConfigurationFactory();
                var config = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                var auth = new Authentificator(config, IsDebug);
                auth.AuthentificateUser();
                if (auth.IsPassed)
                {
                    SplashWindow splash = new SplashWindow
                    {
                        DataContext = new SplashVM { ApplicationName = "АИС Строительство и архитектура" }
                    };

                    var init =
                        new Task<GUException>(() => InitilizeApplication(config, splash));

                    init.Start();
                    splash.ShowDialog();

                    init.Wait();
                    if (init.Result != null)
                    {
                        NoticeUser.ShowError(init.Result);
                        Current.Shutdown();
                    }

                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    var main = new MainWindow { DataContext = new MainVM(IsDebug) };
                    main.ShowDialog();
                }
                else
                {
                    Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);

            }
        }

        private GUException InitilizeApplication(IProviderConfiguration configuration, SplashWindow splash)
        {
            try
            {
                GuFacade.InitializeCore(configuration);
                BuildingFacade.InitializeCore(configuration);
                this.InitializeUI();

            }
            catch (Exception ex)
            {
                return new GUException("Ошибка иницализации приложения", ex);
            }
            finally
            {
                Action close = splash.Close;
                Current.Dispatcher.BeginInvoke(close, null);
            }
            return null;
        }

        private void InitializeUI()
        {
            // Собираем dm-сборки
            var dmAssemlies = new List<Assembly> { Assembly.UnsafeLoadFrom("GU.Building.DataModel.dll"), Assembly.UnsafeLoadFrom("GU.DataModel.dll") };
            //Собираем ui-сборки
            var uiAssemlies = new List<Assembly> { Assembly.UnsafeLoadFrom("GU.Building.UI.dll"), Assembly.UnsafeLoadFrom("GU.UI.dll") };

            //Регистрируем типы для UI
            UIFacade.InitializeUI(BuildingFacade.GetLogicContainer());
            UIFacade.RegisterVMTypes(uiAssemlies);
            UIFacade.RegisterSearchProperties(dmAssemlies);

            // Регистрируем view для редактирования доменных объектов
            UIFacade.RegisterEditableView<TaskDockableView, GU.DataModel.Task>();
            UIFacade.RegisterEditableView<UserView, GU.DataModel.DbUser>();

            UIFacade.RegisterSearchPresetList(
                GuFacade.GetUserPreferences().SearchPresetSpecContainer.PresetSpecList);
        }

        private void OnApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
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

        private bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}
