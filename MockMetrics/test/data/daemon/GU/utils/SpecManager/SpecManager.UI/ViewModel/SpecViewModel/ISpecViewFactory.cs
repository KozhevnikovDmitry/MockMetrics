using System.Windows.Controls;

using SpecManager.BL.Model;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public interface ISpecViewFactory
    {
        SpecVmBase GetSpecVm(SpecNodeParent specNodeParent);

        UserControl GetSpecView(SpecNodeParent specNodeParent);

        SpecDockableVm GetSpecDockableVm(Spec spec, string path);

        SpecDockableVm GetSpecDockableVm(string path);

        SpecDockableVm GetSpecDockableVm(int id, string connectionString);

        SpecDockableVm GetSpecDockableVm(string uri, string connectionString);
    }
}
