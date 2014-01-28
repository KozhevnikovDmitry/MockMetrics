using System;
using System.Windows.Controls;
using Common.BL.ReportMapping;
using Common.DA;
using Common.DA.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.Interface
{
    public interface IDockableUiFactory
    {
        ISearchVM<T> GetSearchVm<T>() where T : IPersistentObject;
        ISearchVM GetSearchVmType(Type domainType);
        IEditableVM<T> GetEditableVm<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject;
        IEditableVM GetEditableVm(Type domainType, IDomainObject entity, bool isEditable = true);
        IEditableVM GetEditableVm(Type domainType, string entityKey, bool isEditable = true);
        UserControl GetEditableView(Type domainType);
        UserControl GetReportPresenter(IReport report);
    }

    public interface IDockableDialogFactory : IDockableUiFactory, IDialogUiFactory
    {
        
    }
}