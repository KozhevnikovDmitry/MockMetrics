using System;
using System.Windows.Controls;
using Common.BL.ReportMapping;
using Common.DA;
using Common.DA.Interface;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI
{
    public class SingletonDockableUiFactory : IDockableUiFactory
    {
        public ISearchVM<T> GetSearchVm<T>() where T : IPersistentObject
        {
            return (ISearchVM<T>)GetSearchVmType(typeof(T));
        }

        public ISearchVM GetSearchVmType(Type domainType)
        {
            return UIContainer.Instance.ResolveSearchVMType(domainType);
        }

        public IEditableVM<T> GetEditableVm<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            return (IEditableVM<T>)GetEditableVm(typeof(T), entity, isEditable);
        }

        public IEditableVM GetEditableVm(Type domainType, IDomainObject entity, bool isEditable = true)
        {
            return UIContainer.Instance.ResolveEditableVM(domainType, entity, isEditable);
        }

        public IEditableVM GetEditableVm(Type domainType, string entityKey, bool isEditable = true)
        {
            return UIContainer.Instance.ResolveEditableVM(domainType, entityKey, isEditable);
        }

        public UserControl GetEditableView(Type domainType)
        {
            return UIContainer.Instance.ResolveEditableView(domainType);
        }

        public UserControl GetReportPresenter(IReport report)
        {   
#if DEBUG
            var isDesigner = true;
#else
            var isDesigner = false;
#endif
            return UIFacade.GetReportPresenter(report, isDesigner);
        }
    }
}