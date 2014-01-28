using Common.UI;
using Common.UI.ViewModel;

namespace GU.MZ.DS.UI.ViewModel
{
    /// <summary>
    /// Класс VM для View модуля Лекарственное обеспечение.
    /// </summary>
    public class DrugSupplyModuleVM : BaseAvalonDockVM
    {
        /// <summary>
        /// Класс VM для View модуля Лекарственное обеспечение.
        /// </summary>
        public DrugSupplyModuleVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}
