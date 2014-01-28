using GU.MZ.DataModel;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel
{
    /// <summary>
    /// Класс ViewModel для редактирования данных сущности Адрес
    /// </summary>
    public class AddressVm : SmartValidateableVm<Address>
    {
        public override void Initialize(Address entity)
        {
            base.Initialize(entity);
            NotReadyToValidate();
        }

        #region Binding Properties

        public string Country
        {
            get 
            {
                return Entity.Country;
            }
            set
            {
                if (Entity.Country != value)
                {
                    Entity.Country = value;
                    RaisePropertyChanged(() => Country);
                }
            }
        }

        public string CountryRegion
        {
            get
            {
                return Entity.CountryRegion;
            }
            set
            {
                if (Entity.CountryRegion != value)
                {
                    Entity.CountryRegion = value;
                    RaisePropertyChanged(() => CountryRegion);
                }
            }
        }

        public string Area
        {
            get
            {
                return Entity.Area;
            }
            set
            {
                if (Entity.Area != value)
                {
                    Entity.Area = value;
                    RaisePropertyChanged(() => Area);
                }
            }
        }

        public string City
        {
            get
            {
                return Entity.City;
            }
            set
            {
                if (Entity.City != value)
                {
                    Entity.City = value;
                    RaisePropertyChanged(() => City);
                }
            }
        }

        public string Zip
        {
            get
            {
                return Entity.Zip;
            }
            set
            {
                if (Entity.Zip != value)
                {
                    Entity.Zip = value;
                    RaisePropertyChanged(() => Zip);
                }
            }
        }

        public string Street
        {
            get
            {
                return Entity.Street;
            }
            set
            {
                if (Entity.Street != value)
                {
                    Entity.Street = value;
                    RaisePropertyChanged(() => Street);
                }
            }
        }

        public string House
        {
            get
            {
                return Entity.House;
            }
            set
            {
                if (Entity.House != value)
                {
                    Entity.House = value;
                    RaisePropertyChanged(() => House);
                }
            }
        }

        public string Build
        {
            get
            {
                return Entity.Build;
            }
            set
            {
                if (Entity.Build != value)
                {
                    Entity.Build = value;
                    RaisePropertyChanged(() => Build);
                }
            }
        }

        public string Flat
        {
            get
            {
                return Entity.Flat;
            }
            set
            {
                if (Entity.Flat != value)
                {
                    Entity.Flat = value;
                    RaisePropertyChanged(() => Flat);
                }
            }
        }

        public string Note
        {
            get
            {
                return Entity.Note;
            }
            set
            {
                if (Entity.Note != value)
                {
                    Entity.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => Country);
            RaisePropertyChanged(() => CountryRegion);
            RaisePropertyChanged(() => Area);
            RaisePropertyChanged(() => Build);
            RaisePropertyChanged(() => Flat);
            RaisePropertyChanged(() => Note);
        }
    }
}
