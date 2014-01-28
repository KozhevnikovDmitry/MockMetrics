using System;

namespace Common.Types
{
    /// <summary>
    /// Атрибут для разметки свойств доменных классов, который указывает на то, 
    /// что свойство может использоваться в настраиваемом поиске.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchFieldAttribute : Attribute
    {
        /// <summary>
        /// Атрибут для разметки свойств доменных классов, который указывает на то, 
        /// что свойство может использоваться в настраиваемом поиске.
        /// </summary>
        /// <param name="name">Отображемое имя свойства</param>
        /// <param name="searchTypeSpec">Обобщённый тип свойства</param>
        public SearchFieldAttribute(string name, SearchTypeSpec searchTypeSpec)
        {
            _name = name;
            _searchTypeSpec = searchTypeSpec;
        }

        /// <summary>
        /// Отображемое имя свойства.
        /// </summary>
        private string _name;

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя свойства.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Обобщённый тип свойства
        /// </summary>
        private SearchTypeSpec _searchTypeSpec;
        
        /// <summary>
        /// Возвращает или устанавливает обобщённый тип свойства
        /// </summary>
        public SearchTypeSpec SearchTypeSpec
        {
            get { return _searchTypeSpec; }
            set { _searchTypeSpec = value; }
        }
    }

    /// <summary>
    /// Перечисление обобщённых типов свойства.
    /// </summary>
    public enum SearchTypeSpec
    {
        /// <summary>
        /// Число
        /// </summary>
        Number,

        /// <summary>
        /// Строка
        /// </summary>
        String,

        /// <summary>
        /// Дата
        /// </summary>
        Date,

        /// <summary>
        /// Флаг
        /// </summary>
        Bool,

        /// <summary>
        /// Перечисление
        /// </summary>
        Enum,

        /// <summary>
        /// Справочное поле
        /// </summary>
        Dictionary
    }
}
