using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.DataMapping
{
    /// <summary>
    /// Абстрактный маппер доменных объектов, содержащий общую логику по транзакциям, исключениям и эдитабельности.
    /// </summary>
    public abstract class AbstractDataMapper<T> : DomainDependent, IDomainDataMapper<T> where T : IPersistentObject
    {
        public AbstractDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }


        /// <summary>
        /// Возвращает доменный объекта из БД по значению первичного ключа.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <returns>Доменный объект из БД</returns>
        public T Retrieve(object id)
        {
            using (IDomainDbManager db = this.GetDbManager())
            {
                return Retrieve(id, db);
            }
        }

        /// <summary>
        /// Возвращает доменный объекта по значению первичного ключа из БД по конкретному подключению.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Доменный объект из БД</returns>
        public T Retrieve(object id, IDomainDbManager dbManager)
        {
            try
            {
                T obj = this.RetrieveOperation(id, dbManager);
                obj.AcceptChanges();
                return obj;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(string.Format("Ошибка при получении данных сущности {0}.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Сохраняет доменный объект в БД. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый доменный объект</returns>
        public T Save(T obj, bool forceSave = false)
        {
            using (IDomainDbManager db = this.GetDbManager())
            {
                return Save(obj, db, forceSave);
            }
        }

        /// <summary>
        /// Сохраняет доменный объект в БД по конкретному подключению. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый доменный объект</returns>
        public T Save(T obj, IDomainDbManager dbManager, bool forceSave = false)
        {
           try
            {
                dbManager.BeginDomainTransaction();

                var result = this.SaveOperation(obj, dbManager, forceSave);

                dbManager.CommitDomainTransaction();
                result.AcceptChanges();
                return result;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (TransactionControlException ex)
            {
                throw new BLLException(ex);
            }
            catch (Exception ex)
            {
                dbManager.RollbackDomainTransaction();
                throw new BLLException(string.Format("Ошибка при сохранении данных сущности {0}.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <param name="obj">Удаляему доменный объект</param>
        public virtual void Delete(T obj)
        {
            using (IDomainDbManager db = this.GetDbManager())
            {
                Delete(obj, db);
            }
        }

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <param name="obj">Удаляему доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        public void Delete(T obj, IDomainDbManager dbManager)
        {
            try
            {
                dbManager.BeginDomainTransaction();

                this.DeleteOperation(obj, dbManager);

                dbManager.CommitDomainTransaction();
            }
            catch (BLLException)
            {
                throw;
            }
            catch (TransactionControlException ex)
            {
                throw new BLLException(ex);
            }
            catch (Exception ex)
            {
                dbManager.RollbackDomainTransaction();
                throw new BLLException(string.Format("Ошибка при удалении данных сущности {0}.", typeof(T).Name), ex);
            }
        }


        /// <summary>
        /// Заполняет отображаемые ассоциации доменного объекта.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        public virtual void FillAssociations(T obj)
        {
            using (IDomainDbManager db = this.GetDbManager())
            {
                FillAssociations(obj, db);
            }
        }

        /// <summary>
        /// Заполняет отображаемые ассоциации доменного объекта по конкретному подключению.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        public void FillAssociations(T obj, IDomainDbManager dbManager)
        {
            try
            {
                this.FillAssociationsOperation(obj, dbManager);
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(string.Format("Ошибка при заполнении ассоциаций данных сущности {0}.", typeof(T).Name), ex);
            }
        }

        protected abstract T RetrieveOperation(object id, IDomainDbManager dbManager);

        protected abstract T SaveOperation(T obj, IDomainDbManager dbManager, bool forceSave = false);

        protected virtual void DeleteOperation(T obj, IDomainDbManager dbManager)
        {
            obj.MarkDeleted();
            dbManager.SaveDomainObject(obj);
        }

        protected abstract void FillAssociationsOperation(T obj, IDomainDbManager dbManager);

        protected void SaveChildCollection<T1>(EditableList<T1> childCollection, Action<T1> setId, IDomainDbManager dbManager) where T1 :  IPersistentObject
        {
            foreach (var child in childCollection)
            {
                setId(child);
                dbManager.SaveDomainObject(child);
            }

            if (childCollection.DelItems != null)
            {
                foreach (var delItem in childCollection.DelItems.Cast<T1>())
                {
                    delItem.MarkDeleted();
                    dbManager.SaveDomainObject(delItem);
                }
            }
        }

        protected void RetrieveChildCollection<T1>(EditableList<T1> childCollection, Func<T1, bool> filterIds, Action<T1> setParent, IDomainDbManager dbManager) where T1 : IdentityDomainObject<T1>, IPersistentObject
        {
            var ids =
                dbManager.GetDomainTable<T1>()
                         .Where(filterIds)
                         .Select(t => t.Id)
                         .ToList();

            foreach (var id in ids)
            {
                var child = dbManager.RetrieveDomainObject<T1>(id);
                childCollection.Add(child);
                setParent(child);
            }
        }
    }
}
