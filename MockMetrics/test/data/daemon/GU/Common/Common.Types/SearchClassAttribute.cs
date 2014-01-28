using System;

namespace Common.Types
{
    /// <summary>
    /// Атрибут для разметки доменных классов, который указывает на то, что сущность может использоваться в настраиваемом поиске.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SearchClassAttribute : Attribute
    {
        /// <summary>
        /// Атрибут для разметки доменных классов, который указывает на то, что сущность может использоваться в настраиваемом поиске.
        /// </summary>
        /// <param name="displayName">Отображаемое имя сущности</param>
        public SearchClassAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// Отображаемое имя сущности.
        /// </summary>
        private string _displayName;
        
        /// <summary>
        /// Возвращает или устанавливает отображаемое имя сущности.
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
