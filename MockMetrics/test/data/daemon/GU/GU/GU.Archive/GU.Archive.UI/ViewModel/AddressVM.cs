using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel
{
    public class AddressVM : ValidateableVM
    {

        public AddressVM(Address address, bool isValidateable = true)
            : base(isValidateable)
        {
            Address = address;
            AllowValidate = false;
        }

        public Address Address { get; set; }

        #region Binding Commands

        #endregion

        #region Binding Properties

        public string Country
        {
            get 
            {
                return Address.Country;
            }
            set
            {
                if (Address.Country != value)
                {
                    Address.Country = value;
                    RaisePropertyChanged(() => Country);
                }
            }
        }

        public string CountryRegion
        {
            get
            {
                return Address.CountryRegion;
            }
            set
            {
                if (Address.CountryRegion != value)
                {
                    Address.CountryRegion = value;
                    RaisePropertyChanged(() => CountryRegion);
                }
            }
        }

        public string Area
        {
            get
            {
                return Address.Area;
            }
            set
            {
                if (Address.Area != value)
                {
                    Address.Area = value;
                    RaisePropertyChanged(() => Area);
                }
            }
        }

        public string City
        {
            get
            {
                return Address.City;
            }
            set
            {
                if (Address.City != value)
                {
                    Address.City = value;
                    RaisePropertyChanged(() => City);
                }
            }
        }

        public string Zip
        {
            get
            {
                return Address.Zip;
            }
            set
            {
                if (Address.Zip != value)
                {
                    Address.Zip = value;
                    RaisePropertyChanged(() => Zip);
                }
            }
        }

        public string Street
        {
            get
            {
                return Address.Street;
            }
            set
            {
                if (Address.Street != value)
                {
                    Address.Street = value;
                    RaisePropertyChanged(() => Street);
                }
            }
        }

        public string House
        {
            get
            {
                return Address.House;
            }
            set
            {
                if (Address.House != value)
                {
                    Address.House = value;
                    RaisePropertyChanged(() => House);
                }
            }
        }

        public string Build
        {
            get
            {
                return Address.Build;
            }
            set
            {
                if (Address.Build != value)
                {
                    Address.Build = value;
                    RaisePropertyChanged(() => Build);
                }
            }
        }

        public string Flat
        {
            get
            {
                return Address.Flat;
            }
            set
            {
                if (Address.Flat != value)
                {
                    Address.Flat = value;
                    RaisePropertyChanged(() => Flat);
                }
            }
        }

        public string Note
        {
            get
            {
                return Address.Note;
            }
            set
            {
                if (Address.Note != value)
                {
                    Address.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }       

        private bool _isEditable = true;

        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                if (_isEditable != value)
                {
                    _isEditable = value;
                    RaisePropertyChanged(() => IsEditable);
                }
            }
        }

        #endregion
    }
}
