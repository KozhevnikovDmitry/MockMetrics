using Common.DA;
using Common.DA.ProviderConfiguration;
using Common.UI;
using Common.UI.ViewModel.Attachables.Properties;
using Common.UI.ViewModel.Interfaces;
using GU.Trud.BL;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.Trud.ViewModel
{
    public class LoginVM : NotificationObject, IConfirmableVM, ITextBoxController
    {
        private readonly IProviderConfiguration _configuration;

        public LoginVM(string lastLogin, IProviderConfiguration configuration)
        {
            Login = lastLogin;
            _configuration = configuration;
            ReadyToLoginCommand = new DelegateCommand(ReadyToLogin);
        }

        #region Binding Properties

        private string _login;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged(() => Login);
                }
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand ReadyToLoginCommand { get; protected set; }

        private void ReadyToLogin()
        {
            if (!string.IsNullOrEmpty(Login))
            {
                SelectElement("Password");
            }
        }

        #endregion

        #region ITextBoxController

        private void SelectElement(string key)
        {
            if (SelectText != null)
                SelectText(this, key);
        }

        public event SelectTextEventHandler SelectText;
        
        #endregion

        #region IConfirmable

        public void Confirm()
        {
            var auth = new Common.BL.Authentification.Authentificator(_configuration, new DataAccessLayerInitializer());
            var testConfResult = auth.AuthentificateUser(Login, Password);
            if (testConfResult)
            {
                IsConfirmed = true;
            }
            else
            {
                NoticeUser.ShowWarning(auth.ErrorMessage);
            }
        }

        private bool LoginAttempt(string login, string password)
        {
            _configuration.User = login;
            _configuration.Password = password;
            return TrudFacade.TestProviderConfiguration(_configuration);
        }

        public void ResetAfterFail()
        {
            Password = string.Empty;
            SelectElement("Login");
        }

        public bool IsConfirmed { get; private set; }
        
        #endregion
                

    }
}
