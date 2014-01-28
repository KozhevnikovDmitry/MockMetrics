using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Common.BL;
using Common.BL.Authentification;
using Common.DA;
using Common.DA.ProviderConfiguration;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;

using GU.BL;
using GU.MZ.DS.BL;
using GU.MZ.DS.View;
using GU.UI.View.TaskView;
using GU.UI.View.UserView;

namespace GU.MZ.DS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var splashService = new SplashService();

            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            try
            {
                var init = new Initializer();
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                if (init.Login())
                {
                    splashService.Show("Министерство здравоохранения. АИС Лекарственное обеспечение");
                    if (init.Initilaize())
                    {
                        splashService.Close();
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        var main = new MainWindow();
                        main.ShowDialog();
                    }
                    else
                    {
                        NoticeUser.ShowError(init.InitializationException);
                    }
                }
                Current.Shutdown();
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        private class Initializer
        {
            private IProviderConfiguration _configuration;

            public GUException InitializationException { get; private set; }

            public bool Login()
            {
                var confFactory = new ProviderConfigurationFactory();
                this._configuration = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
                var authentificator = new Authentificator(this._configuration, new DataAccessLayerInitializer());
                var connectChecker = new ConnectChecker(_configuration);
#if DEBUG
                return authentificator.AuthentificateUser("test_mz_ds", "test");
#else
                var userPreferences = new DsUserPreferences();
                var loginVm = new LoginVM(authentificator, connectChecker);
                loginVm.Login = userPreferences.LastLogin;
                if(UIFacade.ShowConfirmableDialogView(new LoginView(), 
                                                          loginVm, 
                                                          "Аутентификация пользователя"))
                {
                    userPreferences.LastLogin = loginVm.Login;
                    userPreferences.SaveSettings();
                    return true;
                }
                return false;

#endif
            }

            public bool Initilaize()
            {
                try
                {
                    GuFacade.InitializeCore(this._configuration);
                    DsFacade.InitializeCore();
                    this.InitializeUI();
                    return true;
                }
                catch (Exception ex)
                {
                    this.InitializationException = new GUException("Ошибка иницализации приложения", ex);
                    return false;
                }
            }

            private void InitializeUI()
            {
                // Собираем dm-сборки
                var dmAssemlies = new List<Assembly>
                                      {
                                          Assembly.UnsafeLoadFrom("GU.MZ.DS.DataModel.dll"),
                                          Assembly.UnsafeLoadFrom("GU.DataModel.dll")
                                      };

                // Собираем ui-сборки
                var uiAssemlies = new List<Assembly>
                                      { Assembly.UnsafeLoadFrom("GU.MZ.DS.UI.dll"), Assembly.UnsafeLoadFrom("GU.UI.dll") };

                // Регистрируем типы для UI
                UIFacade.InitializeUI(DsFacade.GetLogicContainer());
                UIFacade.RegisterVMTypes(uiAssemlies);
                UIFacade.RegisterSearchProperties(dmAssemlies);

                // Регистрируем пресеты кастомного поиска
                UIFacade.RegisterSearchPresetList(
                    DsFacade.GetUserPreferences().SearchPresetSpecContainer.PresetSpecList);
                UIFacade.RegisterSearchPresetList(
                    GuFacade.GetUserPreferences().SearchPresetSpecContainer.PresetSpecList);

                // Регистрируем view для редактирования доменных объектов
                UIFacade.RegisterEditableView<TaskDockableView, GU.DataModel.Task>();
                UIFacade.RegisterEditableView<UserView, GU.DataModel.DbUser>();
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
