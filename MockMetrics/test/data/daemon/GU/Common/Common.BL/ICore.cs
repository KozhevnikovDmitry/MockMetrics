using Common.BL.DictionaryManagement;

namespace Common.BL
{
    /// <summary>
    /// Интерфейс классов, отвечающих за хостинг синглтонов бизнес-логики в предметной области.
    /// </summary>
    public interface ICore
    {
        /// <summary>
        /// Вовзвращает менеджер справочников.
        /// </summary>
        IDictionaryManager DictionaryManager { get; }

        /// <summary>
        /// Возвращает Пользовательские настройки.
        /// </summary>
        IUserPreferences UserPreferences { get; }
    }
}
