using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace GU.MZ.UI.ViewModel.Attachable.Behavior
{
    public class WindowClosingBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += AssociatedObjectClosing;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Closing -= AssociatedObjectClosing;
        }

        private void AssociatedObjectClosing(object sender, CancelEventArgs e)
        {
            var vm = AssociatedObject.DataContext as IExitVm;
            if (vm != null)
            {
                if (vm.CanExit())
                {
                    vm.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
