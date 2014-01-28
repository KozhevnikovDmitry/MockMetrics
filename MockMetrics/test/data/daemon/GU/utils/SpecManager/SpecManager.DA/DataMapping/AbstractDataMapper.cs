using System;

using SpecManager.BL.Interface;
using SpecManager.DA.Exceptions;

namespace SpecManager.DA.DataMapping
{
    public abstract class AbstractDataMapper<T> : IDomainDataMapper<T> where T : IDomainObject
    {
        private readonly IDbFactory _dbFactory;

        protected AbstractDataMapper(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public T Retrieve(int id)
        {
            using (var db = this._dbFactory.GetDbManager())
            {
                return this.Retrieve(id, db);
            }
        }

        public T Retrieve(int id, IDomainDbManager dbManager)
        {
            try
            {
                T obj = this.RetrieveOperation(id, dbManager);
                return obj;
            }
            catch (DALException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DALException(string.Format("Ошибка при получении данных сущности {0}.", typeof(T).Name), ex);
            }
        }

        public T Save(T obj)
        {
            using (var db = this._dbFactory.GetDbManager())
            {
                return this.Save(obj, db);
            }
        }

        public T Save(T obj, IDomainDbManager dbManager)
        {
            try
            {
                dbManager.BeginDomainTransaction();

                var result = this.SaveOperation(obj, dbManager);

                dbManager.CommitDomainTransaction();
                return result;
            }
            catch (DALException)
            {
                throw;
            }
            catch (TransactionControlException ex)
            {
                throw new DALException(ex);
            }
            catch (Exception ex)
            {
                dbManager.RollbackDomainTransaction();
                throw new DALException(string.Format("Ошибка при сохранении данных сущности {0}.", typeof(T).Name), ex);
            }
        }

        public virtual void Delete(T obj)
        {
            using (var db = this._dbFactory.GetDbManager())
            {
                this.Delete(obj, db);
            }
        }

        public void Delete(T obj, IDomainDbManager dbManager)
        {
            try
            {
                dbManager.BeginDomainTransaction();

                this.DeleteOperation(obj, dbManager);

                dbManager.CommitDomainTransaction();
            }
            catch (DALException)
            {
                throw;
            }
            catch (TransactionControlException ex)
            {
                throw new DALException(ex);
            }
            catch (Exception ex)
            {
                dbManager.RollbackDomainTransaction();
                throw new DALException(string.Format("Ошибка при удалении данных сущности {0}.", typeof(T).Name), ex);
            }
        }


        protected abstract T RetrieveOperation(int id, IDomainDbManager dbManager);

        protected abstract T SaveOperation(T obj, IDomainDbManager dbManager);

        protected virtual void DeleteOperation(T obj, IDomainDbManager dbManager)
        {
            dbManager.DeleteDomainObject(obj);
        }
    }
}
