using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI;
using Common.UI.ViewModel.Attachables.Properties;
using Common.UI.ViewModel.Interfaces;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.UI.ViewModel.UserViewModel
{
    public class SetPasswordVM : NotificationObject, IConfirmableVM, ITextBoxController
    {
        #region Binding Properties

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

        private string _confirmPassword;

        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    RaisePropertyChanged(() => ConfirmPassword);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion

        #region ITextBoxController

        private void SelectElement(string key)
        {
            if (SelectText != null)
                SelectText(this, key);
        }

        public event SelectTextEventHandler SelectText;

        #endregion

        #region Implementation of IConfirmableVM

        public void Confirm()
        {
            if (Password != string.Empty && Password == ConfirmPassword)
            {
                IsConfirmed = true;
            }
            else
            {
                NoticeUser.ShowWarning("Пароли не совпадают");
            }
        }

        public void ResetAfterFail()
        {
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            SelectElement("Password");
        }

        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
