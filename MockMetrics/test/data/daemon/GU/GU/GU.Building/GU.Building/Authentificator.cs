using Common.DA.ProviderConfiguration;
using Common.UI;
using GU.Building.BL;
using GU.Building.View;
using GU.Building.ViewModel;

namespace GU.Building
{
    /// <summary>
    /// Класс, предназначенный для проведения аутентификации пользователя.
    /// </summary>
    internal class Authentificator
    {
        private readonly IProviderConfiguration _configuration;
        private bool _isDebug;

        /// <summary>
        /// Класс, предназначенный для проведения аутентификации пользователя.
        /// </summary>
        public Authentificator(IProviderConfiguration configuration, bool isDebug)
        {
            _isDebug = isDebug;
            IsPassed = false;
            _configuration = configuration;
        }

        /// <summary>
        /// Возвращает флаг успешности прохождения аутентификации.
        /// </summary>
        public bool IsPassed { get; private set; }

        /// <summary>
        /// Проводит аутентификацию пользователя
        /// </summary>
        public void AuthentificateUser()
        {
            if(_isDebug)
            {
                IsPassed = true;
                _configuration.User = "test_building";
                _configuration.Password = "test";
            }
            else
            {
                var up = BuildingFacade.GetUserPreferences();
                var loginVm = new LoginVM(up.LastLogin, _configuration);
                if (UIFacade.ShowConfirmableDialogView(new LoginView(), loginVm, "Аутентификация пользователя"))
                {
                    IsPassed = true;
                    up.LastLogin = loginVm.Login;
                    up.SaveSettings();
                }
            }
        }        
    }
}
