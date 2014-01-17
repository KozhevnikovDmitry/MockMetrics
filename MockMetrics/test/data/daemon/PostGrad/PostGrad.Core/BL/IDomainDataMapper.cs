using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Интерфейс классов, предназначенных для маппинга сложных доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменных объектов</typeparam>
    /// <remarks>
    /// Классы DataMapper'ы предназначены для отображения в БД и обратно сложных композиций доменных объектов.
    /// Мапперы работают только с персистентными объектами.
    /// </remarks>
    public interface IDomainDataMapper<T> 
        where T : IPersistentObject
    {
        /// <summary>
        /// Возвращает доменный объекта из БД по значению первичного ключа.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <returns>Доменный объект из БД</returns>
        T Retrieve(object id);

        /// <summary>
        /// Возвращает доменный объекта по значению первичного ключа из БД по конкретному подключению.
        /// </summary>
        /// <param name="id">Значение первичного ключа</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Доменный объект из БД</returns>
        T Retrieve(object id, IDomainDbManager dbManager);

        /// <summary>
        /// Сохраняет доменный объект в БД. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый доменный объект</returns>
        T Save(T obj, bool forceSave = false);

        /// <summary>
        /// Сохраняет доменный объект в БД по конкретному подключению. Возвращает сохранённый объект.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг безусловного сохранения</param>
        /// <returns>Сохранённый доменный объект</returns>
        T Save(T obj, IDomainDbManager dbManager, bool forceSave = false);

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

        /// <summary>
        /// Заполняет отображаемые ассоциации доменного объекта.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <remarks>
        /// Метод выгружает только ассоциации необходимые для отображения информации о сущности.
        /// Метод не обременяет себя проставлением всех взаимосвязей между составляющими сущностями.
        /// Получившаяся сущность может быть безопасно использована толька для отображения данных на фэйсе.
        /// </remarks>
        void FillAssociations(T obj);

        /// <summary>
        /// Заполняет отображаемые ассоциации доменного объекта по конкретному подключению.
        /// </summary>
        /// <param name="obj">Доменный объект</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        void FillAssociations(T obj, IDomainDbManager dbManager);
    }
}
