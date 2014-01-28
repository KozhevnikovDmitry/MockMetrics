using System.ComponentModel;

namespace SpecManager.BL.Model
{
    /// <summary>
    /// Перечисление типов элементов
    /// </summary>
    public enum SpecNodeType
    {
        /// <summary>
        /// Простой тип-значение
        /// </summary>
        [Description("Простой тип-значение")]
        Simple = 1,

        /// <summary>
        /// Внешний тип (с указанием reg_element_group_id)
        /// </summary>
        [Description("Внешний")]
        RefSpec = 2,

        /// <summary>
        /// Составной - все элементы (xsd:sequence)
        /// </summary>
        [Description("Составной")]
        Complex = 3,

        /// <summary>
        /// Составной - один из элементов (xsd:choice)
        /// </summary>
        [Description("Выбор")]
        ComplexChoice = 4
    }
}