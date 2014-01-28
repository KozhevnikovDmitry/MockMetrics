using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.HQ.DataModel;
using GU.HQ.UI.ViewModel.PersonViewModel;


namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerSummaryVm : NotificationObject
    {
        private Person _person;

        public DeclarerSummaryVm(Person person)
        {
            _person = person;

            PersonInfoVm = UIFacade.GetDomainValidateableVM(person);
            PersonContactInfoVm = UIFacade.GetDomainValidateableVM(person.ContactInfo);
            PersonDocsVm = new PersonDocsVM(person);
            PersonAddressesVm = new PersonAddressesVM(person);
            PersonDisabilityVm = UIFacade.GetDomainValidateableVM(person.Disability);

            AddDeclarerContactInfoCommand = new DelegateCommand(AddDeclarerContactInfo);
            AddDeclarerDisabilityCommand = new DelegateCommand(AddDeclarerDisability);

            /*
            if (person.Disability == null)
            {
                person.Disability = PersonDisability.CreateInstance();
                person.AcceptMemberChanges(Util.GetPropertyName(() => person.Disability));
            }
            PersonDisabilityVm = new PersonDisabilityVM(person, HqFacade.GetValidator<PersonDisability>());
             */
        }


        /// <summary>
        /// обновиться с учетом информации о валидации объектов
        /// </summary>
        public void RaiseItemsValidatingPropertyChanged()
        {
            PersonInfoVm.RaiseValidatingPropertyChanged();
            PersonContactInfoVm.RaiseValidatingPropertyChanged();
            PersonDocsVm.RaiseValidatingPropertyChanged();
            PersonAddressesVm.RaiseValidatingPropertyChanged();
            PersonDisabilityVm.RaiseValidatingPropertyChanged();
        }

        #region BindingCommand

        /// <summary>
        /// Добавиь контактную информацию
        /// </summary>
        public DelegateCommand AddDeclarerContactInfoCommand { get; private set; }
        private void AddDeclarerContactInfo()
        {
            _person.ContactInfo = PersonContactInfo.CreateInstance();
            PersonContactInfoVm = UIFacade.GetDomainValidateableVM(_person.ContactInfo);
            RaisePropertyChanged(() => NoContactInfo);
            RaisePropertyChanged(() => HasContactInfo);
        }

        /// <summary>
        /// Добавить информацию об инвалидности
        /// </summary>
        public DelegateCommand AddDeclarerDisabilityCommand { get; private set; }
        private void AddDeclarerDisability()
        {
            _person.Disability = PersonDisability.CreateInstance();
            PersonDisabilityVm = UIFacade.GetDomainValidateableVM(_person.Disability);
            RaisePropertyChanged(() => NoDisability);
            RaisePropertyChanged(() => HasDisability);
        }

        #endregion BindingCommand


        #region BindingProperties

        #region Common

        public bool NoContactInfo { get { return _person.ContactInfo == null; } }

        public bool HasContactInfo { get { return !NoContactInfo; } }

        public bool NoDisability { get { return _person.Disability == null; } }

        public bool HasDisability { get { return !NoDisability; } }

        #endregion Common

        /// <summary>
        /// основная информация по заявителю
        /// </summary>
        private IDomainValidateableVM<Person> _personInfoVm;
        public IDomainValidateableVM<Person> PersonInfoVm
        {
            get { return _personInfoVm; }
            set
            {
                if (_personInfoVm == value) return;
                _personInfoVm = value;
                RaisePropertyChanged(() => PersonInfoVm);
            }
        }

        /// <summary>
        /// контактная информация по заявителю
        /// </summary>
        private IDomainValidateableVM<PersonContactInfo> _personContactInfoVm;
        public IDomainValidateableVM<PersonContactInfo> PersonContactInfoVm
        {
            get { return _personContactInfoVm; }
            set
            {
                if (_personContactInfoVm == value) return;
                _personContactInfoVm = value;
                RaisePropertyChanged(() => PersonContactInfoVm);
            }
        }

        /// <summary>
        /// документы заявителя
        /// </summary>
        private PersonDocsVM _personDocsVm;
        public PersonDocsVM PersonDocsVm
        {
            get { return _personDocsVm; }
            set
            {
                if (_personDocsVm == value) return;
                _personDocsVm = value;
                RaisePropertyChanged(() => PersonDocsVm);
            }
        }

        /// <summary>
        /// документы заявителя
        /// </summary>
        private PersonAddressesVM _personAddressesVm;
        public PersonAddressesVM PersonAddressesVm
        {
            get { return _personAddressesVm; }
            set
            {
                if (_personAddressesVm == value) return;
                _personAddressesVm = value;
                RaisePropertyChanged(() => PersonAddressesVm);
            }
        }

        /// <summary>
        /// информация об инвалидности по заявителю
        /// </summary>
        private IDomainValidateableVM<PersonDisability> _personDisabilityVm;
        public IDomainValidateableVM<PersonDisability> PersonDisabilityVm
        {
            get { return _personDisabilityVm; }
            set
            {
                if (_personDisabilityVm == value) return;
                _personDisabilityVm = value;
                RaisePropertyChanged(() => PersonDisabilityVm);
            }
        }

        #endregion BindingProperties
    }
}