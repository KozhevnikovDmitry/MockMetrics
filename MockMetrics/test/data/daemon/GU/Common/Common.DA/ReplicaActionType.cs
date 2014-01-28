namespace Common.DA
{
    /// <summary>
    /// Перечисление типов действий репликации
    /// </summary>
    public enum ReplicaActionType
    {
        /// <summary>
        /// Вставка 
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Обновление
        /// </summary>
        Update = 2,

        /// <summary>
        /// Удаление
        /// </summary>
        Delete = 3
    }
}