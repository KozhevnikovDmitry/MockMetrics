namespace GU.BL.Import.ImportException
{
    /// <summary>
    /// Класс исключение для ошибки "Невозможно найти ассоциированый элемент по тегу"
    /// </summary>
    public class SingleAttrValueTagMissingException : ImportTagException
    {
        /// <summary>
        /// Класс исключение для ошибки "Невозможно найти ассоциированый элемент по тегу".
        /// </summary>
        /// <param name="tag">Тег, по которому не нашлось</param>
        public SingleAttrValueTagMissingException(string tag)
            : base("Невозможно найти ассоциированый элемент по тегу {0}", tag)
        {

        }
    }
}