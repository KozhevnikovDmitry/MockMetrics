using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonContactInfoVM : DomainValidateableVM<PersonContactInfo>
    {
        public PersonContactInfoVM(PersonContactInfo entity, IDomainValidator<PersonContactInfo> validator, bool isValidateable = true)
            : base(entity, validator, isValidateable)
        { }


        #region Binding Properties

        public string PhoneHome
        {
            get { return Entity == null ? "" :  Entity.PhoneHome; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Entity.PhoneHome.Equals(value))
                    Entity.PhoneHome = value;
            }
        }

        public string PhoneWork
        {
            get { return Entity == null ? "" : Entity.PhoneWork; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Entity.PhoneWork.Equals(value))
                    Entity.PhoneWork = value;
            }
        }

        public string PhoneMobile
        {
            get { return Entity == null ? "" : Entity.PhoneMobile; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Entity.PhoneMobile.Equals(value))
                    Entity.PhoneMobile = value;
            }
        }

        public string EMail
        {
            get { return Entity == null ? "" : Entity.EMail; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !Entity.EMail.Equals(value))
                    Entity.EMail = value;
            }
        }

        #endregion Binding Properties
    }
}
