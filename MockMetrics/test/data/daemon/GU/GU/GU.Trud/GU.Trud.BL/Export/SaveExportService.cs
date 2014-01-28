using System;
using System.IO;
using System.Linq;

using Common.Types.Exceptions;

using GU.Trud.BL.Export.Interface;
using GU.Trud.DataModel;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Cлужба сохранения экспортных данных.
    /// </summary>
    public class SaveExportService : ISaveExportService
    {
        public void SaveExport(TaskExport taskExport, string path)
        {
            try
            {
                using (var db = new TrudDbManager())
                {
                    var file = (from t in db.GetDomainTable<TaskExport>()
                                where t.Id == taskExport.Id
                                select t.Data).Single();

                    File.WriteAllBytes(path, file);
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка сохранения файла экспорта из БД", ex);
            }
        }
    }
}
