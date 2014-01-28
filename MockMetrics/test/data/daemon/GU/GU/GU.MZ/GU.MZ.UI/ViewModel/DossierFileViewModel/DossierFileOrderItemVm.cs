using System;
using Common.DA;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.MZ.DataModel.MzOrder;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    public class DossierFileOrderItemVm : NotificationObject, IAvalonDockCaller
    {
        public IOrder Order { get; private set; }

        public DossierFileOrderItemVm(IOrder order)
        {
            Order = order;
            Order.PropertyChanged += (sender, args) =>
            {
                var name = args.PropertyName;
                if (name == "Stamp")
                {
                    name += "String";
                }
                RaisePropertyChanged(name);
            };
            GoToOrderCommand = new DelegateCommand(GoToOrder, CanGoToOrder);
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
        }

        #region Binding Properties

        public string RegNumber
        {
            get
            {
                return Order.RegNumber;
            }
        }

        public DateTime Stamp
        {
            get
            {
                return Order.Stamp;
            }
        }

        public string StampString
        {
            get
            {
                return string.Format("{0} {1}", Order.Stamp.ToShortDateString(), Order.Stamp.ToLongTimeString());
            }
        }

        public string Name
        {
            get
            {
                return Order.Name;
            }
        }

        #endregion


        #region Binding Commands

        public DelegateCommand GoToOrderCommand { get; private set; }

        private void GoToOrder()
        {
            try
            {
                if (Order is ExpertiseOrder)
                {
                    AvalonInteractor.RaiseOpenEditableDockable(Order.ToString(), typeof(ExpertiseOrder), Order);
                    return;
                }

                if (Order is InspectionOrder)
                {
                    AvalonInteractor.RaiseOpenEditableDockable(Order.ToString(), typeof(InspectionOrder), Order);
                    return;
                }

                if (Order is StandartOrder)
                {
                    AvalonInteractor.RaiseOpenEditableDockable(Order.ToString(), typeof(StandartOrder), Order);
                    return;
                }

                throw new NotSupportedException("Неизвестный тип приказа");
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

        private bool CanGoToOrder()
        {
            try
            {
                return Order.PersistentState == PersistentState.Old;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
                return false;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
                return false;
            }
        }

        #endregion


        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}