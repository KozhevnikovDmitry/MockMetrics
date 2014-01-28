namespace Common.Types
{
    /// <summary>
    /// Тип сообщения лога
    /// </summary>
    public enum AppLogType
    {
        /// <summary>
        /// Критическая ошибка
        /// </summary>
        FatalError = 1,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 2,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Информация
        /// </summary>
        Info = 4
    }
}
