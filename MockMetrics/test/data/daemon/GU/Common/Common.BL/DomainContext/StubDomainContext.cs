using Common.DA.Interface;

namespace Common.BL.DomainContext
{
    /// <summary>
    /// Класс-заглушка доменного контекста, всегда возвращает переданный в конструктор менеджер БД.
    /// </summary>
    public class StubDomainContext : IDomainContext
    {
        /// <summary>
        /// Менеджер базы данных
        /// </summary>
        private readonly IDomainDbManager _dbManager;

        /// <summary>
        /// Класс-заглушка доменного контекста, всегда возвращает переданный в конструктор менеджер БД.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        public StubDomainContext(IDomainDbManager dbManager)
        {
            _dbManager = dbManager;
        }

        #region Implementation of IDomainContext

        /// <summary>
        /// Вовзвращает менеджер базы данных для доменного контекста.
        /// </summary>
        /// <param name="contextKey">Ключ доменного контекста</param>
        /// <returns>Менеджер базы данных</returns>
        public IDomainDbManager GetDbManager(string contextKey)
        {
            return _dbManager;
        }

        #endregion
    }
}
