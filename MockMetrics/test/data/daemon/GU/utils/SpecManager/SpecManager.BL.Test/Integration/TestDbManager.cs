using System;
using System.Linq;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Test.Integration
{
    public class TestDbManager : IDomainDbManager
    {
        private readonly IDomainDbManager _domainDbManager;

        public TestDbManager(IDomainDbManager domainDbManager)
        {
            this._domainDbManager = domainDbManager;
        }

        public void Dispose()
        {
            
        }

        public T RetrieveDomainObject<T>(object key) where T : IDomainObject
        {
            return this._domainDbManager.RetrieveDomainObject<T>(key);
        }

        public void SaveDomainObject<T>(T obj) where T : IDomainObject
        {
            this._domainDbManager.SaveDomainObject(obj);
        }

        public void DeleteDomainObject<T>(T obj) where T : IDomainObject
        {
            this._domainDbManager.DeleteDomainObject(obj);
        }

        public void BeginDomainTransaction()
        {
            this._domainDbManager.BeginDomainTransaction();
        }

        public void RollbackDomainTransaction()
        {
            this._domainDbManager.RollbackDomainTransaction();
        }

        public void CommitDomainTransaction()
        {
            this._domainDbManager.CommitDomainTransaction();
        }

        public IQueryable<T> GetDomainTable<T>() where T : class, IDomainObject
        {
            return this._domainDbManager.GetDomainTable<T>();
        }

        public IQueryable GetDomainTable(Type domainType)
        {
            return this._domainDbManager.GetDomainTable(domainType);
        }

        public void Execute(string query, params object[] parameters)
        {
            this._domainDbManager.Execute(query, parameters);
        }
    }
}