using System.Windows;
using Common.DA.Interface;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public class EditableUnit
    {
        public IPersistentObject Entity { get; private set; }
        public ISmartEditableVm Vm { get; private set; }
        public IWeakEventListener Listener { get; set; }

        public EditableUnit(IPersistentObject entity, ISmartEditableVm vm)
        {
            Entity = entity;
            Vm = vm;
        }
    }

    public class ValidateableUnit
    {
        public IDomainObject Entity { get; private set; }
        public ISmartValidateableVm Vm { get; private set; }

        public ValidateableUnit(IDomainObject entity, ISmartValidateableVm vm)
        {
            Entity = entity;
            Vm = vm;
        }
    }
}