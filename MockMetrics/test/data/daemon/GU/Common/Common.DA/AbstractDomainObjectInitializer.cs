using System;
using Common.DA.Interface;

namespace Common.DA
{
    /// <summary>
    /// Класс предназначенный для начальной инициализации свойств доменного объекта
    /// </summary>
    /// <remarks>
    /// Вызов инициализатора происходит в конструкторе базового класса.
    /// Если в конструкторе наследника переопределяются какие-то значения -
    /// не забыть поставить AcceptChanges.
    /// </remarks>
    public abstract class AbstractDomainObjectInitializer : IDomainObjectInitializer
    {
        /// <summary>
        /// Класс предназначенный для начальной инициализации свойств доменного объекта
        /// </summary>
        /// <param name="userName">Имя пользователя приложения</param>
        protected AbstractDomainObjectInitializer(string userName)
        {
            _userName = userName;
        }

        private readonly string _userName;

        /// <summary>
        /// Инициализиует доменный объект
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="obj">Доменный объект</param>
        public abstract void InitializeObject<T>(T obj) where T : IPersistentObject;

        public void InitializeObjectCommonData<T>(T obj, ReplicaActionType action) where T : IPersistentObject
        {
            obj.CommonData = CreateCommonData();
            obj.CommonData.Entity = obj.GetType().Name;
            obj.CommonData.KeyValue = obj.GetKeyValue();
            obj.CommonData.Stamp = DateTime.Now;
            obj.CommonData.User = _userName;
            obj.CommonData.Action = action;
        }

        protected abstract ICommonData CreateCommonData();
    }
}
