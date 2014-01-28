using Common.UI;
using Common.UI.ViewModel;

namespace GU.MZ.CERT.UI.ViewModel
{
    /// <summary>
    /// Класс VM для View модуля аттестации
    /// </summary>
    public class CertificationModuleVM : BaseAvalonDockVM
    {
        /// <summary>
        /// Класс VM для View модуля аттестации.
        /// </summary>
        public CertificationModuleVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}
