using System;
using System.ComponentModel;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Expertise
{
    public class ExpertiseOrderDataVm : SmartValidateableVm<ExpertiseOrder>
    {
        public override void Initialize(ExpertiseOrder entity)
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
                return "Приказ о преведении документарной проверки";
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

        public string FullName
        {
            get
            {
                return Entity.FullName;
            }
            set
            {
                if (Entity.FullName != value)
                {
                    Entity.FullName = value;
                    RaisePropertyChanged(() => FullName);
                }
            }
        }

        public int TaskId
        {
            get
            {
                return Entity.TaskId;
            }
            set
            {
                if (Entity.TaskId != value)
                {
                    Entity.TaskId = value;
                    RaisePropertyChanged(() => TaskId);
                }
            }
        }

        public int DueDays
        {
            get
            {
                return Entity.DueDays;
            }
            set
            {
                if (Entity.DueDays != value)
                {
                    Entity.DueDays = value;
                    RaisePropertyChanged(() => DueDays);
                }
            }
        }

        public DateTime StartDate
        {
            get
            {
                return Entity.StartDate;
            }
            set
            {
                if (Entity.StartDate != value)
                {
                    Entity.StartDate = value;
                    RaisePropertyChanged(() => StartDate);
                }
            }
        }

        public DateTime DueDate
        {
            get
            {
                return Entity.DueDate;
            }
            set
            {
                if (Entity.DueDate != value)
                {
                    Entity.DueDate = value;
                    RaisePropertyChanged(() => DueDate);
                }
            }
        }

        public DateTime TaskStamp
        {
            get
            {
                return Entity.TaskStamp;
            }
            set
            {
                if (Entity.TaskStamp != value)
                {
                    Entity.TaskStamp = value;
                    RaisePropertyChanged(() => TaskStamp);
                }
            }
        }

        public string Inn
        {
            get
            {
                return Entity.Inn;
            }
            set
            {
                if (Entity.Inn != value)
                {
                    Entity.Inn = value;
                    RaisePropertyChanged(() => Inn);
                }
            }
        }

        public string LicensedActivity
        {
            get
            {
                return Entity.LicensedActivity;
            }
            set
            {
                if (Entity.LicensedActivity != value)
                {
                    Entity.LicensedActivity = value;
                    RaisePropertyChanged(() => LicensedActivity);
                }
            }
        }

        public string ActivityAdditionalInfo
        {
            get
            {
                return Entity.ActivityAdditionalInfo;
            }
            set
            {
                if (Entity.ActivityAdditionalInfo != value)
                {
                    Entity.ActivityAdditionalInfo = value;
                    RaisePropertyChanged(() => ActivityAdditionalInfo);
                }
            }
        }

        public string Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => OrderName);
            RaisePropertyChanged(() => ActivityAdditionalInfo);
        }
    }
}
