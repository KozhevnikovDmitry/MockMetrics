
using Common.BL.Search.SearchSpecification;

namespace Common.BL
{
    /// <summary>
    /// Интерфейс классов, предназначенных для управления пользовательскими настройками приложения.
    /// </summary>
    public interface IUserPreferences
    {
        /// <summary>
        /// Устанавливает пользовательские настройки по умолчанию.
        /// </summary>
        void SetDefaultSettings();

        /// <summary>
        /// Загружает пользовательские настройки из хранилища.
        /// </summary>
        void LoadSettings();

        /// <summary>
        /// Сохраняет пользовательские настроки в хранилище.
        /// </summary>
        void SaveSettings();
        
        /// <summary>
        /// Контейнер пресетов
        /// </summary>
        SearchPresetSpecContainer SearchPresetSpecContainer { get; }
    }
}
