using System.ComponentModel;

namespace SpecManager.BL.Model
{
    /// <summary>
    /// Перечисление упрощённых типов значений для атрибутов заявки.
    /// </summary>
    public enum AttrDataType
    {
        /// <summary>
        /// Строка
        /// </summary>
        [Description("Строка")]
        String = 1,

        /// <summary>
        /// Число
        /// </summary>
        [Description("Число")]
        Number = 2,

        /// <summary>
        /// Дата
        /// </summary>
        [Description("Дата")]
        Date = 3,
        
        /// <summary>
        /// Флаг
        /// </summary>
        [Description("Флаг")]
        Boolean = 4,

        /// <summary>
        /// Файл
        /// </summary>
        [Description("Файл")]
        File = 5,

        /// <summary>
        /// Справочное поле
        /// </summary>
        [Description("Справочное поле")]
        List = 6
    }
}