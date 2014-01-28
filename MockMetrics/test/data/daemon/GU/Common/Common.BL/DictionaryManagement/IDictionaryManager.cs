using System;
using System.Collections.Generic;
using Common.DA.Interface;

namespace Common.BL.DictionaryManagement
{
    /// <summary>
    /// Интерфейс, предназначенный для получения значений справочных таблиц.
    /// </summary>
    public interface IDictionaryManager
    {
        /// <summary>
        /// Возвращает одно значение из справочника по ключу
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта в справочнике</typeparam>
        /// <param name="id">Первичный ключ желаемого объекта</param>
        T GetDictionaryItem<T>(object id) where T : IDomainObject;

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта в справочнике</typeparam>
        /// <returns>Значения справочника</returns>
        List<T> GetDictionary<T>() where T : IDomainObject;

        List<T> GetDictionary<T>(Func<T, bool> predicate) where T : IDomainObject;

        List<T> GetDynamicDictionary<T>() where T : IDomainObject;

        List<T> GetDynamicDictionary<T>(Func<T, bool> predicate) where T : IDomainObject;

        List<T> GetDynamicDictionary<T>(IDomainDbManager dbManager) where T : IDomainObject;

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <param name="typeName">Имя типа доменного объекта в справочнике</param>
        /// <returns>Значения справочника</returns>
        List<IDomainObject> GetDictionary(string typeName);

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <param name="type">Тип доменного объекта в справочнике</param>
        /// <returns>Значения справочника</returns>
        List<IDomainObject> GetDictionary(Type type);

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <returns>Значения перечисления</returns>
        Dictionary<int, string> GetEnumDictionary<T>();

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <param name="typeName">Имя типа перечисления</param>
        /// <returns>Значения перечисления</returns>
        Dictionary<int, string> GetEnumDictionary(string typeName);

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <param name="type">Nип перечисления</param>
        /// <returns>Значения перечисления</returns>
        Dictionary<int, string> GetEnumDictionary(Type type);

        /// <summary>
        /// Мержит справочники из другого менеджера.
        /// </summary>
        /// <param name="dictionaryManager">Менеджер справочников для мержа</param>
        void Merge(IDictionaryManager dictionaryManager);

        /// <summary>
        /// Возвращает true, если менеджер уже замержен.
        /// </summary>
        /// <param name="dictionaryManager">Менеджер справочников для мержа</param>
        /// <returns>true, если менеджер уже замержен</returns>
        bool IsMerged(IDictionaryManager dictionaryManager);
    }

}
