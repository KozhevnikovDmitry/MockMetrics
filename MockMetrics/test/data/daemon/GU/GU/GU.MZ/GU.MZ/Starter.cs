using System;
using System.Windows;
using Common.BL;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using GU.BL;
using GU.MZ.BL;
using GU.MZ.UI.View;
using GU.MZ.UI.ViewModel;

namespace GU.MZ
{
    internal class Starter
    {
        private readonly Lazy<LoginVM> _loginVm;
        private readonly Lazy<MainVm> _mainVm;
        private readonly GumzUserPreferences _userPreferences;
        private readonly SplashService _splashService;
        private readonly IDomainLogicContainer _domainLogicContainer;

        public Starter(Lazy<LoginVM> loginVm,
                       Lazy<MainVm> mainVm, 
                       GumzUserPreferences userPreferences, 
                       SplashService splashService,
                       IDomainLogicContainer domainLogicContainer)
        {
            _loginVm = loginVm;
            _mainVm = mainVm;
            _userPreferences = userPreferences;
            _splashService = splashService;
            _domainLogicContainer = domainLogicContainer;
        }

        public void Start()
        {
            if (Login())
            {
                Launch();
            }
        }

        private bool Login()
        {
            var loginVm = _loginVm.Value;
#if DEBUG
            loginVm.Login = "mz_lic_it_min";
            loginVm.Password = "test";
            loginVm.Confirm();
            return true;
#else
            loginVm.Login = _userPreferences.LastLogin;
            if (UIFacade.ShowConfirmableDialogView(new Common.UI.View.LoginView(),
                loginVm,
                "Аутентификация пользователя",
                showInTaskbar: true))
            {
                _userPreferences.LastLogin = loginVm.Login;
                _userPreferences.SaveSettings();
                return true;
            }
            return false;
#endif
        }

        private void Launch()
        {
            try
            {
                _splashService.Show("Министерство здравоохранения. АИС Лицензирование");
                InitSingletones();
                var main = new MainWindow
                {
                    DataContext = _mainVm.Value
                };
                Application.Current.MainWindow = main;
                _splashService.Close();
                main.ShowDialog();
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка при запуске приложения", ex);
            }
        }

        private void InitSingletones()
        {
            GumzFacade.InitializeCore(_domainLogicContainer);
            GuFacade.InitializeCore(_domainLogicContainer);
            UIFacade.InitializeUI(_domainLogicContainer);
        }
    }
}