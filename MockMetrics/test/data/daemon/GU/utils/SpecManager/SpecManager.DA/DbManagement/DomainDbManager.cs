using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;

using SpecManager.BL.Interface;
using SpecManager.DA.Access;
using SpecManager.DA.Exceptions;

namespace SpecManager.DA.DbManagement
{
    /// <summary>
    /// Класс, предназначенный для осуществления доступа к источнику данных на основе доменных объектов.
    /// </summary>
    internal class DomainDbManager : IDomainDbManager
    {
        private readonly DbManager _dbManager;

        public DomainDbManager(DataProviderBase dataProvider, string connectionString)
        {
            _dbManager = new DbManager(dataProvider, connectionString);
            DbManager.TurnTraceSwitchOn();
            DbManager.WriteTraceLine = (message, displayName) => Debug.WriteLine(message, displayName);
            _transactionLevel = 0;
        }

        public void Execute(string query, params object[] parameters)
        {
            _dbManager.SetSpCommand(query, parameters).ExecuteNonQuery();
        }

        #region Domain object mapping

        /// <summary>
        /// Получает и возвращает постоянный доменный объект из БД по значению первичного ключа.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="key">Значение первичного ключа</param>
        /// <returns>Результирующий доменный объект</returns>
        public T RetrieveDomainObject<T>(object key) where T : IDomainObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), _dbManager);
            var obj = (T)oa.SelectByKey(key);
            return obj;
        }

        /// <summary>
        /// Сохраняет постоянный доменный объект в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        public void SaveDomainObject<T>(T obj) where T : IDomainObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), _dbManager);
            if (obj.Id == 0)
            {
                obj.Id = Convert.ToInt32(oa.Insert(obj));
            }
            else
            {
                oa.Update(obj);
            }
        }

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        public void DeleteDomainObject<T>(T obj) where T : IDomainObject
        {
            var oa = (IDomainObjectAccessor)DataAccessor.CreateInstance(typeof(AbstractAccessor<T>), _dbManager);
            oa.Delete(obj);
        }

        #endregion

        #region Transaction management

        private int _transactionLevel;

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
            this._transactionLevel++;

            if (this._transactionLevel == 1)
            {
                try
                {
                    _dbManager.BeginTransaction();
                }
                catch (Exception)
                {
                    this._transactionLevel--;
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
            if (this._transactionLevel > 0)
            {
                this._transactionLevel--;

                if (this._transactionLevel == 0)
                {
                    try
                    {
                        _dbManager.CommitTransaction();
                    }
                    catch (Exception)
                    {
                        this._transactionLevel++;
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
            if (this._transactionLevel == 0)
                throw new TransactionControlException();

            this._transactionLevel = 0;
            _dbManager.RollbackTransaction();
        }

        #endregion

        #region Domain Table Retrieving
        
        /// <summary>
        /// Возвращает фильтрованный запрос по доменной таблице.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Запрос по доменной таблице</returns>
        public IQueryable<T> GetDomainTable<T>() where T : class, IDomainObject
        {
            // выставляем большой таймаут на случай долгих запросов по вьюхам.
            _dbManager.SelectCommand.CommandTimeout = 30000;
            return _dbManager.GetTable<T>();
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
            MethodInfo method = this.GetType().GetMethod("GetDomainTable", new Type[] { })
                                .MakeGenericMethod(new[] { domainType });
            return (IQueryable)method.Invoke(this, new object[] { });
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _dbManager.Dispose();
        }
        
        #endregion

    }
}
