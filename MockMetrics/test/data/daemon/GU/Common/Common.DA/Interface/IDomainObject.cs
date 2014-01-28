namespace Common.DA.Interface
{
    /// <summary>
    /// Интерфейс для классов доменных объектов
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Возвращает значение первичного ключа сущности 
        /// </summary>
        /// <returns>Значние первичного ключа</returns>
        string GetKeyValue();

        /// <summary>
        /// Устанавливает значение первичного ключа сущности 
        /// </summary>
        /// <param name="val"></param>
        void SetKeyValue(object val);
    }
}
