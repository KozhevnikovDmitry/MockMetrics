namespace GU.BL.Import.ImportException
{
    /// <summary>
    /// Класс исключение для ошибки "Неверная структура тегов. Найдено более одной ассоциации с таким тегом"
    /// </summary>
    public class TagWrongStructureException : ImportTagException
    {
        /// <summary>
        /// Класс исключение для ошибки "Невозможно найти ассоциированый элемент по тегу".
        /// </summary>
        /// <param name="tag">Тег, по которому не нашлось</param>
        public TagWrongStructureException(string tag)
            : base("Неверная структура тегов {0}. Найдено более одной ассоциации с таким тегом", tag)
        {

        }
    }
}