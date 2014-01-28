using System;

namespace Common.BL.ReportMapping
{
    /// <summary>
    /// Класс исключений для обработки ошибок "Ошибка при получении данных отчёта"
    /// </summary>
    public class ReportDataRetrieveException : Exception
    {
        public ReportDataRetrieveException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}