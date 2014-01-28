using System;
using Common.BL.DataMapping;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public abstract class EntityInfoVm<T> : NotificationObject, IEntityInfoVm<T> where T : IPersistentObject
    {
        protected readonly IDomainDataMapper<T> EntityMapper;
        public T Entity { get; private set; }

        protected EntityInfoVm(IDomainDataMapper<T> entityMapper)
        {
            EntityMapper = entityMapper;
            TakeEntityCommand = new DelegateCommand(() => TakeEntity());
        }

        public virtual void Initialize(T entity)
        {
            Entity = entity;
        }

        public event Action<T> OnTakeEntity;

        public DelegateCommand TakeEntityCommand { get; private set; }

        public T TakeEntity()
        {
            try
            {
                var entity =EntityMapper.Retrieve(Entity.GetKeyValue());
                if (OnTakeEntity != null && Entity.PersistentState == PersistentState.Old)
                {
                    OnTakeEntity(entity);
                }
                return entity;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                throw;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                throw;
            }
        }
    }
}