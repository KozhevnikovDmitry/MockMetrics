using System.Collections.Generic;
using GU.Trud.BL.Export.DataModel;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс классов осуществляющих выгрузку данных в файл.
    /// </summary>
    internal interface IToFileDumper
    {
        /// <summary>
        /// Осуществляет выгрузку данных в файл.
        /// </summary>
        /// <param name="data">Данные для выгрузки</param>
        /// <param name="filename">Имя файла</param>
        void Dump(List<IToItemArray> data, string filename);

        /// <summary>
        /// Отменяет процедуру выгрузки данных.
        /// </summary>
        void CancelDump();
    }
}
