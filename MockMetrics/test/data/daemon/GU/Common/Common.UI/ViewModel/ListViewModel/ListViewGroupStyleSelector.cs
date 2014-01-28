using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Common.Types.Exceptions;

namespace Common.UI.ViewModels.ListViewModel
{
    /// <summary>
    /// Класс переключатель стилей для групп элементов на <c>ListView</c>
    /// </summary>
    public class ListViewGroupStyleSelector : StyleSelector
    {
        /// <summary>
        /// Кэш стилей
        /// </summary>
        private static readonly Dictionary<string, Style> _styleCache = new Dictionary<string, Style>();

        /// <summary>
        /// Возвращает стиль группировки элементов на <c>ListView</c>
        /// </summary>
        /// <param name="item">View коллекции элементов в группе</param>
        /// <param name="container">View группы</param>
        /// <returns>Стиль группы</returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            CollectionViewGroup groupView = item as CollectionViewGroup;
            string groupStyleKey = string.Empty;
            if (groupView.Items != null && groupView.Items.Count != 0)
            {
                string vmItemName = groupView.Items.First().GetType().Name;
                groupStyleKey = vmItemName + "GroupContainerStyle";
                return GetStyle(groupStyleKey);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Возвращает стиль группы на <c>ListView</c> полученный из ресурсов или кэша.
        /// </summary>
        /// <param name="groupStyleName">Имя стиля</param>
        /// <returns>Стиль группы</returns>
        private Style GetStyle(string groupStyleName)
        {
            if (_styleCache.ContainsKey(groupStyleName))
            {
                return _styleCache[groupStyleName];
            }
            else
            {
                var result = (Style)Application.Current.Resources[groupStyleName];
                if (result != null)
                {
                    _styleCache[groupStyleName] = result;
                    return result;
                }
                else
                {
                    throw new VMException(string.Format("Стиль группировки элементов не найден. {0}", groupStyleName));
                }
            }
        }
    }
}
