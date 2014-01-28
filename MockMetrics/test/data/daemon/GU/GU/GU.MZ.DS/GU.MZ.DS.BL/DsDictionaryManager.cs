using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace GU.MZ.DS.BL
{
    /// <summary>
    /// Класс, предназначенный для хранения справочных данных схемы gu_mz_ds
    /// </summary>
    public class DsDictionaryManager : AbstractDictionaryManager
    {
        /// <summary>
        /// Класс, предназначенный для получения значений справочных таблиц БД.
        /// </summary>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <exception cref="BLLException">Ошибка заполнения кэша справочников</exception>
        public DsDictionaryManager(IDomainDbManager dbManager)
        {
            try
            {
                this._dictionaries = new Dictionary<Type, List<IDomainObject>>();
                this._enumDictionaries = new Dictionary<Type, Dictionary<int, string>>();
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка заполнения кэша справочников", ex);
            }            
        }

        /// <summary>
        /// Возвращает список доменных объектов.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Список доменных объектов</returns>
        private List<IDomainObject> GetDomainList<T>(IDomainDbManager dbManager) where T : class, IDomainObject
        {
            return dbManager.GetDomainTable<T>().ToList<IDomainObject>();
        }
    }
}
