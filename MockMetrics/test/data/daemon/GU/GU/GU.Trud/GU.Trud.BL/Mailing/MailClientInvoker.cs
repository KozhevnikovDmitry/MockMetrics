using System;
using System.IO;
using System.Linq;
using Common.Types.Exceptions;
using GU.Trud.BL.Mailing.Interface;
using GU.Trud.BL.Mailing.MapiMail;

namespace GU.Trud.BL.Mailing
{
    /// <summary>
    /// Класс для доступа к почтовому клиенту.
    /// </summary>
    internal class MailClientInvoker : IMailClientInvoker
    {
        private readonly string _operatingCatalog;

        /// <summary>
        /// Класс для доступа к почтовому клиенту.
        /// </summary>
        /// <param name="operatingCatalog">Операционный каталог для работы с вложениями</param>
        public MailClientInvoker(string operatingCatalog)
        {
            _operatingCatalog = operatingCatalog;
        }

        /// <summary>
        /// Открывает диалог отправки письма в почтовом клиенте по умолчанию.
        /// </summary>
        /// <param name="to">Кому</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело письма</param>
        /// <param name="attachmentName">Имя вложения</param>
        /// <param name="attachment">Вложение</param>
        public void OpenMailWithDefaultAccount(string to, 
                                               string subject, 
                                               string body, 
                                               string attachmentName, 
                                               byte[] attachment)
        {
            try
            {
                if (!Directory.Exists(_operatingCatalog))
                {
                    var dirInfo = Directory.CreateDirectory(_operatingCatalog);
                    dirInfo.Attributes = FileAttributes.Hidden;
                }
                else
                {
                    ClearOperatingCatalog();
                }
                var message = new MapiMailMessage(subject, body);
                message.Recipients.Add(to);
                File.WriteAllBytes(Path.Combine(_operatingCatalog, attachmentName), attachment);
                message.Files.Add(Path.Combine(_operatingCatalog, attachmentName));
                message.ShowDialog();
            }
            catch (GUException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new BLLException("Ошибка отправки письма в почтовом клиенте пользователя", ex);
            }
        }
        
        /// <summary>
        /// Очищает каталог от файлов.
        /// </summary>
        /// <param name="path">Путь к каталогов</param>
        private void ClearOperatingCatalog()
        {
            Directory.GetFiles(_operatingCatalog).ToList().ForEach(File.Delete);
        }
    }
}
