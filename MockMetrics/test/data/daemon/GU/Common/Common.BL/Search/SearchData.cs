using System.Collections.Generic;
using Common.BL.Search.SearchSpecification;
using Common.DA.Interface;

namespace Common.BL.Search
{
    /// <summary>
    /// Класс, инкапсулирующий данные для проведения поиска доменных объектов.
    /// </summary>
    public class SearchData : ISearchData
    {
        /// <summary>
        /// Класс, инкапсулирующий данные для проведения поиска доменных объектов.
        /// </summary>
        /// <param name="searchObject">Шаблонный объект для шаблонного поиска</param>
        /// <param name="pageSize">Размер страницы поиска</param>
        /// <param name="pageStartPosition">Стартовая позиция страницы</param>
        /// <param name="isSelfSearch">Флаг указывающий на то, является ли поиск вида Self</param>
        public SearchData(IDomainObject searchObject,
                          int pageSize,
                          int pageStartPosition,
                          bool isSelfSearch = false)
        {
            SearchObject = searchObject;
            PageSize = pageSize;
            PageStartPosition = pageStartPosition;
            SearchActionType = isSelfSearch ? SearchActionType.Self : SearchActionType.OrdinalJoin;

        }
        
        /// <summary>
        /// Класс, инкапсулирующий данные для проведения поиска доменных объектов.
        /// </summary>
        /// <param name="combinedSearchObjects">Список шаблонных объектов для комбинированного шаблонного поиска</param>
        /// <param name="pageSize">Размер страницы поиска</param>
        /// <param name="pageStartPosition">Стартовая позиция страницы</param>
        public SearchData(List<IDomainObject> combinedSearchObjects,
                          int pageSize,
                          int pageStartPosition)
        {
            CombinedSearchObjects = combinedSearchObjects;
            PageSize = pageSize;
            PageStartPosition = pageStartPosition;
            SearchActionType = SearchActionType.Combined;
        }
        
        /// <summary>
        /// Класс, инкапсулирующий данные для проведения поиска доменных объектов.
        /// </summary>
        /// <param name="pageSize">Размер страницы поиска</param>
        /// <param name="pageStartPosition">Стартовая позиция страницы</param>
        public SearchData(int pageSize,
                          int pageStartPosition)
        {
            PageSize = pageSize;
            PageStartPosition = pageStartPosition;
            SearchActionType = SearchActionType.Default;
        }
        
        /// <summary>
        /// Класс, инкапсулирующий данные для проведения поиска доменных объектов.
        /// </summary>
        /// <param name="searchPreset">Настройки для настраиваемого поиска</param>
        /// <param name="pageSize">Размер страницы поиска</param>
        /// <param name="pageStartPosition">Стартовая позиция страницы</param>
        public SearchData(SearchPreset searchPreset,
                          int pageSize,
                          int pageStartPosition)
        {
            SearchPreset = searchPreset;
            PageSize = pageSize;
            PageStartPosition = pageStartPosition;
        }

        /// <summary>
        /// Настройки для настраиваемого поиска
        /// </summary>
        public SearchPreset SearchPreset { get; private set; }

        /// <summary>
        /// Шаблонный объект для шаблонного поиска.
        /// </summary>
        public IDomainObject SearchObject { get; private set; }

        /// <summary>
        /// Список шаблонных объектов для комбинированного шаблонного поиска
        /// </summary>
        public List<IDomainObject> CombinedSearchObjects { get; private set; }

        /// <summary>
        /// Размер страницы поиска
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Стартовая позиция страницы
        /// </summary>
        public int PageStartPosition { get; private set; }

        /// <summary>
        /// Вид шаблонного поиска
        /// </summary>
        public SearchActionType SearchActionType { get; private set; }

        /// <summary>
        /// Подготавливает объект к повторному использованию в поиске.
        /// </summary>
        /// <param name="position">Новая позиция страницы</param>
        public void PrepareReuse(int position)
        {
            PageStartPosition = position;
        }
    }
}
