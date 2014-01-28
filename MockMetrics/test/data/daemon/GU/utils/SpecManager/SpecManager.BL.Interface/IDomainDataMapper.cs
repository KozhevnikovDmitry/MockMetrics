namespace SpecManager.BL.Interface
{
    /// <summary>
    /// Интерфейс классов, предназначенных для маппинга сложных доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    public interface IDomainDataMapper<T> 
        where T : IDomainObject
    {
        /// <summary>
        /// Возвращает доменный объекта из БД по значению первичного ключа.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <returns>Доменный объект из БД</returns>
        T Retrieve(int id);

        /// <summary>
        /// Возвращает доменный объекта по значению первичного ключа из БД по конкретному подключению.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Доменный объект из БД</returns>
        T Retrieve(int id, IDomainDbManager dbManager);

        /// <summary>
        /// Сохраняет доменный объект в БД. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <returns>Сохранённый доменный объект</returns>
        T Save(T obj);

        /// <summary>
        /// Сохраняет доменный объект в БД по конкретному подключению. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сохранённый доменный объект</returns>
        T Save(T obj, IDomainDbManager dbManager);

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <param name="obj">Удаляему доменный объект</param>
        void Delete(T obj);

        /// <summary>
        /// Удаляет данные доменного объекта из БД.
        /// </summary>
        /// <param name="obj">Удаляему доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        void Delete(T obj, IDomainDbManager dbManager);
    }
}
