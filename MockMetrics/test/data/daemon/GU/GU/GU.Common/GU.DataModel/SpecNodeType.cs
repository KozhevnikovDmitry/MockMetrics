namespace GU.DataModel
{
    /// <summary>
    /// Перечисление типов элементов
    /// </summary>
    public enum SpecNodeType
    {
        /// <summary>
        /// Простой тип-значение
        /// </summary>
        Simple = 1,

        /// <summary>
        /// Внешний тип (с указанием reg_element_group_id)
        /// </summary>
        RefSpec = 2,

        /// <summary>
        /// Составной - все элементы (xsd:sequence)
        /// </summary>
        Complex = 3,

        /// <summary>
        /// Составной - один из элементов (xsd:choice)
        /// </summary>
        ComplexChoice = 4
    }
}