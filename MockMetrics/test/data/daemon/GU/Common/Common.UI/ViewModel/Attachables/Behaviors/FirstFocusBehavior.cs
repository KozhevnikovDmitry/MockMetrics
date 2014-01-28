using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Класс-поведение, предназначенный для перевода фокуса на первый элементу управления по TabOrder на UserControl'е при загрузке.
    /// </summary>
    public class FirstFocusBehavior : Behavior<UserControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching(); 
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.First));
        }
    }
}
