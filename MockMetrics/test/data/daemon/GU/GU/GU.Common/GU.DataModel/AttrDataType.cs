namespace GU.DataModel
{
    /// <summary>
    /// Перечисление упрощённых типов значений для атрибутов заявки.
    /// </summary>
    public enum AttrDataType
    {
        /// <summary>
        /// Строка
        /// </summary>
        String = 1,

        /// <summary>
        /// Число
        /// </summary>
        Number = 2,

        /// <summary>
        /// Дата
        /// </summary>
        Date = 3,
        
        /// <summary>
        /// Флаг
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// Файл
        /// </summary>
        File = 5,

        /// <summary>
        /// Справочное поле
        /// </summary>
        List = 6
    }
}