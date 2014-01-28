using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Controls;

namespace Common.UI.ViewModel.Attachables.Properties
{
    public class IgnoreMouseWheelBehavior
    {
        public static readonly DependencyProperty IsMouseWheelIgnoredProperty =
            DependencyProperty.RegisterAttached
            (
                "IsMouseWheelIgnored",
                typeof(bool),
                typeof(IgnoreMouseWheelBehavior),
                new PropertyMetadata(false, OnIsMouseWheelIgnoredPropertyChanged)
            );

        public static bool GetIsMouseWheelIgnored(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseWheelIgnoredProperty);
        }

        public static void SetIsMouseWheelIgnored(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMouseWheelIgnoredProperty, value);
        }

        private static void OnIsMouseWheelIgnoredPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            if (dpo is Selector)
            {
                Selector selector = dpo as Selector;
                if ((bool)args.NewValue)
                {
                    selector.PreviewMouseWheel += OnPreviewListBoxMouseWheel;
                }
                else
                {
                    selector.PreviewMouseWheel -= OnPreviewListBoxMouseWheel;
                }
            }
        }

        private static void OnPreviewListBoxMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Selector selector = (Selector)sender;
            bool ignore = false;            
            foreach (var cb in selector.Descendants<ComboBox>().Cast<ComboBox>())
            {
                if (cb.IsDropDownOpen || cb.IsMouseOver)
                {
                    ignore = true;
                    break;
                }
            }
            if (!ignore)
            {
                var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                e2.RoutedEvent = UIElement.MouseWheelEvent;
                selector.RaiseEvent(e2);
            }
        }
    }
}
