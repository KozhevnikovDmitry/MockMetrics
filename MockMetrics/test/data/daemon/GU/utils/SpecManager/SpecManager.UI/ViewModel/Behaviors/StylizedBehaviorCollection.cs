using System.Windows;
using System.Windows.Interactivity;

namespace SpecManager.UI.ViewModel.Behaviors
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
