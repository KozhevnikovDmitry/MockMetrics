using System;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class IndRequisitesVm : DomainValidateableVM<IndRequisites>
    {
        public IndRequisitesVm(IndRequisites entity, IDomainValidator<IndRequisites> domainValidator, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {
        }

        #region Binding Properties

        public string Name
        {
            get { return Entity.Name; }
            set
            {
                if (Entity.Name != value)
                {
                    Entity.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public string Patronymic
        {
            get { return Entity.Patronymic; }
            set
            {
                if (Entity.Patronymic != value)
                {
                    Entity.Patronymic = value;
                    RaisePropertyChanged(() => Patronymic);
                }
            }
        }

        public string Surname
        {
            get { return Entity.Surname; }
            set
            {
                if (Entity.Surname != value)
                {
                    Entity.Surname = value;
                    RaisePropertyChanged(() => Surname);
                }
            }
        }

        public string Serial
        {
            get { return Entity.Serial; }
            set
            {
                if (Entity.Serial != value)
                {
                    Entity.Serial = value;
                    RaisePropertyChanged(() => Serial);
                }
            }
        }

        public string Number
        {
            get { return Entity.Number; }
            set
            {
                if (Entity.Number != value)
                {
                    Entity.Number = value;
                    RaisePropertyChanged(() => Number);
                }
            }
        }

        public DateTime Stamp
        {
            get { return Entity.Stamp; }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public string Organization
        {
            get { return Entity.Organization; }
            set
            {
                if (Entity.Organization != value)
                {
                    Entity.Organization = value;
                    RaisePropertyChanged(() => Organization);
                }
            }
        }

        public string Note
        {
            get { return Entity.Note; }
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
    }
}