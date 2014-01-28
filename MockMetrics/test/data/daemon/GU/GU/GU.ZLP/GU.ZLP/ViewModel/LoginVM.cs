using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Principal;

using Common.DA;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Attachables.Properties;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.ZLP.ViewModel
{
    public class LoginVM : NotificationObject, IConfirmableVM, ITextBoxController
    {
        private IProviderConfiguration _configuration;

        public LoginVM(string lastLogin, IProviderConfiguration configuration)
        {
            Login = lastLogin;
            _configuration = configuration;
            ReadyToLoginCommand = new DelegateCommand(ReadyToLogin);
        }

        public string AgencyName { get; private set; }

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
            if (LoginAttempt(Login, Password))
            {
                IsConfirmed = true;
            }
            else
            {
                NoticeUser.ShowWarning("Неверный логин или пароль");
            }
        }

        private bool LoginAttempt(string login, string password)
        {
            try
            {
                _configuration.User = login;
                _configuration.Password = Password;
                bool result = GuFacade.TestProviderConfiguration(_configuration);
                // берем тип залепного приложения (ожидается agency_id)
                string val = ConfigurationManager.AppSettings["ZlpAppType"];
                int? zlpType = val == null ? null : (int?)Convert.ToInt32(val);

                if (result == true)
                {
                    DataAccessLayerInitializer init = new DataAccessLayerInitializer();
                    init.Initialize(_configuration, Assembly.UnsafeLoadFrom("GU.DataModel.dll"), new GuDomainObjectInitializer(WindowsIdentity.GetCurrent().Name));

                    // ZLP становится рудиментом
                    //if (!UserPolicy.IsDbUserValid(login, zlpType))return false;

                    using (var db = new GuDbManager())
                    {
                        AgencyName = (from a in db.GetTable<Agency>()
                                      where a.Id == zlpType
                                      select a.Name).SingleOrDefault();
                    }
                }
                return result;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
                return false;
            }
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
