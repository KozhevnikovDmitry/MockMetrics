using Common.Types.Exceptions;
using GU.Trud.BL.Export.Interface;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Класс представляющий результаты экспорта.
    /// </summary>
    internal class ExportResult : IExportResult
    {
        /// <summary>
        /// Класс представляющий результаты экспорта.
        /// </summary>
        public ExportResult()
        {
            IsFailed = false;
        }

        /// <summary>
        /// Класс представляющий результаты экспорта.
        /// </summary>
        /// <param name="exception">Объект исключение с информацией об ошибке</param>
        public ExportResult(GUException exception)
        {
            Exception = exception;
            IsFailed = true;
        }

        /// <summary>
        /// Возвращает флаг ошибочного завершения работы.
        /// </summary>
        public bool IsFailed { get; private set; }

        /// <summary>
        /// Объект исключение с информацией об ошибке.
        /// </summary>
        public GUException Exception { get; private set; }
    }
}
