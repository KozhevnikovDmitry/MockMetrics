using System;

namespace Common.DA
{
    /// <summary>
    /// Класс, представляющий информацию о тесте конфигурации подключения к базе данных
    /// </summary>
    public class ConfigurationTestResult
    {
        /// <summary>
        /// Флаг успешности подключения
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Сообщение об ошибке подключения
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Исключение с ошибкой подключения
        /// </summary>
        public Exception Exception { get; set; }
    }
}
