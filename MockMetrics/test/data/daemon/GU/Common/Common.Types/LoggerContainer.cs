using System;
using System.Configuration;
using System.IO;

namespace Common.Types
{
    /// <summary>
    /// Контейнер объектов поддержки логирования.
    /// </summary>
    /// <remarks>Может содержать один или два объекта <c>ILogger</c>. При наличии двух логгеров, второй действует как дублёр.</remarks>
    public static class LoggerContainer
    {
        /// <summary>
        /// Контейнер объектов поддержки логирования.
        /// </summary>
        static LoggerContainer()
        {
            _appName = ConfigurationManager.AppSettings["AppName"] ?? "GU.Common";
        }

        /// <summary>
        /// Имя приложения;
        /// </summary>
        private static string _appName;

        /// <summary>
        /// Помещает в контейнер объект <c>ILogger</c>.
        /// </summary>
        /// <param name="major">Oбъект <c>ILogger</c></param>
        public static void Initilize(ILogger major)
        {
            MajorLogger = major;
        }

        /// <summary>
        /// Помещает в контейнер объект <c>ILogger</c> и его дублёра.
        /// </summary>
        /// <param name="major">главный логгер</param>
        /// <param name="substitute">логгер дублёр</param>
        public static void Initilize(ILogger major, ILogger substitute)
        {
            SubstituteLogger = substitute;
            Initilize(major);
        }

        /// <summary>
        /// Главный, штатный логгер.
        /// </summary>
        public static ILogger MajorLogger
        {
            get;
            private set;
        }

        /// <summary>
        /// Логгер дублёр.
        /// </summary>
        public static ILogger SubstituteLogger
        {
            get;
            private set;
        }

        /// <summary>
        /// Логирует сообщение об ошибке с использованием заведённых логгеров.
        /// </summary>
        /// <param name="ex">Объект-исключение с информацией об ошибке</param>
        public static void LogError(Exception ex)
        {
            try
            {
                // логируем в главный логгер
                MajorLogger.Error(ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    // Логируем в логгер дублёр 
                    SubstituteLogger.Error(ex);
                    SubstituteLogger.Error(logEx);
                }
                catch (Exception subLogEx)
                {
                    BreakdownLogError(ex.ToString(), logEx, subLogEx);
                }
            }
        }

        /// <summary>
        /// Логирует сообщение с использованием заведённых логгеров.
        /// </summary>
        /// <param name="message">Логируемое сообщение</param>
        /// <param name="appLogType">Тип сообщения</param>
        public static void LogMessage(string message, AppLogType appLogType)
        {
            try
            { 
                // логируем в главный логгер
                MajorLogger.Log(message, appLogType);
            }
            catch (Exception logEx)
            {
                try
                {
                    // Логируем в логгер дублёр 
                    SubstituteLogger.Log(message, appLogType);
                    SubstituteLogger.Error(logEx);
                }
                catch (Exception subLogEx)
                {
                    BreakdownLogError(message, logEx, subLogEx);
                }
            }
        }

        /// <summary>
        /// Проводит аварийное логирование ошибок в случаях, когда штатные логгеры упали.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="logException">Ошибка логирования в главном логгере</param>
        /// <param name="subLogException">Ошибка логирования в логгере-дублёре</param>
        private static void BreakdownLogError(string message, Exception logException, Exception subLogException)
        {
            var folderAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folder = Path.Combine(folderAppData, _appName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var logPath = Path.Combine(folder, string.Format("{0}_error_log.txt", _appName));
            File.AppendAllLines(logPath, new[] { message, logException.ToString(), subLogException.ToString() });
        }
    }
}
