using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using Common.BL;
using Common.BL.Search.SearchSpecification;
using Common.Types.Exceptions;

namespace GU.Trud.BL
{
    /// <summary>
    /// Класс, предназначенный для управления пользовательскими настройками приложения МинЗдрава.
    /// </summary>
    public class TrudUserPreferences : IUserPreferences
    {
        /// <summary>
        /// Класс, предназначенный для управления пользовательскими настройками приложения Труда.
        /// </summary>
        public TrudUserPreferences()
        {
            _configDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                                   + "\\GU.Trud";
            _сonfigurationPath = _configDirectoryPath + "\\TrudConfig.xml";
            LoadSettings();
        }

        #region Private

        /// <summary>
        /// Контейнер настроек.
        /// </summary>
        private TrudUserPreferencesContainer _preferencesContainer;

        /// <summary>
        /// Путь к каталогу хранения настроек
        /// </summary>
        private readonly string _configDirectoryPath;

        /// <summary>
        /// Возвращает путь к хранилищу настроек.
        /// </summary>
        private readonly string _сonfigurationPath;

        #endregion        

        #region IUserPreferences

        /// <summary>
        /// Устанавливает пользовательские настройки по умолчанию.
        /// </summary>
        public void SetDefaultSettings()
        {
            _preferencesContainer = new TrudUserPreferencesContainer();
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
                        var sf = new BinaryFormatter();
                        _preferencesContainer = (TrudUserPreferencesContainer)sf.Deserialize(stream);
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

        public SearchPresetSpecContainer SearchPresetSpecContainer { get; private set; }

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
    internal class TrudUserPreferencesContainer
    {
        /// <summary>
        /// Класс, предназначенный для хранения пользовательских настроек приложения МинЗдрава.
        /// </summary>
        public TrudUserPreferencesContainer()
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
