using System;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using GU.HQ.UI.ViewModel.AddressViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonAddressesVM : NotificationObject
    {
        private Person _person;
        private PersonAddress _personAddressReg;

        public PersonAddressesVM(Person person)
        {
            _person = person;
            AddAddressRegCommand = new DelegateCommand(AddAddressReg);

            foreach (var perAddress in person.Addresses)
            {
                if(perAddress.AddressTypeId == AddressType.Residence)
                    AddressFullLiveVm = new AddressFullVM(perAddress.Address);
                if (perAddress.AddressTypeId == AddressType.Registration)
                {
                    AddressFullRegVm = new AddressFullVM(perAddress.Address);
                    _personAddressReg = perAddress;
                }
            }
        }

        #region Common

        /// <summary>
        /// Обновление Vm после валижации
        /// </summary>
        public void RaiseValidatingPropertyChanged()
        {
            AddressFullLiveVm.RaiseValidatingPropertyChanged();
            AddressFullRegVm.RaiseValidatingPropertyChanged();
        }

        #endregion Common


        #region BindingCommand

        public DelegateCommand AddAddressRegCommand { get; private set; }
        private void AddAddressReg()
        {
            _personAddressReg = PersonAddress.CreateInstance();
            _personAddressReg.Address = Address.CreateInstance();
            _personAddressReg.AddressTypeId = AddressType.Registration;

            _person.Addresses.Add(_personAddressReg);
            AddressFullRegVm = new AddressFullVM(_personAddressReg.Address);

            RaisePropertyChanged(() => NoAddressReg);
            RaisePropertyChanged(() => HasAddressReg);
        }

        #endregion BindingCommand


        #region BindingProperties

        public bool NoAddressReg { get { return _personAddressReg == null; } }

        public bool HasAddressReg { get { return !NoAddressReg; } }

        public DateTime? DateReg 
        {
            get { return _personAddressReg == null ? null : _personAddressReg.DateReg; }
            set { _personAddressReg.DateReg = value; }
        }
        

        private AddressFullVM _addressFullLiveVm;
        public AddressFullVM AddressFullLiveVm
        {
            get { return _addressFullLiveVm; }
            set
            {
                if (_addressFullLiveVm == value) return;
                _addressFullLiveVm = value;
                RaisePropertyChanged(() => AddressFullLiveVm);
            }
        }

        private AddressFullVM _addressFullRegVm;
        public AddressFullVM AddressFullRegVm
        {
            get { return _addressFullRegVm; }
            set
            {
                if (_addressFullRegVm == value) return;
                _addressFullRegVm = value;
                RaisePropertyChanged(() => AddressFullRegVm);
            }
        }

        #endregion BindingProperties
    }
}
