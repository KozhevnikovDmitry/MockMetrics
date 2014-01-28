using System;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс представлющий выражение сортировки поиска доменных объектов
    /// </summary>
    [Serializable]
    public class SearchOrdering
    {
        public SearchOrdering()
        {
            
        }

        /// <summary>
        /// Имя свойства сортировки
        /// </summary>
        public string DomainPropertyName { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public OrderDirection OrderDirection { get; set; }
        
        /// <summary>
        /// Тип сущности хозяина свойства
        /// </summary>
        public Type DomainType { get; set; }
    }
}
