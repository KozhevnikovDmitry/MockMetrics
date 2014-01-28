using System.Windows.Interactivity;
using System.Windows.Controls;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для <c>DataGrid</c>, которое скролит <c>DataGrid</c> к выделенному элементу.
    /// </summary>
    public class ScrollIntoSelectedItemDataGridBehavior : Behavior<DataGrid>
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

        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssociatedObject.ScrollIntoView(AssociatedObject.SelectedItem);
        }
    }
}
