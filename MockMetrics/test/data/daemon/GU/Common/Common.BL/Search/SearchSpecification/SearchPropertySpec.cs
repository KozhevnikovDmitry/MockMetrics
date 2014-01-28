using System;

using Common.Types;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс предназначенный для описания свойства доменного объекта по которому может производиться поиск.
    /// </summary>
    public class SearchPropertySpec
    {
        /// <summary>
        /// Класс предназначенный для описания свойства доменного объекта по которому может производиться поиск.
        /// </summary>
        /// <param name="displayName">Отображаемое имя свойства</param>
        /// <param name="domainPropertyName">Доменное имя свойства</param>
        /// <param name="domainName">Отображаемое имя объекта, которому принадлежит свойство</param>
        /// <param name="searchTypeSpec">Упрощённый тип свойства</param>
        /// <param name="domainPropertyType">Реальный тип свойства</param>
        /// <param name="domainType">Тип объекта, которому принадлежит свойство</param>
        public SearchPropertySpec(string displayName,
                                  string domainPropertyName,
                                  string domainName,
                                  SearchTypeSpec searchTypeSpec,
                                  Type domainPropertyType,
                                  Type domainType)
        {
            DisplayName = displayName;
            DomainPropertyName = domainPropertyName;
            DomainName = domainName;
            DomainType = domainType;
            SearchTypeSpec = searchTypeSpec;
            DomainPropertyType = domainPropertyType;
        }

        /// <summary>
        /// Возвращает отображаемое имя свойства.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Возвращает доменное имя свойства.
        /// </summary>
        public string DomainPropertyName { get; private set; }

        /// <summary>
        /// Возвращает отображаемое имя класса.
        /// </summary>
        public string DomainName { get; private set; }

        /// <summary>
        /// Возвращает упрощённый тип свойства доменного объекта.
        /// </summary>
        public SearchTypeSpec SearchTypeSpec { get; private set; }

        /// <summary>
        /// Возвращает реальный тип свойства доменного объекта.
        /// </summary>
        public Type DomainPropertyType { get; set; }

        /// <summary>
        /// Возвращает реальный тип доменного объекта.
        /// </summary>
        public Type DomainType { get; set; }
    }
}
