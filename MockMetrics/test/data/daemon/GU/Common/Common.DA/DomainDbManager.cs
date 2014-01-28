using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;

using Common.DA.DAException;
using Common.DA.Interface;

namespace Common.DA
{
    /// <summary>
    /// Класс, предназначенный для осуществления доступа к источнику данных на основе доменных объектов.
    /// </summary>
    public abstract class DomainDbManager : DbManager, IDomainDbManager
    {
        /// <summary>
        /// Класс, предназначенный для осуществления доступа к источнику данных на основе доменных объектов.
        /// </summary>
        /// <param name="configurationString">Имя конфигурации подключения к БД</param>
        protected DomainDbManager(string configurationString)
            : base(configurationString)
        {
            _filterPredicates = new Dictionary<Type, object>();
        }

        #region Domain object mapping

        /// <summary>
        /// Получает и возвращает постоянный доменный объект из БД по значению первичного ключа.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="key">Значение первичного ключа</param>
        /// <returns>Результирующий доменный объект</returns>
        public T RetrieveDomainObject<T>(object key) where T : IPersistentObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), this);
            var obj = (T)oa.SelectByKey(key);
            obj.PersistentState = PersistentState.Old;
            //RetrieveCommonData(obj);
            return obj;
        }

        /// <summary>
        /// Сохраняет постоянный доменный объект в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        public void SaveDomainObject<T>(T obj) where T : IPersistentObject
        {
            SaveDomainObject(obj, false);
        }

        /// <summary>
        /// Сохраняет постоянный доменный объект в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        public void SaveDomainObject<T>(T obj, bool forceSave) where T : IPersistentObject
        {
            SaveDomainObject(obj, forceSave, null);
        }

        /// <summary>
        /// Сохраняет постоянный доменный объект в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        public void SaveDomainObject<T>(T obj, bool forceSave, string targetHost) where T : IPersistentObject
        {
            var eo = obj as EditableObject;
            if (eo != null)
            {
                if (!eo.IsDirty && !forceSave)
                    return;
            }

            switch (obj.PersistentState)
            {
                case PersistentState.New:
                    InsertDomainObject(obj, targetHost);
                    break;

                case PersistentState.NewDeleted:
                    break;

                case PersistentState.Old:
                    UpdateDomainObject(obj, targetHost);
                    break;

                case PersistentState.OldDeleted:
                    DeleteDomainObject(obj, targetHost);
                    break;
            }
        }

        private void InitializeCommonData<T>(T obj, ReplicaActionType action) where T : IPersistentObject
        {
            var doi = DomainObjectInitializerContainer.Instance.ResolveInitializer(obj.GetType());
            doi.InitializeObjectCommonData(obj, action);
        }

        /// <summary>
        /// Отображает данные доменного объекта в запись БД. (insert)
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        private void InsertDomainObject<T>(T obj, string targetHost) where T : IPersistentObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), this);
            obj.SetKeyValue(oa.Insert(obj));
            obj.PersistentState = PersistentState.Old;

            InitializeCommonData(obj, ReplicaActionType.Insert);
            SaveCommonData(obj.CommonData, targetHost);
        }

        /// <summary>
        /// Отображает данные доменного объекта в запись БД. (update)
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        private void UpdateDomainObject<T>(T obj, string targetHost) where T : IPersistentObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), this);
            oa.Update(obj);
            obj.PersistentState = PersistentState.Old;

            InitializeCommonData(obj, ReplicaActionType.Update);
            SaveCommonData(obj.CommonData, targetHost);
        }

        /// <summary>
        /// Отображает данные доменного объекта в запись БД. (insert/update)
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        private void InsertOrUpdateDomainObject<T>(T obj, string targetHost) where T : IPersistentObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), this);
            oa.InsertOrUpdate(obj);
            obj.PersistentState = PersistentState.Old;

            InitializeCommonData(obj, ReplicaActionType.Update);
            SaveCommonData(obj.CommonData, targetHost);
        }

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        private void DeleteDomainObject<T>(T obj, string targetHost) where T : IPersistentObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), this);
            oa.Delete(obj);

            InitializeCommonData(obj, ReplicaActionType.Delete);
            SaveCommonData(obj.CommonData, targetHost);
        }

        /// <summary>
        /// Получает и возвращает последний объект-реплики для доменного объекта.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        protected virtual void RetrieveCommonData(IPersistentObject obj)
        {
            // т.к. CommonData может быть несколько штук и находятся они в других сборках
            // приходится загрузку CommonData выносить в наследников DomainDbManager
            // подумать, может можно обойтись без оверайда
            // например, как-то тип ресолвить с помощью TypeManager'а
            // UPD 17.09.2012, petrenko:
            // Возвращаемся на старый вариант, т.к. вариант с  abstract IEnumerable<ICommonData>
            // приводил к тому, что сначала происходила материализация всего списка, а только потом
            // в Linq2Objects накладывались условия.
            throw new NotImplementedException("RetrieveCommonData was not redefined");
        }

        /// <summary>
        /// Сохраняет объект-реплики для доменного объекта
        /// </summary>
        /// <param name="cd">Объект-реплика</param>
        /// <param name="targetHost">Целевой хост доменного объекта</param>
        private void SaveCommonData(ICommonData cd, string targetHost)
        {
            Type openAccessorType = typeof(AbstractAccessor<>);
            Type actualAccessorType = openAccessorType.MakeGenericType(new[] { cd.GetType() });
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(actualAccessorType, this);
            cd.Stamp = DateTime.Now;
            oa.Insert(cd);
        }

        #endregion

        #region Transaction management

        private int _transactionLevel = 0;

        /// <summary>
        /// Открывает транзакцию отображения доменных объектов в БД.
        /// </summary>
        /// <remarks>
        /// Увеличиваем уровень на 1 - если он == 1, значит это был первый вызов
        /// начала транзакции - открываем транзакцию. Если нет - то просто поднимаем уровень.
        /// </remarks>
        /// <exception cref="Exception">Ошибка инициирования транзакции</exception>
        public void BeginDomainTransaction()
        {
            _transactionLevel++;

            if (_transactionLevel == 1)
            {
                try
                {
                    base.BeginTransaction();
                }
                catch (Exception)
                {
                    _transactionLevel--;
                    throw;
                }
            }
        }

        /// <summary>
        /// Фиксирует транзакцию отображения доменных объектов в БД.
        /// </summary>
        /// <remarks>
        /// Уменьшаем уровень на 1 - если он == 0, значит мы вернулись на уровень вложенности,
        /// где в действительности началась транзакция. Комитим ее, если не получилось -
        /// восстанавливаем уровень транзакции.
        /// Если транзакции на момент вызова не существует (_transactionLevel == 0) -
        /// сообщаем об этом вызывающему коду живительным исключением.
        /// </remarks>
        /// <exception cref="Exception">Ошибка фиксации транзакции</exception>
        /// <exception cref="TransactionControlException">Транзакция уже была зафиксирована или отменена.</exception>
        public void CommitDomainTransaction()
        {
            if (_transactionLevel > 0)
            {
                _transactionLevel--;

                if (_transactionLevel == 0)
                {
                    try
                    {
                        base.CommitTransaction();
                    }
                    catch (Exception)
                    {
                        _transactionLevel++;
                        throw;
                    }
                }
            }
            else
            {
                throw new TransactionControlException();
            }
        }

        /// <summary>
        /// Откатывает транзакцию отображения доменных объектов в БД.
        /// </summary>
        /// <remarks>
        /// IDbTransaction не поддерживает SavePoint'ов - поэтому при вызове метода дергается
        /// ролбэк, вне зависимости от того, на каком уровне вложенности произошел вызов.
        /// Если после отката будет повторный вызов метода выбрасывается исключение,
        /// которое должен обработать вызывающий код.
        /// </remarks>
        /// <exception cref="TransactionControlException">Транзакция уже была зафиксирована или отменена.</exception>
        public void RollbackDomainTransaction()
        {
            if (_transactionLevel == 0)
                throw new TransactionControlException();

            _transactionLevel = 0;
            base.RollbackTransaction();
        }

        #endregion

        #region Domain Table Retrieving

        /// <summary>
        /// Словарь предикатов, используемых для фильтрации запросов по сущностям в методе GetDomainTable()
        /// </summary>
        /// <remarks>
        /// Предикаты задуманы для того, чтобы фильтровать запросы по некоторым таблицам для конкретного DbManager'а
        /// согласно ограничениям предметной области.
        /// </remarks>
        protected readonly Dictionary<Type, object> _filterPredicates;

        /// <summary>
        /// Возвращает фильтрованный запрос по доменной таблице.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Запрос по доменной таблице</returns>
        public IQueryable<T> GetDomainTable<T>() where T : class, IDomainObject
        {
            // выставляем большой таймаут на случай долгих запросов по вьюхам.
            SelectCommand.CommandTimeout = 30000;

            // делаем запрос
            IQueryable<T> query = GetTable<T>();

            // фильтруем запрос
            if (_filterPredicates.ContainsKey(typeof(T)))
            {
                query = query.Where((Expression<Func<T, bool>>)_filterPredicates[typeof(T)]);
            }

            return query;
        }

        /// <summary>
        /// Возращает представление запроса данных по таблице содержащей отображение объектов типа <c>domainType</c>
        /// </summary>
        /// <param name="domainType">Тип доменного объекта</param>
        /// <returns>Представление запроса по таблице <c>domainType</c></returns>
        /// <remarks>
        /// Может работать медленнее из-за применения Reflection.
        /// </remarks>
        public IQueryable GetDomainTable(Type domainType)
        {
            MethodInfo method = GetType().GetMethod("GetDomainTable", new Type[] { })
                                .MakeGenericMethod(new[] { domainType });
            return (IQueryable)method.Invoke(this, new object[] { });
        }

        #endregion
    }
}
