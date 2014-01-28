using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.HQ.DataModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.HQ.UI.ViewModel.AddressViewModel
{
    public class AddressFullVM : NotificationObject
    {
        private Address _address;

        public AddressFullVM (Address address)
        {
            _address = address;
            AddAddressDescCommand = new DelegateCommand(AddAddressDesc);

            AddressVm = UIFacade.GetDomainValidateableVM(address);
            
            if (address.AddressDesc != null)
                AddressDescVm = UIFacade.GetDomainValidateableVM(address.AddressDesc);
        }

        public void RaiseValidatingPropertyChanged()
        {
            AddressVm.RaiseValidatingPropertyChanged();
            AddressDescVm.RaiseValidatingPropertyChanged();
        }

        #region BindingCommand



        #endregion BindigCommand

        public DelegateCommand AddAddressDescCommand { get; private set; }
        private void AddAddressDesc()
        {
            _address.AddressDesc = AddressDesc.CreateInstance();
            AddressDescVm = UIFacade.GetDomainValidateableVM(_address.AddressDesc);
            RaisePropertyChanged(() => NoAddressDesc);
            RaisePropertyChanged(() => HasAddressDesc);
        }

        #region BindingProperties

        public bool NoAddressDesc{ get { return (_address == null || _address.AddressDesc == null); } }

        public bool HasAddressDesc{ get { return !NoAddressDesc; } }

        /// <summary>
        /// Адрес
        /// </summary>
        private IDomainValidateableVM<Address> _addressVm;
        public IDomainValidateableVM<Address> AddressVm
        {
            get{ return _addressVm; }
            set
            {
                if (_addressVm == value) return;
                _addressVm = value;
                RaisePropertyChanged(() => AddressVm);
            }
        }

        /// <summary>
        /// Описание жилого помещения
        /// </summary>
        private IDomainValidateableVM<AddressDesc> _addressDescVm;
        public IDomainValidateableVM<AddressDesc> AddressDescVm
        {
            get { return _addressDescVm; }
            set
            {
                if (_addressDescVm == value) return;
                _addressDescVm = value;
                RaisePropertyChanged(() => AddressDescVm);
            }
        }

        #endregion BindingProperties
    }
}
