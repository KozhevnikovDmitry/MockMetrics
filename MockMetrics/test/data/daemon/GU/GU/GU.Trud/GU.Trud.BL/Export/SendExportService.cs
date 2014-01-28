using System;
using System.Linq;
using Common.Types.Exceptions;
using GU.Trud.BL.Export.Interface;
using GU.Trud.BL.Mailing.Interface;
using GU.Trud.DataModel;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Класс служба отправки экспортных данных
    /// </summary>
    internal class SendExportService : ISendExportService
    {
        /// <summary>
        /// Объект доступа к почтовому клиенту.
        /// </summary>
        private readonly IMailClientInvoker _mailClientInvoker;

        /// <summary>
        /// Класс служба отправки экспортных данных
        /// </summary>
        /// <param name="mailClientInvoker">Объект доступа к почтовому клиенту</param>
        public SendExportService(IMailClientInvoker mailClientInvoker)
        {
            _mailClientInvoker = mailClientInvoker;
        }

        /// <summary>
        /// Отправляет данные выгрузки.
        /// </summary>
        /// <param name="taskExport">Объект выгрузка</param>
        public void SendExport(TaskExport taskExport)
        {
            try
            {
                var data = GetExportData(taskExport);
                _mailClientInvoker.OpenMailWithDefaultAccount("catharsis@catharsis.ru", taskExport.Filename, "", taskExport.Filename, data);
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка отправки экспортного файла", ex);
            }
        }

        /// <summary>
        /// Возвращает файл выгрузки.
        /// </summary>
        /// <param name="taskExport">Объект выгрузка</param>
        /// <returns></returns>
        private byte[] GetExportData(TaskExport taskExport)
        {
            try
            {
                using (var db = new TrudDbManager())
                {
                    return (from t in db.GetDomainTable<TaskExport>()
                            where t.Id == taskExport.Id
                            select t.Data).Single();
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка получения файла экспорта из БД", ex);
            }
        }
    }
}
