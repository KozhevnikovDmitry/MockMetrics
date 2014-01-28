using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using VIBlend.WPF.Controls;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для придания DatePicker'у свойств Nullable и Masked.
    /// </summary>
    /// <remarks>
    /// В результате ввод в DatePicker производится только по маске или через DropDown.
    /// Никаких разделителей печатать ненужно, только цифры. 
    /// Можно оставить дату незаполненой, в этом случае будет отображаться Watermark "Введите дату".
    /// --------------------------------------------------------------------------------------------
    /// Эффект достигается засчёт смены ControlTemplate'ов DatePickerTextBox внутри ControlTemaplate DatePicker'а.
    /// Основная идея хака в методе <c>AssociatedObject_GotFocus</c> попёрта отсюда http://mel-green.com/2011/02/wpf-focus-datepicker-on-text-in-textbox/
    /// </remarks>
    public class MaskedPromtDatePickerBehavior : Behavior<DatePicker>
    {
        /// <summary>
        /// Флаг, выставляемый при получении фокуса по Tab.
        /// </summary>
        private bool _lock;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedDateChanged += AssociatedObject_SelectedDateChanged;
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectedDateChanged -= AssociatedObject_SelectedDateChanged;
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus; 
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        /// <summary>
        /// По событию Loaded навешиваем шаблон на DatePickerTextBox в зависимости от наличия даты в DatePicker'е
        /// </summary>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            //Явно применяем дефолтный шаблон DatePicker'а
            AssociatedObject.ApplyTemplate();
            //Ищем в шаблоне DatePickerTextBox
            DatePickerTextBox _dateTextBox = (DatePickerTextBox)AssociatedObject.Template.FindName("PART_TextBox", AssociatedObject);
            _dateTextBox.GotFocus += DateTextBoxGotFocus;
            if (AssociatedObject.SelectedDate.HasValue)
            {
                //Если дата загружена налепляем Masked шаблон
                _dateTextBox.Template = (ControlTemplate)Application.Current.Resources["MaskedDatePickerTextBoxControlTemplate"];

            }
            else
            {
                //Если дата не загружена налепляем Promted шаблон с Watermark'ом
                _dateTextBox.Template = (ControlTemplate)Application.Current.Resources["PromtedDatePickerTextBoxControlTemplate"];
            }
        }

        /// <summary>
        /// Событие SelectedDateChanged обрабатывается на случай программного изменения даты через Модель Представления.
        /// </summary>
        void AssociatedObject_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AssociatedObject.ApplyTemplate();
            //В шаблоне DatePicker'a ищем DatePickerTextBox
            DatePickerTextBox dateTextBox = (DatePickerTextBox)AssociatedObject.Template.FindName("PART_TextBox", AssociatedObject);//Явно применяем шаблон
            
            if (AssociatedObject.SelectedDate.HasValue)
            {
                //Если дата загружена налепляем Masked шаблон
                dateTextBox.Template = (ControlTemplate)Application.Current.Resources["MaskedDatePickerTextBoxControlTemplate"]; 
                dateTextBox.ApplyTemplate();
                //В шаблоне DatePickerTextBox'a ищем MaskEditor
                var maskedText = (MaskEditor)dateTextBox.Template.FindName("PART_TextBox", dateTextBox);
                Keyboard.AddKeyDownHandler(maskedText, MaskedTextKeyDown);
            }
            else
            {
                //Если дата не загружена налепляем Promted шаблон с Watermark'ом
                dateTextBox.Template = (ControlTemplate)Application.Current.Resources["PromtedDatePickerTextBoxControlTemplate"];
            }
        }

        void MaskedTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.Focus(AssociatedObject);
                AssociatedObject.RaiseEvent(e);
            }
        }

        /// <summary>
        /// При переходе на DatePicker по Tab (именно Tab, а не Shift-Tab) фокус получает DatePickerTextBox, а не DatePicker.
        /// Поэтому обрабатываем ещё и DatePickerTextBox.GotFocus.
        /// </summary>
        void DateTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            //Флажок говорит о том, что DatePicker.LostFocus обрабатывать не надо (см. AssociatedObject_LostFocus)
            _lock = true;
            AssociatedObject_GotFocus(sender, e);
            _lock = false;
        }

        /// <summary>
        /// По событию GotFocus навешиваем на DatePickerTextBox Masked шаблон.
        /// </summary>
        void AssociatedObject_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            //Если нет даты в DatePicker'е
            if (!AssociatedObject.SelectedDate.HasValue)
            {
                //В шаблоне DatePicker'a ищем DatePickerTextBox
                DatePickerTextBox dateTextBox = (DatePickerTextBox)AssociatedObject.Template.FindName("PART_TextBox", AssociatedObject);
                //Налепляем на DatePickerTextBox Masked-шаблон
                dateTextBox.Template = (ControlTemplate)Application.Current.Resources["MaskedDatePickerTextBoxControlTemplate"];
                //Явно применяем шаблон
                dateTextBox.ApplyTemplate();
                //В шаблоне DatePickerTextBox'a ищем MaskEditor
                var maskedText = (MaskEditor)dateTextBox.Template.FindName("PART_TextBox", dateTextBox);               
                //Организуем событие KeyDown на _maskedText
                MaskedTextKeyDown(maskedText);
                Keyboard.AddKeyDownHandler(maskedText, MaskedTextKeyDown);
            }
            else
            {
                //Если даты в наличии, выделяем весь текст даты 
                DatePickerTextBox dateTextBox = (DatePickerTextBox)AssociatedObject.Template.FindName("PART_TextBox", AssociatedObject);
                dateTextBox.ApplyTemplate();
                var maskedText = dateTextBox.Template.FindName("PART_TextBox", dateTextBox);
                if (maskedText is MaskEditor)
                {
                    if (_lock)
                    {
                        MaskedTextKeyDown(maskedText as MaskEditor);
                    } 
                    (maskedText as MaskEditor).SelectAll();
                }
            }
        }

        /// <summary>
        /// Организует событие KeyDown на контроле <c>MaskEditor</c>.
        /// </summary>
        /// <param name="maskedText"><c>MaskEditor</c>, на который нужно ткнуть</param>
        private void MaskedTextKeyDown(MaskEditor maskedText)
        {
            //Хак: вручную делаем KeyDown клавиатуры на MaskEditor
            Keyboard.Focus(maskedText);
            var eventArgs = new KeyEventArgs(Keyboard.PrimaryDevice,
                                             Keyboard.PrimaryDevice.ActiveSource,
                                             0,
                                             Key.Down);
            eventArgs.RoutedEvent = UIElement.KeyDownEvent;
            maskedText.RaiseEvent(eventArgs);
            //KeyDown необходим для того, чтобы по клику на DatePicker курсор сразу попадал в наш MaskEditor.
            //В противном случае событие DatePicker.LostFocus при переходе на другие контролы не происходит.  
            //Соответсвенно смена шаблона предусмотренна в AssociatedObject_LostFocus не срабатывает, Watermark не появляется.
            //http://mel-green.com/2011/02/wpf-focus-datepicker-on-text-in-textbox/
        }

        /// <summary>
        /// По событию LostFocus навешиваем на DatePickerTextBox (если дата не введена) Promted шаблон  с Watermark'ом.
        /// </summary>
        void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!AssociatedObject.SelectedDate.HasValue && !AssociatedObject.IsDropDownOpen && !_lock)
            {
                DatePickerTextBox dateTextBox = (DatePickerTextBox)AssociatedObject.Template.FindName("PART_TextBox", AssociatedObject);
                dateTextBox.Template = (ControlTemplate)Application.Current.Resources["PromtedDatePickerTextBoxControlTemplate"];
            }
        }
    }
    
}
