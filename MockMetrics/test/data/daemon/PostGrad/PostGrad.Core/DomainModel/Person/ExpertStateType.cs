namespace PostGrad.Core.DomainModel.Person
{
    /// <summary>
    /// Перечисление типов состояния эксперта
    /// </summary>
    public enum ExpertStateType
    {
        /// <summary>
        /// Физическое лицо
        /// </summary>
        Individual = 1,

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        Juridical = 2
    }
}