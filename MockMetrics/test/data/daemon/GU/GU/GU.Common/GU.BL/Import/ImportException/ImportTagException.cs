using Common.Types.Exceptions;
using GU.DataModel;

namespace GU.BL.Import.ImportException
{
    /// <summary>
    /// Базовый класс исключение для классов описывающих ошибки импорта данных заявки по тегам. 
    /// </summary>
    public class ImportTagException : GUException
    {
        public ContentNode ContentNode { get; set; }
        public Content Content { get; set; }

        public string Tag { get; set; }

        protected ImportTagException(string message, string tag)
            :base(string.Format(message, tag))
        {
            Tag = tag;
        }

        public ImportTagException(Content content, ImportTagException innerException)
            : base("Ошибка импорта данных из контента заявки", innerException)
        {
            Content = content;
        }

        public ImportTagException(ContentNode contentNode, ImportTagException innerException)
            : base("Ошибка импорта данных из ноды контента заявки", innerException)
        {
            ContentNode = contentNode;
        }
    }
}