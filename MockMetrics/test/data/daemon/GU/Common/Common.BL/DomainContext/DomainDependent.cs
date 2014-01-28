using System;
using Common.DA.Interface;

namespace Common.BL.DomainContext
{
    /// <summary>
    /// Базовый класс для доменно-зависимых классов.
    /// </summary>
    public abstract class DomainDependent : TransactionWrapper, IDomainDependent
    {
        /// <summary>
        /// Доменный контекст.
        /// </summary>
        private readonly IDomainContext _domainContext;

        /// <summary>
        /// Идентификатор доменного контекста.
        /// </summary>
        private string _domainContextKey;

        /// <summary>
        /// Устанавливает идентификатор доменного контекста.
        /// </summary>
        /// <param name="assemblyName">Имя сборки с доменно-зависимыми классами</param>
        public void SetDomainKey(string assemblyName)
        {
            this._domainContextKey = assemblyName;
        }

        /// <summary>
        /// Устанавливает идентификатор доменного контекста.
        /// </summary>
        public string DomainKey
        {
            set
            {
                _domainContextKey = value;
            }
            get
            {
                return _domainContextKey;
            }
        }

        /// <summary>
        /// Базовый класс для доменно-зависимых классов.
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        protected DomainDependent(IDomainContext domainContext)
        {
            _domainContext = domainContext;
        }

        /// <summary>
        /// Возвращает менеджер базы данных, релевантный заданому доменному контексту.
        /// </summary>
        /// <returns>Менеджер базы данных</returns>
        protected IDomainDbManager GetDbManager()
        {
            return _domainContext.GetDbManager(this._domainContextKey);
        }

        /// <summary>
        /// Обёртка для выполнения действия внутри транзакции 
        /// </summary>
        /// <param name="action">Дейсвтие, включающее запись в БД</param>
        protected void TransactionWrapper(Action<IDomainDbManager> action)
        {
            using (var dbManager = this.GetDbManager())
            {
                Transaction(dbManager, action);
            }
        }
    }
}
