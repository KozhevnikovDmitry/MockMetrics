namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Перечисление типов вместимости условий поиска
    /// </summary>
    public enum ExpressionQuantity
    {
        /// <summary>
        /// Унарное условие.
        /// </summary>
        Single,

        /// <summary>
        /// Бинарное условие.
        /// </summary>
        Double,

        /// <summary>
        /// N-арное условние.
        /// </summary>
        Multiple
    }
}
