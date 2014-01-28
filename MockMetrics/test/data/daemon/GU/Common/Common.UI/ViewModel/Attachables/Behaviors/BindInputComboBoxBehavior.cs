using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Поведение для <c>ComboBox</c> ввода, предназначенное для того, чтобы производить явный <c>UpdateSourceTrigger</c> по определённым событиям.
    /// </summary>
    public class BindInputComboBoxBehavior : Behavior<ComboBox>
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
            if (!AssociatedObject.IsFocused)
            {
                UpdateSourceTrigger();
            }
        }

        private void UpdateSourceTrigger()
        {
            BindingExpression textBinding = AssociatedObject.GetBindingExpression(ComboBox.TextProperty);
            BindingExpression selectedValueBinding = AssociatedObject.GetBindingExpression(ComboBox.SelectedValueProperty);
            BindingExpression selectedIndexBinding = AssociatedObject.GetBindingExpression(ComboBox.SelectedIndexProperty);
            
            if (textBinding != null)
            {
                textBinding.UpdateSource();
            }

            if (selectedValueBinding != null)
            {
                selectedValueBinding.UpdateSource();
            }

            if (selectedIndexBinding != null)
            {
                selectedIndexBinding.UpdateSource();
            }
        }
    }
}
