using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение, которое определяет стиль ToolTip об ошибке для контрола.
    /// </summary>
    public class ErrorToolTipStyleBehavior : Behavior<Control>
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

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (!AssociatedObject.Resources.Contains(typeof(ToolTip)))
            {
                AssociatedObject.Resources.Add(typeof(ToolTip), Application.Current.Resources["ErrorToolTipStyle"]);
            }
        }
    }
}
