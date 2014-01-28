using System.Windows.Interactivity;
using System.Windows.Data;
using System.Windows.Controls;

namespace SpecManager.UI.ViewModel.Behaviors
{
    /// <summary>
    /// Поведение для <c>ComboBox</c> ввода, предназначенное для того, чтобы производить явный <c>UpdateSourceTrigger</c> по определённым событиям.
    /// </summary>
    public class BindInputComboBoxBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.LostKeyboardFocus += this.AssociatedObject_LostKeyboardFocus;
            this.AssociatedObject.KeyDown += this.AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.LostKeyboardFocus -= this.AssociatedObject_LostKeyboardFocus;
            this.AssociatedObject.KeyDown -= this.AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter ||
                e.Key == System.Windows.Input.Key.LeftCtrl ||
                e.Key == System.Windows.Input.Key.RightCtrl)
            {
                this.UpdateSourceTrigger();
            }
        }

        private void AssociatedObject_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (!this.AssociatedObject.IsFocused)
            {
                this.UpdateSourceTrigger();
            }
        }

        private void UpdateSourceTrigger()
        {
            BindingExpression textBinding = this.AssociatedObject.GetBindingExpression(ComboBox.TextProperty);
            BindingExpression selectedValueBinding = this.AssociatedObject.GetBindingExpression(ComboBox.SelectedValueProperty);
            BindingExpression selectedIndexBinding = this.AssociatedObject.GetBindingExpression(ComboBox.SelectedIndexProperty);
            
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
