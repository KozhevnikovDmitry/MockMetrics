using System.Windows.Interactivity;
using System.Windows.Controls;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для <c>ComboBox</c>, которое интерпретирует первый элемент списка как null.
    /// </summary>
    /// <remarks>
    /// Данное поведение может быть использовано для nullable CombBox. Поведение только обрабатывает выбор первого элеметна
    /// В ItemsSource целевого ComboBox'a необходимо и обязательно добавлять специальный элемент, который отображается в списке как "Не выбрано" или "Не указано".
    /// Если этого не сделать, поведение ComboBox'a будет неадекватным.
    /// </remarks>
    public class NotChosenItemComboBoxBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssociatedObject.SelectedItem != null && AssociatedObject.SelectedItem.Equals(AssociatedObject.Items[0]))
            {
                AssociatedObject.SelectedItem = null;
            }
        }
    }
}
