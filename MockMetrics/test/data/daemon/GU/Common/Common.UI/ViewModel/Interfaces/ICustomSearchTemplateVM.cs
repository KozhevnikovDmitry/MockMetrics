using System.ComponentModel;
using Common.DA.Interface;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomSearchTemplateVM : IWorkspaceVM
    {

    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ICustomSearchTemplateVM<T> : ICustomSearchTemplateVM where T : IDomainObject
    {

    }

    /// <summary>
    /// </summary>
    public interface ISearchPresetVM : INotifyPropertyChanged
    {

    }
}
