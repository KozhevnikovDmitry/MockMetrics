using System;
using System.Linq;
using PostGrad.Core.DomainModel;

namespace PostGrad.Core.DA
{
    /// <summary>
    /// Интерфейс менеждера доступа к данным доменных объектов.
    /// </summary>
    public interface IDomainDbManager : IDisposable 
    {
        /// <summary>
        /// Возвращает экземпляр доменного объекта полученного из БД по ключу.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="key">Значение первичного ключа</param>
        /// <returns>Экземпляр доменного объекта</returns>
        T RetrieveDomainObject<T>(object key) where T : IPersistentObject;

        /// <summary>
        /// Сохраняет данные доменного объекта в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        void SaveDomainObject<T>(T obj) where T : IPersistentObject;

        /// <summary>
        /// Сохраняет данные доменного объекта в БД.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        void SaveDomainObject<T>(T obj, bool forceSave) where T : IPersistentObject;

        /// <summary>
        /// Открывает транзакцию отображения доменных объектов.
        /// </summary>
        void BeginDomainTransaction();

        /// <summary>
        /// Откатывает транзакцию отображения доменных объектов в БД.
        /// </summary>
        void RollbackDomainTransaction();

        /// <summary>
        /// Фиксирует транзакцию отображения доменных объектов в БД.
        /// </summary>
        void CommitDomainTransaction();

        /// <summary>
        /// Возращает представление запроса данных по таблице содержащей отображение объектов типа T.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <returns>Представление запроса по таблице T</returns>
        IQueryable<T> GetDomainTable<T>() where T : class, IDomainObject;

        /// <summary>
        /// Возращает представление запроса данных по таблице содержащей отображение объектов типа <c>domainType</c>
        /// </summary>
        /// <param name="domainType">Тип доменного объекта</param>
        /// <returns>Представление запроса по таблице <c>domainType</c></returns>
        IQueryable GetDomainTable(Type domainType);
    }
}