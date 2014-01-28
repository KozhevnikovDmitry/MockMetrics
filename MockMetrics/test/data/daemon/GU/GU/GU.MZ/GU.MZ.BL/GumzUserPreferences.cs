using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

using Common.BL;
using Common.BL.Search.SearchSpecification;
using Common.Types.Exceptions;

using GU.MZ.BL.Search;

namespace GU.MZ.BL
{
    /// <summary>
    /// Класс, предназначенный для управления пользовательскими настройками приложения МинЗдрава.
    /// </summary>
    public class GumzUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Класс, предназначенный для управления пользовательскими настройками приложения МинЗдрава.
        /// </summary>
        public GumzUserPreferences()
        {
            _configDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"GU.MZ");
            _сonfigurationPath = Path.Combine(_configDirectoryPath, "GumzConfig.xml");
            _presetConfigurationPath = Path.Combine(_configDirectoryPath, "Presets.xml");
            LoadSettings();
            LoadPresets();
        }

        #region SearchPresets

        /// <summary>
        /// Загружает пользовательские настройки из хранилища.
        /// </summary>
        public void LoadPresets()
        {
            try
            {
                SearchPresetSpecContainer = new GumzSearchPresetSpecContainer();

                if (File.Exists(_presetConfigurationPath))
                {
                    using (var stream = new FileStream(_presetConfigurationPath, FileMode.Open))
                    {
                        var sf = new BinaryFormatter();
                        var searchPresetSpecContainer = (SearchPresetSpecContainer)sf.Deserialize(stream);

                        if( SearchPresetSpecContainer.LastUpdate > searchPresetSpecContainer.LastUpdate)
                        {
                            SavePresets();
                        }
                        else
                        {
                            SearchPresetSpecContainer = searchPresetSpecContainer;
                        }
                    }
                }
                else
                {
                    SavePresets();
                }
            }
            catch (Exception)
            {
                File.Delete(_сonfigurationPath);
                SearchPresetSpecContainer = new GumzSearchPresetSpecContainer();
                SavePresets();
            }  
        }

        /// <summary>
        /// Сохраняет пользовательские настройки в хранилище.
        /// </summary>
        public void SavePresets()
        {
            try
            {
                Directory.CreateDirectory(_configDirectoryPath);
                using (var stream = new FileStream(Path.Combine(_configDirectoryPath, "Presets.xml"), FileMode.Create))
                {
                    var sf = new BinaryFormatter();
                    sf.Serialize(stream, SearchPresetSpecContainer);
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
        private GumzUserPreferencesContainer _preferencesContainer;

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
            _preferencesContainer = new GumzUserPreferencesContainer();
        }

        /// <summary>
        /// Загружает пользовательские настройки из хранилища.
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_сonfigurationPath))
                {
                    using (var stream = new FileStream(_сonfigurationPath, FileMode.Open))
                    {
                        BinaryFormatter sf = new BinaryFormatter();
                        _preferencesContainer = (GumzUserPreferencesContainer)sf.Deserialize(stream);
                    }
                }
                else
                {
                    SetDefaultSettings();
                }
            }
            catch (XmlException)
            {
                File.Delete(_сonfigurationPath);
                SetDefaultSettings();
            }
            catch (SerializationException)
            {
                SetDefaultSettings();
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
                Directory.CreateDirectory(_configDirectoryPath);
                using (var stream = new FileStream(_сonfigurationPath, FileMode.Create))
                {
                    BinaryFormatter sf = new BinaryFormatter();
                    sf.Serialize(stream, _preferencesContainer);
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
                return _preferencesContainer.LastLogin;
            }
            set
            {
                _preferencesContainer.LastLogin = value;
            }
        }

        /// <summary>
        /// Количество доменных объектов размещаемых на одной странице поиска.
        /// </summary>
        public int LoadingRowCount
        {
            get
            {
                return _preferencesContainer.LoadingRowCount;
            }
            set
            {
                _preferencesContainer.LoadingRowCount = value;
            }
        }

        /// <summary>
        /// Уровень валидации.
        /// </summary>
        public int ValidationLevel
        {
            get
            {
                return _preferencesContainer.ValidationLevel;
            }
            set
            {
                _preferencesContainer.ValidationLevel = value;
            }
        }

        #endregion

    }

    /// <summary>
    /// Класс, предназначенный для хранения пользовательских настроек приложения МинЗдрава.
    /// </summary>
    [Serializable]
    internal class GumzUserPreferencesContainer
    {
        /// <summary>
        /// Класс, предназначенный для хранения пользовательских настроек приложения МинЗдрава.
        /// </summary>
        public GumzUserPreferencesContainer()
        {
            LoadingRowCount = 20;
            ValidationLevel = 1;
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
