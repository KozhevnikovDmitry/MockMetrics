using System;
using System.Linq;
using System.Windows;
using BLToolkit.EditableObjects;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Event;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ListViewModel;

namespace GU.MZ.UI.ViewModel.SmartViewModel
{
    public interface ISmartListVm
    {
        void SetUiFactory(IListVmUiFactory uiFactory);
    }

    public interface ISmartListVm<T> : ISmartListVm, IListVM<T> where T : IDomainObject
    {
        void Initialize(EditableList<T> items);
    }

    public abstract class SmartListVm<T> : AbstractListVM<T>, ISmartListVm<T> where T : IDomainObject
    {
        private IListVmUiFactory _uiFactory;

        protected EditableList<T> Items;

        protected SmartListVm()
            : base(new object())
        {

        }

        public virtual void Initialize(EditableList<T> items)
        {
            if (Items != null)
                Items.CollectionChanged -= OnListItemsChanged;
            Items = items;
            Items.CollectionChanged += OnListItemsChanged;
            CreateListItems(Items);
            SetListOptions();
            CreateListVmView();
            RaisePropertyChanged(() => AddItemCommand);
            RaisePropertyChanged(() => RemoveItemCommand);
            AddItemCommand.RaiseCanExecuteChanged();
            RemoveItemCommand.RaiseCanExecuteChanged();
        }

        public virtual void SetUiFactory(IListVmUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        protected override IListItemVM<T> CreateItemVm(T item)
        {
            return _uiFactory.GetListItemVm(item);
        }

        protected override void OnRemoveItemChanged(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                if (NoticeUser.ShowQuestionYesNo("Вы действительно хотите удалить элемент из списка") ==
                    MessageBoxResult.Yes)
                {
                    Items.Remove(e.Result);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }

        public override void RaiseItemsValidatingPropertyChanged()
        {
            foreach (var listItemVM in ListItemVMs.Cast<ISmartValidateableVm>())
            {
                listItemVM.RaiseAllPropertyChanged();
            }
        }

        public override void RaiseIsValidChanged()
        {
            base.RaiseIsValidChanged();
            foreach (var listItemVM in ListItemVMs.Cast<ISmartValidateableVm>())
            {
                listItemVM.RaiseIsValidChanged();
            }
            RaiseItemsValidatingPropertyChanged();
        }
    }
}