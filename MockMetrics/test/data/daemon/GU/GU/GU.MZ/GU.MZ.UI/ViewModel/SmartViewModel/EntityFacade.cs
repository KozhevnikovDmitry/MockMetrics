using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Common.BL.DataMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public class EntityFacade : IEntityFacade
    {
        private readonly IEnumerable<IDomainDataMapper> _mappers;
        private readonly IEnumerable<IDomainValidator> _validators;
        private readonly IEnumerable<IDomainObjectEventSubscriber> _subscribers;
        private readonly List<EditableUnit> _editables;

        public EntityFacade(IEnumerable<IDomainDataMapper> mappers,
                            IEnumerable<IDomainValidator> validators,     
                            IEnumerable<IDomainObjectEventSubscriber> subscribers)
        {
            _mappers = mappers;
            _validators = validators;
            _subscribers = subscribers;
            _editables = new List<EditableUnit>();
        }

        #region IEditableFacade

        public void Register<T>(ISmartEditableVm vm, T entity) where T : DomainObject<T>, IPersistentObject
        {
            var unit = new EditableUnit(entity, vm);
            IWeakEventListener listener =
                new WeakEventListener<EventArgs>((sender, args) => OnEditableSourcePropertyChanged(sender, args, unit));
            unit.Listener = listener;
            _editables.Add(unit);
        }

        public void Save<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject
        {
            (vm as SmartEditableVm<T>).Entity =
                _mappers.OfType<IDomainDataMapper<T>>().Single().Save(GetEntity<T>(vm));
        }

        public void Resubscribe<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject
        {
            var listener = _editables.Single(t => t.Vm.Equals(vm)).Listener;
            _subscribers.OfType<IDomainObjectEventSubscriber<T>>()
                .Single()
                .PropertyChangedSubscribe(GetEntity<T>(vm), listener);
        }

        public void Close(ISmartEditableVm vm)
        {
            _editables.RemoveAll(t => t.Vm.Equals(vm));
        }

        public ValidationErrorInfo Validate<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject
        {
            return _validators.OfType<IDomainValidator<T>>().Single().Validate(GetEntity<T>(vm));
        }

        private T GetEntity<T>(ISmartEditableVm vm) where T : DomainObject<T>, IPersistentObject
        {
            return (vm as ISmartEditableVm<T>).Entity;
        }

        private void OnEditableSourcePropertyChanged(object sender, EventArgs args, EditableUnit unit)
        {
            unit.Vm.RaiseIsDirtyChanged();
            if (args is NotifyCollectionChangedEventArgs)
            {
                var collArgs = args as NotifyCollectionChangedEventArgs;
                if (collArgs.NewItems != null)
                {
                    foreach (var item in collArgs.NewItems)
                    {
                        if (item is INotifyPropertyChanged)
                        {
                            PropertyChangedWeakEventManager.AddListener(item as INotifyPropertyChanged, unit.Listener);
                        }
                    }
                }
            }
        }

        #endregion
        
        #region IValidateFacade

        public string Validate<T>(ISmartValidateableVm vm, string columnName) where T : IDomainObject
        {
            return _validators.OfType<IDomainValidator<T>>().Single().ValidateProperty(GetEntity<T>(vm), columnName);
        }

        public ValidationErrorInfo Validate<T>(ISmartValidateableVm vm) where T : IDomainObject
        {
            return _validators.OfType<IDomainValidator<T>>().Single().Validate(GetEntity<T>(vm));
        }
        
        public void RaiseValidatingPropertyChanged<T>(ISmartValidateableVm vm) where T : IDomainObject
        {
            foreach (var validatedName in _validators.OfType<IDomainValidator<T>>().Single().ValidatedNames)
            {
                vm.RaisePropertyChanged(validatedName);
            }
        }

        private T GetEntity<T>(ISmartValidateableVm vm) where T : IDomainObject
        {
            return (vm as ISmartValidateableVm<T>).Entity;
        }

        #endregion
    }
}