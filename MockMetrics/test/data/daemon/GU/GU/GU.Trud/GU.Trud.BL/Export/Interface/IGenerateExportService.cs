using Common.DA.Interface;
using GU.Trud.BL.Export.Event;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс для классов-служб экспорта данных.
    /// </summary>
    public interface IGenerateExportService
    {
        /// <summary>
        /// Выполняет экспорт данных.
        /// </summary>
        /// <param name="dbManager">Менеджер данных</param>
        void ExportData(IDomainDbManager dbManager);

        /// <summary>
        /// Событие оповещающее о прогрессе процесса экспорта.
        /// </summary>
        event PercentageProgressDelegate ExportProgressed;

        /// <summary>
        /// Инициирует отмену экспорта данных.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Результат экспорта.
        /// </summary>
        IExportResult ExportResult { get; }
    }
}