namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс классов, предназначенных для отображения доменных объектов в базу данных.
    /// </summary>
    public interface IDomainObjectAccessor
    {
        /// <summary>
        /// Выбирает доменный объект из БД по первичному ключу.
        /// </summary>
        /// <param name="key">Значение первичного ключа</param>
        /// <returns>Результирующий объект</returns>
        IDomainObject SelectByKey(object key);

        /// <summary>
        /// Вставляет доменный объект в БД.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <returns>Значение первичного ключа новой записи в БД</returns>
        object Insert(IDomainObject obj);

        /// <summary>
        /// Обновляет данные доменного объекта в БД.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <returns>Значение первичного ключа обновлённой записи в БД</returns>
        object Update(IDomainObject obj);

        /// <summary>
        /// Сохраняет доменный объект в БД. Если объект новый - вставляет, если уже сохранённый - обновляет данные.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <returns>Значение первичного ключа сохранённыой записи в БД</returns>
        object InsertOrUpdate(IDomainObject obj);

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        void Delete(IDomainObject obj);

        /// <summary>
        /// Удаляет запись о доменному объекте из БД по первичному ключу.
        /// </summary>
        /// <param name="key">Значение первичного ключа удалённой записи</param>
        void DeleteByKey(object key);
    }
}