namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Перечисление статусов постоянства POCO-объектов.
    /// </summary>
    public enum PersistentState
    {
        /// <summary>
        /// Новый
        /// </summary>
        New = 0,

        /// <summary>
        /// Старый
        /// </summary>
        Old = 1,

        /// <summary>
        /// Новый удалённый
        /// </summary>
        NewDeleted = 2,

        /// <summary>
        /// Старый удалённый
        /// </summary>
        OldDeleted = 3,
    }
}