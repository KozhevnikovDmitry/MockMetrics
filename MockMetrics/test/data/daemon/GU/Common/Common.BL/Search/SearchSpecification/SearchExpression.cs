using System;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс представляющий условие поиска доменных объектов.
    /// </summary>
    public class SearchExpression
    {
        /// <summary>
        /// Возвращает имя свойства, на которое налагается условие.
        /// </summary>
        public string PropertyName
        {
            get { return SearchPropertySpec.DomainPropertyName; }
        }

        /// <summary>
        /// Возвращает или устанавливает спецификацию условия поиска.
        /// </summary>
        public SearchExpressionSpec SearchExpressionSpec { get; set; }

        /// <summary>
        /// Возвращает или устанавливает спецификацию свойства, используемого в условии.
        /// </summary>
        public SearchPropertySpec SearchPropertySpec { get; set; }

        #region Specific values

        public string StringValue { get; set; }

        public int NumberValue1 { get; set; }

        public int NumberValue2 { get; set; }

        public DateTime? DateTimeValue1 { get; set; }

        public DateTime? DateTimeValue2 { get; set; }

        public bool BoolValue { get; set; }

        public int DictionarySelectedValue { get; set; }

        #endregion
    }
}
