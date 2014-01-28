using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.EmployeeViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сотрудника.
    /// </summary>
    public class EmployeeDataVm : SmartValidateableVm<Employee>
    {
        #region Binding Properties

        /// <summary>
        /// Возвращает или устанавливает ФИО сотрудника.
        /// </summary>
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

        /// <summary>
        /// Возвращает или устанавливает телефон сотрудника.
        /// </summary>
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

        /// <summary>
        /// Возвращает или устанавливает должность сотрудника.
        /// </summary>
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

        /// <summary>
        /// Возвращает или устанавливает Email сотрудника.
        /// </summary>
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

        #endregion
    }
}
