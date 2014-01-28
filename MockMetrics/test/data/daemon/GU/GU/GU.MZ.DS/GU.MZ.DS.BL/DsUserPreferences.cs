using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

using Common.BL;
using Common.BL.Search.SearchSpecification;
using Common.Types.Exceptions;

using GU.MZ.DS.BL.Search;

namespace GU.MZ.DS.BL
{
    /// <summary>
    /// Класс, предназначенный для управления пользовательскими настройками приложения Лекарственное обеспечение.
    /// </summary>
    public class DsUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Класс, предназначенный для управления пользовательскими настройками приложения Лекарственное обеспечение.
        /// </summary>
        public DsUserPreferences()
        {
            this._configDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GU.MZ.DS");
            this._сonfigurationPath = Path.Combine(this._configDirectoryPath, "DsConfig.xml");
            this._presetConfigurationPath = Path.Combine(this._configDirectoryPath, "Presets.xml");
            this.LoadSettings();
            this.LoadPresets();
        }

        #region SearchPresets

        /// <summary>
        /// Загружает пользовательские настройки из хранилища.
        /// </summary>
        public void LoadPresets()
        {
            try
            {
                this.SearchPresetSpecContainer = new DsSearchPresetSpecContainer();

                if (File.Exists(this._presetConfigurationPath))
                {
                    using (var stream = new FileStream(this._presetConfigurationPath, FileMode.Open))
                    {
                        var sf = new BinaryFormatter();
                        var searchPresetSpecContainer = (SearchPresetSpecContainer)sf.Deserialize(stream);

                        if( this.SearchPresetSpecContainer.LastUpdate > searchPresetSpecContainer.LastUpdate)
                        {
                            this.SavePresets();
                        }
                        else
                        {
                            this.SearchPresetSpecContainer = searchPresetSpecContainer;
                        }
                    }
                }
                else
                {
                    this.SavePresets();
                }
            }
            catch (Exception)
            {
                File.Delete(this._сonfigurationPath);
                this.SearchPresetSpecContainer = new DsSearchPresetSpecContainer();
                this.SavePresets();
            }  
        }

        /// <summary>
        /// Сохраняет пользовательские настройки в хранилище.
        /// </summary>
        public void SavePresets()
        {
            try
            {
                Directory.CreateDirectory(this._configDirectoryPath);
                using (var stream = new FileStream(Path.Combine(this._configDirectoryPath, "Presets.xml"), FileMode.Create))
                {
                    var sf = new BinaryFormatter();
                    sf.Serialize(stream, this.SearchPresetSpecContainer);
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("SearchPresetSpecContainer save operation failed", ex);
            }
        }

        /// <summary>
        /// Контейнер пресетов
        /// </summary>
        public SearchPresetSpecContainer SearchPresetSpecContainer { get; private set; }

        #endregion

        #region Private

        /// <summary>
        /// Контейнер настроек.
        /// </summary>
        private DsUserPreferencesContainer _preferencesContainer;

        /// <summary>
        /// Путь к каталогу хранения настроек
        /// </summary>
        private readonly string _configDirectoryPath;

        /// <summary>
        /// Путь к хранилищу настроек.
        /// </summary>
        private readonly string _сonfigurationPath;

        /// <summary>
        /// Путь к хранилищу пресетов поиска.
        /// </summary>
        private readonly string _presetConfigurationPath;

        #endregion        

        #region IUserPreferences

        /// <summary>
        /// Устанавливает пользовательские настройки по умолчанию.
        /// </summary>
        public void SetDefaultSettings()
        {
            this._preferencesContainer = new DsUserPreferencesContainer();
        }

        /// <summary>
        /// Загружает пользовательские настройки из хранилища.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                if (File.Exists(this._сonfigurationPath))
                {
                    using (var stream = new FileStream(this._сonfigurationPath, FileMode.Open))
                    {
                        BinaryFormatter sf = new BinaryFormatter();
                        this._preferencesContainer = (DsUserPreferencesContainer)sf.Deserialize(stream);
                    }
                }
                else
                {
                    this.SetDefaultSettings();
                }
            }
            catch (XmlException)
            {
                File.Delete(this._сonfigurationPath);
                this.SetDefaultSettings();
            }
            catch (SerializationException)
            {
                this.SetDefaultSettings();
            }
            catch (Exception ex)
            {
                throw new BLLException("User preferences load operation failed", ex);
            }
        }

        /// <summary>
        /// Сохраняет пользовательские настроки в хранилище.
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(this._configDirectoryPath);
                using (var stream = new FileStream(this._сonfigurationPath, FileMode.Create))
                {
                    BinaryFormatter sf = new BinaryFormatter();
                    sf.Serialize(stream, this._preferencesContainer);
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("User preferences save operation failed", ex);
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Логин из последней удачной аутентификации.
        /// </summary>
        public string LastLogin 
        {
            get
            {
                return this._preferencesContainer.LastLogin;
            }
            set
            {
                this._preferencesContainer.LastLogin = value;
            }
        }

        /// <summary>
        /// Количество доменных объектов размещаемых на одной странице поиска.
        /// </summary>
        public int LoadingRowCount
        {
            get
            {
                return this._preferencesContainer.LoadingRowCount;
            }
            set
            {
                this._preferencesContainer.LoadingRowCount = value;
            }
        }

        /// <summary>
        /// Уровень валидации.
        /// </summary>
        public int ValidationLevel
        {
            get
            {
                return this._preferencesContainer.ValidationLevel;
            }
            set
            {
                this._preferencesContainer.ValidationLevel = value;
            }
        }

        #endregion

    }

    /// <summary>
    /// Класс, предназначенный для хранения пользовательских настроек приложения Лекарственное обеспечение.
    /// </summary>
    [Serializable]
    internal class DsUserPreferencesContainer
    {
        /// <summary>
        /// Класс, предназначенный для хранения пользовательских настроек приложения Лекарственное обеспечение.
        /// </summary>
        public DsUserPreferencesContainer()
        {
            this.LoadingRowCount = 20;
            this.ValidationLevel = 1;
        }

        /// <summary>
        /// Логин из последней удачной аутентификации.
        /// </summary>
        public string LastLogin { get; set; }

        /// <summary>
        /// Количество доменных объектов размещаемых на одной странице поиска.
        /// </summary>
        public int LoadingRowCount { get; set; }

        /// <summary>
        /// Уровень валидации.
        /// </summary>
        public int ValidationLevel { get; set; }


    }
}
