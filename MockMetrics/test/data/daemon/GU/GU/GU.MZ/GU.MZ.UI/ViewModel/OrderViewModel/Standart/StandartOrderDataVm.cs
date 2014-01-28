using System;
using System.ComponentModel;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Standart
{
    public class StandartOrderDataVm : SmartValidateableVm<StandartOrder>
    {
        public override void Initialize(StandartOrder entity)
        {
            if (Entity != null)
            {
                Entity.PropertyChanged -= OrderOnPropertyChanged;
            }
            base.Initialize(entity);
            Entity.PropertyChanged += OrderOnPropertyChanged;
        }

        private void OrderOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RaisePropertyChanged(args.PropertyName);
        }

        #region Binding Properties

        public string OrderName
        {
            get
            {
                return Entity.Name;
            }
        }

        public string RegNumber
        {
            get
            {
                return Entity.RegNumber;
            }
            set
            {
                if (Entity.RegNumber != value)
                {
                    Entity.RegNumber = value;
                    RaisePropertyChanged(() => RegNumber);
                }
            }
        }

        public DateTime Stamp
        {
            get
            {
                return Entity.Stamp;
            }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public string EmployeeName
        {
            get
            {
                return Entity.EmployeeName;
            }
            set
            {
                if (Entity.EmployeeName != value)
                {
                    Entity.EmployeeName = value;
                    RaisePropertyChanged(() => EmployeeName);
                }
            }
        }

        public string EmployeePosition
        {
            get
            {
                return Entity.EmployeePosition;
            }
            set
            {
                if (Entity.EmployeePosition != value)
                {
                    Entity.EmployeePosition = value;
                    RaisePropertyChanged(() => EmployeePosition);
                }
            }
        }

        public string EmployeeContacts
        {
            get
            {
                return Entity.EmployeeContacts;
            }
            set
            {
                if (Entity.EmployeeContacts != value)
                {
                    Entity.EmployeeContacts = value;
                    RaisePropertyChanged(() => EmployeeContacts);
                }
            }
        }

        public string LicensialHeadName
        {
            get
            {
                return Entity.LicensiarHeadName;
            }
            set
            {
                if (Entity.LicensiarHeadName != value)
                {
                    Entity.LicensiarHeadName = value;
                    RaisePropertyChanged(() => LicensialHeadName);
                }
            }
        }

        public string LicensiarHeadPosition
        {
            get
            {
                return Entity.LicensiarHeadPosition;
            }
            set
            {
                if (Entity.LicensiarHeadPosition != value)
                {
                    Entity.LicensiarHeadPosition = value;
                    RaisePropertyChanged(() => LicensiarHeadPosition);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => OrderName);
        }
    }
}
