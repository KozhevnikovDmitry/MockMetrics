using System;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InventoryViewModel
{
    public class InventoryDataVm : SmartValidateableVm<DocumentInventory>
    {
        #region Binding Properties

        public int RegNumber
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

        public DateTime? Stamp
        {
            get
            {
                return Entity.Stamp;
            }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value ?? Entity.Stamp;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public string LicenseHolder
        {
            get
            {
                return Entity.LicenseHolder;
            }
            set
            {
                if (Entity.LicenseHolder != value)
                {
                    Entity.LicenseHolder = value;
                    RaisePropertyChanged(() => LicenseHolder);
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

        #endregion
    }
}
