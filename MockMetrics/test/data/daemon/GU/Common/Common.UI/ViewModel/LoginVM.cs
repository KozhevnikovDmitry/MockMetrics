using System;
using Common.BL;
using Common.BL.Authentification;
using Common.Types.Exceptions;
using Common.UI.ViewModel.Attachables.Properties;
using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    /// <summary>
    /// Класс VM для окна аутентификации пользователя.
    /// </summary>
    public class LoginVM : NotificationObject, IConfirmableVM, ITextBoxController
    {
        /// <summary>
        /// Объект отвечающий за аутентификацию пользователя.
        /// </summary>
        private readonly IAuthentificator _authentificator;

        private readonly ConnectChecker _connectChecker;

        /// <summary>
        /// Класс VM для окна аутентификации пользователя.
        /// </summary>
        public LoginVM(IAuthentificator authentificator, ConnectChecker connectChecker)
        {
            _authentificator = authentificator;
            _connectChecker = connectChecker;
            this.Password = string.Empty;
            this.ReadyToLoginCommand = new DelegateCommand(this.ReadyToLogin);
        }

        #region Binding Properties

        /// <summary>
        /// Логин.
        /// </summary>
        private string _login;

        /// <summary>
        /// Вовзращает иили устанваливает значение логина.
        /// </summary>
        public string Login
        {
            get
            {
                return this._login;
            }
            set
            {
                if (this._login != value)
                {
                    this._login = value;
                    this.RaisePropertyChanged(() => this.Login);
                }
            }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        private string _password;

        /// <summary>
        /// Возвращает или устанваливает значение пароля.
        /// </summary>
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.RaisePropertyChanged(() => this.Password);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда подготовки окна аутентификации к вводу данных.
        /// </summary>
        public DelegateCommand ReadyToLoginCommand { get; protected set; }

        /// <summary>
        /// Переводит фаокус ввода на пустое поле(начала логин, потом пароль).
        /// </summary>
        private void ReadyToLogin()
        {
            if (!string.IsNullOrEmpty(this.Login))
            {
                this.SelectElement("Password");
            }
        }

        #endregion

        #region ITextBoxController

        /// <summary>
        /// Переводит фокус ввода на элемент по ключу
        /// </summary>
        /// <param name="key">Ключ элемента</param>
        private void SelectElement(string key)
        {
            if (this.SelectText != null)
            {
                this.SelectText(this, key);
            }
        }

        /// <summary>
        /// Событие оповещающее о необходимости передачи фокуса ввода элементу.
        /// </summary>
        public event SelectTextEventHandler SelectText;

        #endregion

        #region IConfirmable

        public void Confirm()
        {
            try
            {
                if (_connectChecker.Check())
                {
                    if (LoginAttempt(Login, Password))
                    {
                        IsConfirmed = true;
                    }
                    else
                    {
                        NoticeUser.ShowWarning(_authentificator.ErrorMessage);
                    }
                }
                else
                {
                    NoticeUser.ShowWarning("Отсутствует подключение к базе данных");
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        /// <summary>
        /// Выполняет попытку аутентификации пользователя.
        /// </summary>
        /// <param name="login">Значение логина</param>
        /// <param name="password">Значение пароля</param>
        /// <returns>Флаг успешности аутентификации</returns>
        private bool LoginAttempt(string login, string password)
        {
            try
            {
                return _authentificator.AuthentificateUser(login, password);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
        }

        /// <summary>
        /// Восстанавливает настройки после неудачной попытки аутентификации.
        /// </summary>
        public void ResetAfterFail()
        {
            this.Password = string.Empty;
            this.SelectElement("Login");
        }

        /// <summary>
        /// Возвращает флаг завершённости работы окна.
        /// </summary>
        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
