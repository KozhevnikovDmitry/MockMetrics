using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

using GU.Archive.BL;
using GU.Archive.DataModel;
using GU.Archive.UI.ViewModel;
using GU.BL;
using GU.DataModel;

namespace GU.Archive.UI.ViewModel.EmployeeViewModel
{
    public class EmployeeDataVM : ValidateableVM
    {
        public Employee Entity { get; protected set; }

        public EmployeeDataVM(Employee employee, bool isValidateable = true)
            : base(isValidateable)
        {
            AllowValidate = false;
            Entity = employee;
            AgencyList = GuFacade.GetDictionaryManager().GetDictionary<Agency>();
        }

        #region Binding Properties

        public string Name
        {
            get
            {
                return Entity.Name;
            }
            set
            {
                if (Entity.Name != value)
                {
                    Entity.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public string Surname
        {
            get
            {
                return Entity.Surname;
            }
            set
            {
                if (Entity.Surname != value)
                {
                    Entity.Surname = value;
                    RaisePropertyChanged(() => Surname);
                }
            }
        }

        public string Patronymic
        {
            get
            {
                return Entity.Patronymic;
            }
            set
            {
                if (Entity.Patronymic != value)
                {
                    Entity.Patronymic = value;
                    RaisePropertyChanged(() => Patronymic);
                }
            }
        }

        public string Phone
        {
            get
            {
                return Entity.Phone;
            }
            set
            {
                if (Entity.Phone != value)
                {
                    Entity.Phone = value;
                    RaisePropertyChanged(() => Phone);
                }
            }
        }

        public string Position
        {
            get
            {
                return Entity.Position;
            }
            set
            {
                if (Entity.Position != value)
                {
                    Entity.Position = value;
                    RaisePropertyChanged(() => Position);
                }
            }
        }

        public string Email
        {
            get
            {
                return Entity.Email;
            }
            set
            {
                if (Entity.Email != value)
                {
                    Entity.Email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        public int AgencyId
        {
            get
            {
                return Entity.AgencyId;
            }
            set
            {
                if (Entity.AgencyId != value)
                {
                    Entity.AgencyId = value;
                    RaisePropertyChanged(() => AgencyId);
                }
            }
        }

        public List<Agency> AgencyList { get; private set; }

        #endregion
    }
}
