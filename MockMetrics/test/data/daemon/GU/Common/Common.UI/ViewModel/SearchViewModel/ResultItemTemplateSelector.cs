using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Common.Types.Exceptions;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.SearchViewModel
{
    /// <summary>
    /// Класс переключатель шаблонов для результатов поиска на <c>SearchView</c>.
    /// </summary>
    public class ResultItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Кэш шаблонов
        /// </summary>
        private static readonly Dictionary<string, DataTemplate> _templateCache = new Dictionary<string, DataTemplate>();

        /// <summary>
        /// Шаблон для выделенного элемента
        /// </summary>
        private readonly DataTemplate _selectedDataTemplate;

        /// <summary>
        /// Шаблон для невыделенного элемента
        /// </summary>
        private readonly DataTemplate _unSelectedDataTemplate;

        /// <summary>
        /// Класс переключатель шаблонов <c>ListBoxItem</c> для результатов поиска на <c>SearchView</c>.
        /// </summary>
        /// <param name="itemTypeName"></param>
        public ResultItemTemplateSelector(string itemTypeName)
        {
            _selectedDataTemplate = GetTemaplate(itemTypeName + "ListSelectedItemTemplate");
            _unSelectedDataTemplate = GetTemaplate(itemTypeName + "ListItemTemplate");
        }

        /// <summary>
        /// Возвращает шаблон <c>ListBoxItem</c> полученный из ресурсов или кэша.
        /// </summary>
        /// <param name="templateName">Имя шаблона</param>
        /// <returns>Шаблон</returns>
        /// <exception cref="VMException">Шаблон результата поиска не найден по имени</exception>
        private DataTemplate GetTemaplate(string templateName)
        {
            if (_templateCache.ContainsKey(templateName))
            {
                return _templateCache[templateName];
            }
            else
            {
                var result = (DataTemplate)Application.Current.Resources[templateName];
                if (result != null)
                {
                    _templateCache[templateName] = result;
                    return result;
                }
                else
                {
                    throw new VMException(string.Format("Шаблон результата поиска не найден по имени {0}", templateName));
                }
            }
        }

        /// <summary>
        /// Возвращает шаблон отображения результата поиска.
        /// </summary>
        /// <param name="item">ViewModel в результатом поиска</param>
        /// <param name="container">View в результатом поиска</param>
        /// <returns>Шаблон отображения результата поиска</returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ISelectableItemVM itemVM = (ISelectableItemVM)item;
            return itemVM.IsSelected ? _selectedDataTemplate : _unSelectedDataTemplate;
        }
    }
}
