namespace GU.HQ.DataModel.Types
{
    /// <summary>
    /// справочник статусов
    /// </summary>
    public enum ClaimStatusType 
    {
        /// <summary>
        /// Проверка данных
        /// </summary>
        DataCheck = 1,
        
        /// <summary>
        /// Заявление поставлено в очередь.
        /// </summary>
        QueueReg  = 2,

        /// <summary>
        /// Заявление поставлено в очередь внеочередников.
        /// </summary>
        QueuePrivReg = 3,

        /// <summary>
        /// Заявление исключено из очереди внеочередников.
        /// </summary>
        QueuePrivDeReg = 4,
    
        /// <summary>
        /// Жилье предоставлено.
        /// </summary>
        HouseProvided = 5,

        /// <summary>
        /// Заявление отклонено/исключено из очереди.
        /// </summary>
        Rejected = 6
    }
}
