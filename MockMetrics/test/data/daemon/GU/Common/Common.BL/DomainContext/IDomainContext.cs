using Common.DA.Interface;

namespace Common.BL.DomainContext
{
    /// <summary>
    /// Интерфейс класса, предоставляющего доменный контекст для доменного зависимых классов.
    /// </summary>
    public interface IDomainContext
    {
        /// <summary>
        /// Вовзвращает менеджер базы данных для доменного контекста.
        /// </summary>
        /// <param name="contextKey">Ключ доменного контекста</param>
        /// <returns>Менеджер базы данных</returns>
        IDomainDbManager GetDbManager(string contextKey);
    }
}
