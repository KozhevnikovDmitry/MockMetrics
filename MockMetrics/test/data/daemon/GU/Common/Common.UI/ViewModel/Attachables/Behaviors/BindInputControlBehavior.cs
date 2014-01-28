using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для <c>TextBox</c> ввода, предназначенное для того, чтобы производить явный <c>UpdateSourceTrigger</c> по определённым событиям.
    /// </summary>
    /// <remarks>
    /// Поведение предназначено на <c>TextBox</c>'ов диалоговых окон, в особенности для окон с <c>ToolBar</c>.
    /// В случае с <c>ToolBar</c> есть нехорошая фича WPF: <c>ToolBar</c> обладает собственной областью фокуса.
    /// Из-за этого при обращении к кнопкам на <c>TextBox</c> не происходит <c>LostFocus</c>, а следовательно <c>UpdateSourceTrigger</c>.
    /// Данное поведение производит явный <c>UpdateSourceTrigger</c> по событию <c>LostKeyboardFocus</c>, которов таки срабатывает при обращение к <c>ToolBar</c>.
    /// Также поведение срабатывает по нажатию Enter и компонентов hotkey - LeftCtrl и RightCtrl, чтобы избежать потери данных при срабатывании hotkey-комманд.
    /// </remarks>
    public class BindInputControlBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LostKeyboardFocus -= AssociatedObject_LostKeyboardFocus;
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.LeftCtrl ||
                e.Key == System.Windows.Input.Key.RightCtrl)
            {
                UpdateSourceTrigger();
            }
        }

        private void AssociatedObject_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            UpdateSourceTrigger();
        }

        private void UpdateSourceTrigger()
        {
            BindingExpression be = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource(); 
        }
    }
    
}
