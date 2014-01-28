namespace GU.Trud.BL.Mailing.Interface
{
    /// <summary>
    /// Интерфейс для доступа к почтовому клиенту
    /// </summary>
    public interface IMailClientInvoker
    {
        /// <summary>
        /// Открывает диалог отправки письма в почтовом клиенте по умолчанию.
        /// </summary>
        /// <param name="to">Кому</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело письма</param>
        /// <param name="attachmentName">Имя вложения</param>
        /// <param name="attachment">Вложение</param>
        void OpenMailWithDefaultAccount(string to,
                                        string subject,
                                        string body,
                                        string attachmentName,
                                        byte[] attachment);
    }
}
