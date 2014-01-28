using GU.Trud.DataModel;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс службы отправки экспортных данных.
    /// </summary>
    public interface ISendExportService
    {
        /// <summary>
        /// Отправляет данные выгрузки.
        /// </summary>
        /// <param name="taskExport">Объекта выгрузка</param>
        void SendExport(TaskExport taskExport);
    }
}
