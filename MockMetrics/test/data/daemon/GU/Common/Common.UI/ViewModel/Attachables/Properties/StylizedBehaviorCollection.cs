using System.Windows;
using System.Windows.Interactivity;

namespace Common.UI.ViewModel.Attachables.Properties
{
    public class StylizedBehaviorCollection : FreezableCollection<Behavior>
    {
        #region Methods (protected)

        protected override Freezable CreateInstanceCore()
        {
            return new StylizedBehaviorCollection();
        }

        #endregion
    }
}
