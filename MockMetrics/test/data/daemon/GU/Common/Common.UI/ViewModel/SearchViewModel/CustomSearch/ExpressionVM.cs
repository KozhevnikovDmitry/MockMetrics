using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Common.BL.Search.SearchSpecification;
using Common.Types;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.SearchViewModel.CustomSearch
{
    /// <summary>
    /// Класс ViewModel для отображения одного условия настраиваемого поиска.
    /// </summary>
    public class ExpressionVM : NotificationObject
    {
        /// <summary>
        /// Отображаемое условие поиска
        /// </summary>
        public SearchExpression SearchExpression { get; private set; }

        /// <summary>
        ///  Класс ViewModel для отображения одного условия настраиваемого поиска.
        /// </summary>
        /// <param name="searchExpression">Условие поиска</param>
        public ExpressionVM(SearchExpression searchExpression)
        {
            SearchExpression = searchExpression;
            RemoveExpressionCommand = new DelegateCommand(RemoveExpression);
            SearchFieldDefinition = string.Format("{0} / {1}",
                                                   SearchExpression.SearchPropertySpec.DomainName,
                                                   SearchExpression.SearchPropertySpec.DisplayName);
            ExpressionDefinition = SearchExpression.SearchExpressionSpec.Name;
            string templateKey = SearchExpression.SearchExpressionSpec.SearchTypeSpec.ToString() + SearchExpression.SearchExpressionSpec.ExpressionQuantity.ToString();
            DisplayTemplate = (ControlTemplate)Application.Current.Resources[templateKey];
            CreateExpressionValues();
        }

        /// <summary>
        /// Задаёт набор возможных значений для справочных полей.
        /// </summary>
        private void CreateExpressionValues()
        {
            if (SearchExpression.SearchExpressionSpec.SearchTypeSpec == SearchTypeSpec.Dictionary ||
                SearchExpression.SearchExpressionSpec.SearchTypeSpec == SearchTypeSpec.Enum)
            {
                DictionaryValues = UIContainer.Instance.ResolveDictionaryValues(SearchExpression.SearchPropertySpec.DomainPropertyType);
                DictionarySelectedValue = SearchExpression.DictionarySelectedValue != 0 ? 
                                          SearchExpression.DictionarySelectedValue : DictionaryValues.First().Key;
            }
        }

        #region Binding Properties

        /// <summary>
        /// Описание свойства, которое используется в условии
        /// </summary>
        public string SearchFieldDefinition { get; set; }

        /// <summary>
        /// Описание усоловия
        /// </summary>
        public string ExpressionDefinition { get; set; }

        /// <summary>
        /// Шаблон для задания значений в условие.
        /// </summary>
        public ControlTemplate DisplayTemplate { get; set; }

        #region Specific values

        public string StringValue
        {
            get
            {
                return SearchExpression.StringValue;
            }
            set
            {
                if (SearchExpression.StringValue != value)
                {
                    SearchExpression.StringValue = value;
                    RaisePropertyChanged(() => StringValue);
                }
            }
        }

        public string NumberValue1
        {
            get
            {
                return SearchExpression.NumberValue1.ToString();
            }
            set
            {
                int result;
                Int32.TryParse(value, out result);

                if (SearchExpression.NumberValue1 != result)
                {
                    SearchExpression.NumberValue1 = result;
                    RaisePropertyChanged(() => NumberValue1);
                }
            }
        }

        public string NumberValue2
        {
            get
            {
                return SearchExpression.NumberValue2.ToString();
            }
            set
            {
                int result;
                Int32.TryParse(value, out result);

                if (SearchExpression.NumberValue2 != result)
                {
                    SearchExpression.NumberValue2 = result;
                    RaisePropertyChanged(() => NumberValue2);
                }
            }
        }

        public DateTime? DateTimeValue1
        {
            get
            {
                return SearchExpression.DateTimeValue1;
            }
            set
            {
                if (SearchExpression.DateTimeValue1 != value)
                {
                    SearchExpression.DateTimeValue1 = value;
                    RaisePropertyChanged(() => DateTimeValue1);
                }
            }
        }

        public DateTime? DateTimeValue2
        {
            get
            {
                return SearchExpression.DateTimeValue2;
            }
            set
            {
                if (SearchExpression.DateTimeValue2 != value)
                {
                    SearchExpression.DateTimeValue2 = value;
                    RaisePropertyChanged(() => DateTimeValue2);
                }
            }
        }

        public bool BoolValue
        {
            get
            {
                return SearchExpression.BoolValue;
            }
            set
            {
                if (SearchExpression.BoolValue != value)
                {
                    SearchExpression.BoolValue = value;
                    RaisePropertyChanged(() => BoolValue);
                }
            }
        }

        private Dictionary<int, object> _dictionaryValues;

        public Dictionary<int, object> DictionaryValues
        {
            get
            {
                return _dictionaryValues;
            }
            set
            {
                if (_dictionaryValues != value)
                {
                    _dictionaryValues = value;
                    RaisePropertyChanged(() => DictionaryValues);
                }
            }
        }

        public int DictionarySelectedValue
        {
            get
            {
                return SearchExpression.DictionarySelectedValue;
            }
            set
            {
                if (SearchExpression.DictionarySelectedValue != value)
                {
                    SearchExpression.DictionarySelectedValue = value;
                    RaisePropertyChanged(() => DictionarySelectedValue);
                }
            }
        }

        #endregion

        #endregion

        #region Binding Commands

        /// <summary>
        /// Событие оповещающие о необходимости удалить условие из пресета
        /// </summary>
        public event Action<SearchExpression> OnRemoveExpressionRequsted;

        /// <summary>
        /// Команда запроса на удаление условия из пресета.
        /// </summary>
        public DelegateCommand RemoveExpressionCommand { get; protected set; }

        /// <summary>
        /// Посылает запрос на удаление условия.
        /// </summary>
        private void RemoveExpression()
        {
            try
            {
                if(OnRemoveExpressionRequsted != null)
                {
                    OnRemoveExpressionRequsted(SearchExpression);
                }
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        #endregion
    }
}
