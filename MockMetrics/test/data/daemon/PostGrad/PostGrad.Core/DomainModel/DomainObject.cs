using System;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace PostGrad.Core.DomainModel
{
    /// <summary>
    /// Базовый класс для классов постоянных доменных объектов
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    public abstract class DomainObject<T> : EditableObject<T>, ICloneable where T : DomainObject<T>, IPersistentObject
    {
        /// <summary>
        /// Базовый класс для классов постоянных доменных объектов
        /// </summary>
        protected DomainObject()
        {
            PersistentState = PersistentState.New;
            AcceptChanges();
        }

        /// <summary>
        /// Возвращает значение первичного ключа сущности 
        /// </summary>
        /// <returns>Значние первичного ключа</returns>
        public abstract string GetKeyValue();

        /// <summary>
        /// Устанавливает значение первичного ключа сущности 
        /// </summary>
        /// <param name="val"></param>
        public abstract void SetKeyValue(object val);

        /// <summary>
        /// Статус постоянства доменного объекта.
        /// </summary>
        [MapIgnore]
        public abstract PersistentState PersistentState { get; set; }

        /// <summary>
        /// Объект-реплика, содержит информацию о репликации сущности.
        /// </summary>
        public ICommonData CommonData { get; set; }

        /// <summary>
        /// Помечает объект на удаление.
        /// </summary>
        public void MarkDeleted()
        {
            if (PersistentState == PersistentState.New)
                PersistentState = PersistentState.NewDeleted;
            if (PersistentState == PersistentState.Old)
                PersistentState = PersistentState.OldDeleted;
        }

        /// <summary>
        /// Перегрузка, учитывающая атрибут <see cref="CloneIgnoreAttribute"/>
        /// Перегружает generic-метод <see cref="EditableObject{T}"/>
        /// </summary>
        public override T Clone()
        {
            return (T)SmartTypeAccessor.Copy(this);
        }
        
        /// <summary>
        /// Перегрузка, учитывающая атрибут <see cref="CloneIgnoreAttribute"/>
        /// Неявно перегружает метод обычного <see cref="EditableObject"/>
        /// </summary>
        object ICloneable.Clone()
        {
            return SmartTypeAccessor.Copy(this);
        }
    }

    /// <summary>
    /// Модифицированный кусок класса <see cref="TypeAccessor"/>.
    /// В функцию копирования объекта добавлено условие: не копировать свойства помеченные <see cref="CloneIgnoreAttribute"/>.
    /// В остальном класс делает то же самое и полагается на обычный <see cref="TypeAccessor"/>.
    /// Исходный код попёрт с https://github.com/igor-tkachev/bltoolkit/blob/master/Source/Reflection/TypeAccessor.cs
    /// 
    /// Чтобы это работало на любом уровне вложенности, <see cref="DomainObject{T}"/> должен перегрузить метод Clone и  <see cref="EditableObject{T}"/> и просто <see cref="EditableObject"/>.
    /// </summary>
    internal static class SmartTypeAccessor
    {
        static object CopyInternal(object source, object dest, TypeAccessor ta)
        {
#if !SILVERLIGHT && !DATA
            var isDirty = false;
            var sourceEditable = source as IMemberwiseEditable;
            var destEditable = dest as IMemberwiseEditable;

            if (sourceEditable != null && destEditable != null)
            {
                foreach (MemberAccessor ma in ta)
                {
                    // Вот здесь добавлено условие
                    if (ma.GetAttribute<CloneIgnoreAttribute>() == null)
                    {
                        ma.CloneValue(source, dest);
                        if (sourceEditable.IsDirtyMember(null, ma.MemberInfo.Name, ref isDirty) && !isDirty)
                            destEditable.AcceptMemberChanges(null, ma.MemberInfo.Name);
                    }
                }
            }
            else
#endif
            {
                foreach (MemberAccessor ma in ta)
                {
                    // Вот здесь добавлено условие
                    if (ma.GetAttribute<CloneIgnoreAttribute>() == null)
                    {
                        ma.CloneValue(source, dest);
                    }
                }
            }

            return dest;
        }

        internal static object Copy(object source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var ta = TypeAccessor.GetAccessor(source.GetType());

            return CopyInternal(source, ta.CreateInstanceEx(), ta);
        }
    }
}
