using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Common.BL;
using Common.DA.ProviderConfiguration;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.UI.View.ClaimView;
using GU.HQ.View;
using GU.HQ.ViewModel;
using GU.UI.View.TaskView;
using GU.UI.View.UserView;
using LoginView = Common.UI.View.LoginView;
using LoginVM = Common.UI.ViewModel.LoginVM;

namespace GU.HQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IProviderConfiguration _configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                if (Login())
                {
                    SplashWindow splash = new SplashWindow
                    {
                        DataContext = new SplashVM { ApplicationName = "АИС УНЖ" }
                    };

                    var init =
                        new Task<GUException>(() => InitilizeApplication(_configuration, splash));

                    init.Start();
                    splash.ShowDialog();

                    init.Wait();
                    if (init.Result != null)
                    {
                        NoticeUser.ShowError(init.Result);
                        Current.Shutdown();
                    }

                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    var main = new MainWindow { DataContext = new MainVM() };
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

        public bool Login()
        {
            var confFactory = new ProviderConfigurationFactory();
            _configuration = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
            var daInitializer = new Common.DA.DataAccessLayerInitializer();
            var connectChecker = new ConnectChecker(_configuration);
            var authentificator = new Common.BL.Authentification.Authentificator(_configuration, daInitializer);
#if DEBUG
            return authentificator.AuthentificateUser("test_mun", "test");
#else
            var userPreferences = new HqUserPreferences();
            var loginVm = new LoginVM(authentificator, connectChecker);
            loginVm.Login = userPreferences.LastLogin;
            if (UIFacade.ShowConfirmableDialogView(new LoginView(),
                                                  loginVm,
                                                  "Аутентификация пользователя",
                                                  showInTaskbar: true))
            {
                userPreferences.LastLogin = loginVm.Login;
                userPreferences.SaveSettings();
                return true;
            }
            return false;

#endif
        }

        private GUException InitilizeApplication(IProviderConfiguration configuration, SplashWindow splash)
        {
            try
            {
                GuFacade.InitializeCore(configuration);
                HqFacade.InitializeCore(configuration);
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
            var dmAssemlies = new List<Assembly> { Assembly.UnsafeLoadFrom("GU.HQ.DataModel.dll"), Assembly.UnsafeLoadFrom("GU.DataModel.dll") };
            //Собираем ui-сборки
            var uiAssemlies = new List<Assembly> { Assembly.UnsafeLoadFrom("GU.HQ.UI.dll"), Assembly.UnsafeLoadFrom("GU.UI.dll") };

            //Регистрируем типы для UI
            UIFacade.InitializeUI(HqFacade.GetLogicContainer());
            UIFacade.RegisterVMTypes(uiAssemlies);
            UIFacade.RegisterSearchProperties(dmAssemlies);

            // Регистрируем view для редактирования доменных объектов
            UIFacade.RegisterEditableView<TaskDockableView, GU.DataModel.Task>();
            UIFacade.RegisterEditableView<UserView, GU.DataModel.DbUser>();
            UIFacade.RegisterEditableView<ClaimView, Claim>();


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
