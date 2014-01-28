namespace Common.BL.DomainContext
{
    /// <summary>
    /// Интерфейс для доменно-зависимых классов
    /// </summary>
    public interface IDomainDependent
    {
        /// <summary>
        /// Устанавливает идентификатор доменного контекста.
        /// </summary>
        /// <param name="assemblyName">Имя сборки с доменно-зависимыми классами</param>
        void SetDomainKey(string assemblyName);

        /// <summary>
        /// Устанавливает идентификатор доменного контекста.
        /// </summary>
        string DomainKey { set; }
    }
}