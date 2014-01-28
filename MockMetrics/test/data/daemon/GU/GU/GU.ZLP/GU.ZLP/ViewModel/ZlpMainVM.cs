using Common.UI;
using Common.UI.ViewModel;

namespace GU.ZLP.ViewModel
{
    public class ZlpMainVM : BaseAvalonDockVM
    {
        public ZlpMainVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}
