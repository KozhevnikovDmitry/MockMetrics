using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.MzOrder;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    public class DossierFileOrdersVm : NotificationObject, IAvalonDockCaller
    {
        private DossierFile _dossierFile;

        public DossierFileOrdersVm()
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
        }

        public void Initialize(DossierFile dossierFile)
        {
            if (_dossierFile != null)
            {
                _dossierFile.Orders.CollectionChanged -= OrdersOnCollectionChanged;
            }

            _dossierFile = dossierFile;
            _dossierFile.CollectOrders();
            BuildOrderItems(_dossierFile.Orders);
            _dossierFile.Orders.CollectionChanged += OrdersOnCollectionChanged;
        }

        private void BuildOrderItems(IEnumerable<IOrder> orders)
        {
            try
            {
                OrderVms = new ObservableCollection<DossierFileOrderItemVm>(orders.Select(BuildOrderItemVm));
                OrderVmsView = new ListCollectionView(OrderVms);
                OrderVmsView.SortDescriptions.Add(new SortDescription("Stamp", ListSortDirection.Ascending));
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        private void OrdersOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            try
            {
                if (notifyCollectionChangedEventArgs.NewItems != null)
                {
                    foreach (var order in notifyCollectionChangedEventArgs.NewItems.Cast<IOrder>())
                    {
                        var removeItem = OrderVms.SingleOrDefault(t => t.Order == order);

                        if (removeItem != null)
                        {
                            throw new VMException("Добавлямый приказ уже находится в списке");
                        }

                        OrderVms.Add(BuildOrderItemVm(order));
                    }
                }

                if (notifyCollectionChangedEventArgs.OldItems != null)
                {
                    foreach (var order in notifyCollectionChangedEventArgs.OldItems.Cast<IOrder>())
                    {
                        var removeItem = OrderVms.SingleOrDefault(t => t.Order == order);

                        if (removeItem == null)
                        {
                            throw new VMException("Удаляемый приказ в списке не найден");
                        }
                        OrderVms.Remove(removeItem);
                    }
                }
                RaisePropertyChanged(() => OrderVms);
                RaisePropertyChanged(() => OrderVmsView);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        private DossierFileOrderItemVm BuildOrderItemVm(IOrder order)
        {
            var itemVm = new DossierFileOrderItemVm(order);
            AvalonInteractor.RegisterCaller(itemVm);
            return itemVm;
        }

        #region Binding Properties

        private ObservableCollection<DossierFileOrderItemVm> _OrderVms;
        
        public ObservableCollection<DossierFileOrderItemVm> OrderVms
        {
            get
            {
                return _OrderVms;
            }
            set
            {
                if (_OrderVms != value)
                {
                    _OrderVms = value;
                    RaisePropertyChanged(() => OrderVms);
                    RaisePropertyChanged(() => OrderVmsView);
                }
            }
        }

        public ICollectionView OrderVmsView { get; private set; }

        #endregion


        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion

    }
}