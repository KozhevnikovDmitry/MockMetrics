using GU.Trud.DataModel;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс службы сохранения экспортных данных.
    /// </summary>
    public interface ISaveExportService
    {
        /// <summary>
        /// Сохраняет данные выгрузки.
        /// </summary>
        /// <param name="taskExport">Объекта выгрузка</param>
        /// <param name="path">Путь для сохранения</param>
        void SaveExport(TaskExport taskExport, string path);
    }
}
