using System.Collections.Generic;
using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Интерфейс для класса, инкапсулирующего данные для проведения поиска в стратегии.
    /// </summary>
    public interface ISearchData
    {
        /// <summary>
        /// Настройки для настраиваемого поиска
        /// </summary>
        SearchPreset SearchPreset { get; }

        /// <summary>
        /// Шаблонный объект для шаблонного поиска.
        /// </summary>
        IDomainObject SearchObject { get; }

        /// <summary>
        /// Список шаблонных объектов для комбинированного шаблонного поиска
        /// </summary>
        List<IDomainObject> CombinedSearchObjects { get; }

        /// <summary>
        /// Размер страницы поиска
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Стартовая позиция страницы
        /// </summary>
        int PageStartPosition { get; }
        
        /// <summary>
        /// Вид шаблонного поиска
        /// </summary>
        SearchActionType SearchActionType { get; }

        /// <summary>
        /// Подготавливает объект к повторному использованию в поиске.
        /// </summary>
        /// <param name="position">Новая позиция страницы</param>
        void PrepareReuse(int position);
    }

    /// <summary>
    /// Перечисление видов шаблонного поиска
    /// </summary>
    public enum SearchActionType
    {
        /// <summary>
        /// Поиск объекта по шаблону того же типа
        /// </summary>
        Self,

        /// <summary>
        /// Поиск объекта по шаблону ассоццированной сущностям
        /// </summary>
        OrdinalJoin,

        /// <summary>
        /// Комбинированный поиск по нескольким шаблонам
        /// </summary>
        Combined,

        /// <summary>
        /// По умолчанию - выборка объектов без условий.
        /// </summary>
        Default
    }
}
