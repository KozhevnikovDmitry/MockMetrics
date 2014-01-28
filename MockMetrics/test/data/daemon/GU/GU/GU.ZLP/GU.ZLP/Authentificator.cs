using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI.Views;
using Common.UI.ViewModels;
using GU.ZLP.ViewModel;
using GU.ZLP.View;
using Common.UI;
using GU.BL;
using Common.DA.ProviderConfiguration;

namespace GU.ZLP
{
    /// <summary>
    /// Класс, предназначенный для проведения аутентификации пользователя.
    /// </summary>
    internal class Authentificator
    {
        private IProviderConfiguration _configuration;

        /// <summary>
        /// Класс, предназначенный для проведения аутентификации пользователя.
        /// </summary>
        public Authentificator(IProviderConfiguration configuration)
        {
            IsPassed = false;
            _configuration = configuration;
        }

        /// <summary>
        /// Возвращает флаг успешности прохождения аутентификации.
        /// </summary>
        public bool IsPassed { get; private set; }

        /// <summary>
        /// Возвращает логин пользователя прошедшего аутентификацию.
        /// </summary>
        public string Login { get; private set; }

        public string AgencyName { get; private set; }

        /// <summary>
        /// Возвращает пароль пользователя прошедшего аутентификацию.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Проводит аутентификацию пользователя
        /// </summary>
        public void AuthentificateUser()
        {
#if DEBUG
            IsPassed = true;
            _configuration.User = "test_archive";
            _configuration.Password = "test";
            this.Login = "test_archive";
            this.Password = "test";
#else
            // TODO: LastLogin лучше бы утащить из UP, т.к. UP должны по-хорошему создаваться только
            //          после вызова Core.Initialize
            var up = GU.BL.GuFacade.GetUserPreferences() as GuUserPreferences;
            LoginVM loginVm = new LoginVM(up.LastLogin, _configuration);
            if (UIFacade.ShowConfirmableDialogView(new LoginView(), loginVm, "Аутентификация пользователя"))
            {
                IsPassed = true;
                AgencyName = loginVm.AgencyName;
                up.LastLogin = loginVm.Login;
                up.SaveSettings();
            }
#endif

        }        
    }
}
