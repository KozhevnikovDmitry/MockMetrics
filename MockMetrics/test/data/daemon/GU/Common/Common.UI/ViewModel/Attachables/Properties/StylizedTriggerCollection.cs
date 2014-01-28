using System.Windows;

namespace Common.UI.ViewModel.Attachables.Properties
{

    public class StylizedTriggerCollection : FreezableCollection<System.Windows.Interactivity.EventTrigger>
    {
        #region Methods (protected)

        protected override Freezable CreateInstanceCore()
        {
            return new StylizedTriggerCollection();
        }

        #endregion
    }
}
