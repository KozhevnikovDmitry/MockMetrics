using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для отображения своего дефолтного watermark в DateTimePicker.
    /// </summary>
    /// <remarks>
    /// Код взят с блога http://matthamilton.net/datepicker-watermark 
    /// Смысл в том, чтобы не писать наследника от <c>DateTimePicker</c> и не переопределять его <c>ContentTemplate</c>.
    /// Вместо этого глобальный <c>EventHandler</c> настраивает каждый <c>DateTimePicker</c> при загрузке.  
    /// Поэтому желательно положить данное поведение в какой-нибудь xaml поглобальнее.
    /// </remarks>
    public class DateTimePickerCustomWatermarkBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            EventManager.RegisterClassHandler(typeof(DatePicker),
                                              FrameworkElement.LoadedEvent,
                                              new RoutedEventHandler(DatePicker_Loaded));
        }

        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp == null) return;

            var tb = GetChildOfType<DatePickerTextBox>(dp);
            if (tb == null) return;

            var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
            if (wm == null) return;

            wm.Content = "Введите дату";
        }
    }
}
