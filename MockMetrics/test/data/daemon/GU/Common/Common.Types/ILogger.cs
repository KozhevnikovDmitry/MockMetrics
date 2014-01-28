using System;

namespace Common.Types
{
    /// <summary>
    /// Интерфейс класса, предназначенного для логирования ошибок и сообщений.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Логирует информацию об исключении.
        /// </summary>
        /// <param name="ex">Логируемое исключение</param>
        void Error(Exception ex);

        /// <summary>
        /// Логирует информацию об исключении и пользовательское сообщение.
        /// </summary>
        /// <param name="message">Сообщение, отображаемое пользователю</param>
        /// <param name="ex">Логируемое исключение</param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Логирует информацию об исключении, пользовательское сообщение с указанием типа сообщения.
        /// </summary>
        /// <param name="message">Сообщение, отображаемое пользователю</param>
        /// <param name="ex">Логируемое исключение</param>
        /// <param name="appLogType">Тип сообщения</param>
        void Error(string message, Exception ex, AppLogType appLogType);

        /// <summary>
        /// Логирует текствое сообщение.
        /// </summary>
        /// <param name="logMessage">Сообщение</param>
        void Log(string logMessage);

        /// <summary>
        /// Логирует текстовое сообщение с указанием его типа.
        /// </summary>
        /// <param name="logMessage">Сообщение</param>
        /// <param name="appLogType">Тип сообщения</param>
        void Log(string logMessage, AppLogType appLogType);

        /// <summary>
        /// Логирует текстовое сообщение, дополнительное сообщение с указанием типа.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="additionalMessage">Дополнительное сообщение</param>
        /// <param name="appLogType">Тип сообщения</param>
        void Log(string message, string additionalMessage, AppLogType appLogType);
    }
}
