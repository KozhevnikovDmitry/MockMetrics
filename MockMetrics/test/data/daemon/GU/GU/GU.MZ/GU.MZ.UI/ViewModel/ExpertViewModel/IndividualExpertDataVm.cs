using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных эксперта физического лица
    /// </summary>
    public class IndividualExpertDataVm : SmartValidateableVm<IndividualExpertState>
    {
        #region Binding Properties

        /// <summary>
        /// Возвращает или устанавливает имя эксперта
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
        /// Возвращает или устанавливает фамилию эксперта
        /// </summary>
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

        /// <summary>
        /// Возвращает или устанавливает отчество эксперта
        /// </summary>
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

        /// <summary>
        /// Возвращает или устаналивает должность эксперта
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
        /// Возвращает или устанавливает наименование организации, к которой принадлежит эксперт
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return Entity.OrganizationName;
            }
            set
            {
                if (Entity.OrganizationName != value)
                {
                    Entity.OrganizationName = value;
                    RaisePropertyChanged(() => OrganizationName);
                }
            }
        }

        #endregion
    }
}
