using System.Windows.Controls;

namespace Common.UI.Report
{
    /// <summary>
    /// Интерфейс для классов формирующих View отображения отчётов
    /// </summary>
    public interface IReportEngine
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
        UserControl GetReportPresenter<T>(string reportFilePath, string dataName, T data, bool isDesigner);
    }
}
