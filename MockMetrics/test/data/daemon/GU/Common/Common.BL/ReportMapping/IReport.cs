namespace Common.BL.ReportMapping
{
    /// <summary>
    /// Интерфейс для классов, инкапсулирующих отчёт
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Путь к файлу с версткой отчёта
        /// </summary>
        string ViewPath { get; }

        /// <summary>
        /// Псевдоним данных в отчёте
        /// </summary>
        string DataAlias { get; }

        /// <summary>
        /// Возвращает данные для отчта
        /// </summary>
        /// <returns>Данные для отчёта</returns>
        object RetrieveData();
    }
}
