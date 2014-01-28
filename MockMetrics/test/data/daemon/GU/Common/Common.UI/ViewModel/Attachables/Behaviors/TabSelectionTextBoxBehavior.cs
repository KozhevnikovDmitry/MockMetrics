using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для <c>TextBox</c> ввода, предназначенное для того, выделять весь текст при получении фокуса ввода клавиатуры
    /// </summary>
    /// <remarks>
    /// Нехорошо только то, что клике мышкой в центр текста
    /// </remarks>
    public class TabSelectionTextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
        }

        void AssociatedObject_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }
    }
}
