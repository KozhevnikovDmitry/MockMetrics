using System;
using System.Windows.Controls;

using Common.UI.Report.ReportException;
using Common.UI.View;
using Common.UI.ViewModel.Report;

using Stimulsoft.Report;

namespace Common.UI.Report
{
    /// <summary>
    /// Класс, отвечающий за создание View для отображения отчётов StimulSoft Reports
    /// </summary>
    internal class StiReportEngine : IReportEngine
    {
        /// <summary>
        /// Вовзвращает View отображения отчёта.
        /// </summary>
        /// <typeparam name="T">Тип данных для отчёта</typeparam>
        /// <param name="reportFilePath">Путь к файлу отчёта</param>
        /// <param name="dataName">Имя для сущности с данными</param>
        /// <param name="data">Данные для отчёта</param>
        /// <param name="isDesigner">Флаг указывающий на необходимость открытия отчёта в режиме дизайнера</param>
        /// <returns>View отображения отчёта</returns>
        public UserControl GetReportPresenter<T>(string reportFilePath, string dataName, T data, bool isDesigner)
        {
            var report = LoadReport(reportFilePath, dataName, data);
            return new ReportView { DataContext = new StimulReportVM(report, isDesigner) };
        }

        /// <summary>
        /// Загружает и возвращает StimulSoft отчёт.
        /// </summary>
        /// <typeparam name="T">Тип данных для отчёта</typeparam>
        /// <param name="reportFilePath">Путь к файлу отчёта</param>
        /// <param name="dataName">Имя для сущности с данными</param>
        /// <param name="data">Данные для отчёта</param>
        /// <returns>StimulSoft отчёт</returns>
        /// <exception cref="ReportLoadingException">Транзакция уже была зафиксирована или отменена.</exception>
        private StiReport LoadReport<T>(string reportFilePath, string dataName, T data)
        {
            try
            {
                var report = new StiReport();
                report.Load(reportFilePath);
                report.RegBusinessObject("Report", dataName, data);
                report.Compile();
                report.Render();
                return report;
            }
            catch (Exception ex)
            {
                throw new ReportLoadingException(ex);
            }
        }
    }
}
