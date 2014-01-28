namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс класса, предназначенного для начальной инициализации доменных объектов.
    /// </summary>
    public interface IDomainObjectInitializer
    {
        /// <summary>
        /// Инициализиует доменный объект
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        void InitializeObject<T>(T obj) where T : IPersistentObject;

        void InitializeObjectCommonData<T>(T obj, ReplicaActionType action) where T : IPersistentObject;
    }
}
