using System;

using Common.Types.Exceptions;

namespace Common.UI.Report.ReportException
{
    /// <summary>
    /// Класс исключений для ошибки "Ошибка загрузки отчёта.".
    /// </summary>
    public class ReportLoadingException : VMException
    {
        public ReportLoadingException(Exception ex)
            : base("Ошибка загрузки отчёта.", ex)
        {
            
        }
    }
}
