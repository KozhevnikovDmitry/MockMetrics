using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.ViewModel;
using SpecManager.BL.Connect;
using SpecManager.BL.Interface;

namespace SpecManager.UI.ViewModel.SpecSourceViewModel
{
    public class DbConfigVm : NotificationObject
    {
        private readonly DbConnector _dbConnector;

        public DbConfigVm(DbConnector dbConnector)
        {
            this._dbConnector = dbConnector;
            this.IsConfig = true;
            this.IsCustom = false;
            this.ConfigList = this._dbConnector.ConfigNameList;
            this.Config = this.ConfigList.FirstOrDefault();
        }

        public string GetConnectionString()
        {
            if (this.IsConfig)
            {
                return this._dbConnector.GetConnectionsString(this.Config, this.User, this.Password);
            }

            return this._dbConnector.GetConnectionsString(this.Server,
                                                            this.Port,
                                                            this.Database,
                                                            this.User,
                                                            this.Password);
        }

        private void OnConfigNameChanged(string configName)
        {
            try
            {
                var connectConfig = _dbConnector.GetConnectConfig(configName);
                Server = connectConfig.Server;
                Database = connectConfig.Database;
                Port = connectConfig.Port;
                User = connectConfig.User;
                Password = connectConfig.Password;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #region Binding Properties

        private bool _isConfig;

        public bool IsConfig
        {
            get
            {
                return this._isConfig;
            }
            set
            {
                if (value.Equals(this._isConfig))
                {
                    return;
                }
                this._isConfig = value;
                this.RaisePropertyChanged(() => this.IsConfig);
            }
        }

        private bool _isCustom;

        public bool IsCustom
        {
            get
            {
                return this._isCustom;
            }
            set
            {
                if (value.Equals(this._isCustom))
                {
                    return;
                }
                this._isCustom = value;
                this.RaisePropertyChanged(() => this.IsCustom);
            }
        }

        private string _server;

        public string Server
        {
            get
            {
                return this._server;
            }
            set
            {
                if (value == this._server)
                {
                    return;
                }
                this._server = value;
                this.RaisePropertyChanged(() => this.Server);
            }
        }

        private string _database;

        public string  Database
        {
            get
            {
                return this._database;
            }
            set
            {
                if (value == this._database)
                {
                    return;
                }
                this._database = value;
                this.RaisePropertyChanged(() => this.Database);
            }
        }

        private string _port;

        public string Port
        {
            get
            {
                return this._port;
            }
            set
            {
                if (value == this._port)
                {
                    return;
                }
                this._port = value;
                this.RaisePropertyChanged(() => this.Port);
            }
        }

        private string _user;

        public string User
        {
            get
            {
                return this._user;
            }
            set
            {
                if (value == this._user)
                {
                    return;
                }
                this._user = value;
                this.RaisePropertyChanged(() => this.User);
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (value == this._password)
                {
                    return;
                }
                this._password = value;
                this.RaisePropertyChanged(() => this.Password);
            }
        }

        private string _config;

        public string Config
        {
            get
            {
                return this._config;
            }
            set
            {
                if (value != this._config)
                {
                    this._config = value;
                    this.RaisePropertyChanged(() => this.Config);
                    OnConfigNameChanged(this._config);
                }
            }
        }

        public List<string> ConfigList { get; set; }

        #endregion
    }
}
