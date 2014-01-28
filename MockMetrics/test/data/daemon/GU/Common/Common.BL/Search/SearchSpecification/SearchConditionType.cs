namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Перечисление типов условий поиска
    /// </summary>
    public enum SearchConditionType
    {
        /// <summary>
        /// Равно, является, совпадает
        /// </summary>
        Equals,

        /// <summary>
        /// Не равно, не является, не совпадает
        /// </summary>
        NotEquals,

        /// <summary>
        /// Начинается с
        /// </summary>
        StartsWith,

        /// <summary>
        /// Оканчивается на
        /// </summary>
        EndsWith,

        /// <summary>
        /// Включает в себя
        /// </summary>
        Includes,

        /// <summary>
        /// Больше, позднее
        /// </summary>
        More,

        /// <summary>
        /// Меньше, раньше
        /// </summary>
        Less,

        /// <summary>
        /// В пределах
        /// </summary>
        Range
    }
}
