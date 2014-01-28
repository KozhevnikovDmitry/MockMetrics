using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.DictionaryManagement.DictionaryException;
using Common.DA.Interface;

namespace Common.BL.DictionaryManagement
{
    /// <summary>
    /// Базовый класс, для классов-хранилищ справочных данных БД.
    /// </summary>
    /// <remarks>
    /// Класс выгружает и хранит в словарях значения всех малых справочников, а также перечислений(в виде пар int,string).
    /// Словарь _dictionaries предназначен для хранения значений всех малых справочник.
    /// Словарь _enumDictionaries хранит значения всех перечислений, используемых в доменных объектах аки справочная таблица.
    /// Класс задуман для того, что не дёргать базу каждый раз, когда нужны данные из малых справочников. Вместо этого они все выгружены в память.
    /// </remarks>
    public abstract class AbstractDictionaryManager : IDictionaryManager
    {
        protected AbstractDictionaryManager()
        {
            _mergedManagers = new List<IDictionaryManager>();
            _dictionaries = new Dictionary<Type, List<IDomainObject>>();
            _enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
            _dynamicDictionaries = new Dictionary<Type, Func<IDomainDbManager, List<IDomainObject>>>();
        }

        /// <summary>
        /// Список замерженных менеджеров.
        /// </summary>
        protected readonly List<IDictionaryManager> _mergedManagers = new List<IDictionaryManager>();

        /// <summary>
        /// Словарь со значениями всех малых справочников.
        /// </summary>
        /// <remarks>
        /// Ключ(Type) - тип доменного объекта в справочнике, Значение(List) - содержание справочника.
        /// </remarks>
        protected Dictionary<Type, List<IDomainObject>> _dictionaries;


        protected readonly Dictionary<Type, Func<IDomainDbManager, List<IDomainObject>>> _dynamicDictionaries;

        /// <summary>
        /// Словарь со значениями всех перечислений используемых в доменных объектах аки справочники.
        /// </summary>
        /// <remarks>
        /// В этот словарь попадают перечисления, которые необходимо отображать в ComboBox'ы, например.
        /// Ключ(Type) - тип перечисления, Значение(Dictionary) - пары (номер значения, имя) значения перечисления.
        /// </remarks>
        protected Dictionary<Type, Dictionary<int, string>> _enumDictionaries;

        public Func<IDomainDbManager> GetDb { get; set; }

        /// <summary>
        /// Возвращает одно значение из справочника по ключу
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта в справочнике</typeparam>
        /// <param name="id">Первичный ключ желаемого объекта</param>
        /// <exception cref="DictionaryItemNotFoundException">Требуемый элемент в справочнике сущностей не найден.</exception>
        /// <returns>Значение из справочника</returns>
        public virtual T GetDictionaryItem<T>(object id) where T : IDomainObject
        {
            try
            {
                return GetDictionary<T>().Single(x => x.GetKeyValue() == id.ToString());
            }
            catch (InvalidOperationException)
            {
                throw new DictionaryItemNotFoundException(typeof(T));
            }
        }

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта в справочнике</typeparam>
        /// <returns>Значения справочника</returns>
        /// <exception cref="DictionaryNotFoundException">Справочник сущностей не найден</exception>
        public virtual List<T> GetDictionary<T>() where T : IDomainObject
        {
            if (_dictionaries.ContainsKey(typeof(T)))
            {
                return _dictionaries[typeof(T)].Cast<T>().ToList();
            }
            
            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetDictionary<T>();
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(typeof(T));
        }

        public virtual List<T> GetDictionary<T>(Func<T, bool> predicate) where T : IDomainObject
        {
            return GetDictionary<T>().Where(predicate).ToList();
        }

        public List<T> GetDynamicDictionary<T>(IDomainDbManager dbManager) where T : IDomainObject
        {
            if (_dynamicDictionaries.ContainsKey(typeof(T)))
            {
                return _dynamicDictionaries[typeof(T)](dbManager).Cast<T>().ToList();
            }

            foreach (var dictionaryManager in _mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetDynamicDictionary<T>(dbManager);
                }
                catch (DictionaryNotFoundException)
                {
                }
            }
            return GetDictionary<T>();
        }

        public List<T> GetDynamicDictionary<T>() where T : IDomainObject
        {
            if (GetDb == null)
            {
                return GetDictionary<T>();
            }

            using (var db = GetDb())
            {
                return GetDynamicDictionary<T>(db);
            }
        }

        public List<T> GetDynamicDictionary<T>(Func<T, bool> predicate) where T : IDomainObject
        {
            return GetDynamicDictionary<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <param name="typeName">Имя типа доменного объекта в справочнике</param>
        /// <returns>Значения справочника</returns>
        /// <exception cref="DictionaryNotFoundException">Справочник сущностей не найден</exception>
        public virtual List<IDomainObject> GetDictionary(string typeName)
        {
            var dictionary = (from d in _dictionaries
                        where d.Key.Name == typeName
                        select d.Value).SingleOrDefault();

            if (dictionary != null)
            {
                return dictionary;
            }

            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetDictionary(typeName);
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(typeName);
        }

        /// <summary>
        /// Возвращает значения справочника.
        /// </summary>
        /// <param name="type">Tип доменного объекта в справочнике</param>
        /// <returns>Значения справочника</returns>
        public virtual List<IDomainObject> GetDictionary(Type type)
        {
            if (_dictionaries.ContainsKey(type))
            {
                return _dictionaries[type];
            }

            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetDictionary(type);
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(type);
        }

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <returns>Значения перечисления</returns>
        /// <exception cref="DictionaryNotFoundException">Справочник сущностей не найден</exception>
        public Dictionary<int, string> GetEnumDictionary<T>()
        {
            if (_enumDictionaries.ContainsKey(typeof(T)))
            {
                return _enumDictionaries[typeof(T)];
            }

            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetEnumDictionary<T>();
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(typeof(T));
        }

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <param name="typeName">Имя типа перечисления</param>
        /// <returns>Значения перечисления</returns>
        /// <exception cref="DictionaryNotFoundException">Справочник сущностей не найден</exception>
        public Dictionary<int, string> GetEnumDictionary(string typeName)
        {
            var enumDictionary = (from d in _enumDictionaries
                              where d.Key.Name == typeName
                              select d.Value).SingleOrDefault();

            if (enumDictionary != null)
            {
                return enumDictionary;
            }

            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetEnumDictionary(typeName);
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(typeName);
        }

        /// <summary>
        /// Возвращает значения перечисления в виде пар (номер значения, имя).
        /// </summary>
        /// <param name="type">Tип доменного объекта в справочнике</param>
        /// <returns>Значения справочника</returns>
        /// <exception cref="DictionaryNotFoundException">Справочник сущностей не найден</exception>
        public Dictionary<int, string> GetEnumDictionary(Type type)
        {
            if (_enumDictionaries.ContainsKey(type))
            {
                return _enumDictionaries[type];
            }

            foreach (var dictionaryManager in this._mergedManagers)
            {
                try
                {
                    return dictionaryManager.GetEnumDictionary(type);
                }
                catch (DictionaryNotFoundException)
                {
                }
            }

            throw new DictionaryNotFoundException(type);
        }

        /// <summary>
        /// Мержит справочники из другого менеджера.
        /// </summary>
        /// <param name="dictionaryManager">Менеджер справочников для мержа</param>
        public void Merge(IDictionaryManager dictionaryManager)
        {
            _mergedManagers.Add(dictionaryManager);
        }

        /// <summary>
        /// Возвращает true, если менеджер уже замержен.
        /// </summary>
        /// <param name="dictionaryManager">Менеджер справочников для мержа</param>
        /// <returns>true, если менеджер уже замержен</returns>
        public bool IsMerged(IDictionaryManager dictionaryManager)
        {
            return _mergedManagers.Contains(dictionaryManager);
        }
    }
}
