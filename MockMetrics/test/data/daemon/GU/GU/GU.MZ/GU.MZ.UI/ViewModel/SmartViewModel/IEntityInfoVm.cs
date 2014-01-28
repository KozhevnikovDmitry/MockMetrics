using System;
using Common.DA.Interface;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public interface IEntityInfoVm<T> where T : IDomainObject
    {
        T Entity { get; }

        void Initialize(T entity);

        event Action<T> OnTakeEntity;

        DelegateCommand TakeEntityCommand { get; }

        T TakeEntity();
    }
}