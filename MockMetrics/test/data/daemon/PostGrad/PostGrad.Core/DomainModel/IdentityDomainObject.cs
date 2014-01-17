using System;

namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Базовый класс для доменных объектов с целочисленными первичными ключей.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    public abstract class IdentityDomainObject<T> : DomainObject<T> where T : IdentityDomainObject<T>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        public abstract int Id { get; set; }

        /// <summary>
        /// Возвращает строковое значение первичного ключа сущности.
        /// </summary>
        /// <returns>Значение первичного ключа сущности</returns>
        public override string GetKeyValue()
        {
            return Id.ToString();
        }

        /// <summary>
        /// Устанавливает значение первичного ключа сущности.
        /// </summary>
        /// <param name="val">Значение первичного ключа</param>
        /// <exception cref="ArgumentException">Аргумент неверного типа</exception>
        public override void SetKeyValue(object val)
        {
            try
            {
                Id = Convert.ToInt32(val);
            }
            catch(Exception ex)
            {
                throw new ArgumentException("Argument of the wrong type", ex);
            }
        }
    }
}
