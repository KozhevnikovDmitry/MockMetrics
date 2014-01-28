using System.Windows.Interactivity;
using Microsoft.Windows.Controls;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    /// <summary>
    /// Класс-поведение, предназнченный для того, чтобы устанавливать курсор в начало <c>MaskedTextBox</c> по клику мыши.
    /// </summary>
    public class MaskedTextBoxKeyDownBehavior : Behavior<MaskedTextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }

        void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                AssociatedObject.Select(0, 0);
            }
        }
    }
}
