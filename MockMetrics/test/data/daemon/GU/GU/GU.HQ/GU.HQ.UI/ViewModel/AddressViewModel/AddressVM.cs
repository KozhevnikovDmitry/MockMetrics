using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.AddressViewModel
{
    public class AddressVM : DomainValidateableVM<Address>
    {
        public AddressVM(Address entity, IDomainValidator<Address> domainValidator, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        { }

        #region Binding Properties

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostIndex
        {
            get { return Entity.PostIndex; }
            set { Entity.PostIndex = value;}
        }

        /// <summary>
        /// Город
        /// </summary>
        public string City
        {
            get { return Entity.City; }
            set { Entity.City = value; }
        }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street
        {
            get { return Entity.Street; }
            set { Entity.Street = value; }
        }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum
        {
            get { return Entity.HouseNum; }
            set { Entity.HouseNum = value; }
        }

        /// <summary>
        /// Номер корпуса
        /// </summary>
        public string KorpNum
        {
            get { return Entity.KorpNum; }
            set { Entity.KorpNum = value; }
        }

        /// <summary>
        /// Номер квартиры
        /// </summary>
        public string KvNum
        {
            get { return Entity.KvNum; }
            set { Entity.KvNum = value; }
        }

        #endregion Binding Properties
    }
}
