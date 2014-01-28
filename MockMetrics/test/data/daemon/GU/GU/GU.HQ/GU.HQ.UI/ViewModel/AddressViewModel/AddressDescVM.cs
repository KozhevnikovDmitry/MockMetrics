using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.AddressViewModel
{
    public class AddressDescVM : DomainValidateableVM<AddressDesc>
    {
        public AddressDescVM(AddressDesc entity, IDomainValidator<AddressDesc> domainValidator, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {
            ListHouseComfort = HqFacade.GetDictionaryManager().GetEnumDictionary<HouseTypeComfort>();
            ListHousePrivate = HqFacade.GetDictionaryManager().GetEnumDictionary<HouseTypePrivate>();
        }


        #region Binding Properties

        public Dictionary<int, string> ListHouseComfort { get; private set; }
        public Dictionary<int, string> ListHousePrivate { get; private set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public decimal AreaGenegal
        {
            get { return Entity.AreaGenegal; }
            set { Entity.AreaGenegal = value; }
        }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public decimal AreaLiving
        {
            get { return Entity.AreaLiving; }
            set { Entity.AreaLiving = value; }
        }

        /// <summary>
        /// Этаж
        /// </summary>
        public int Floor
        {
            get { return Entity.Floor; }
            set { Entity.Floor = value; }
        }

        /// <summary>
        /// Комфортабельность
        /// </summary>
        public int HouseComfort
        {
            get { return (int)Entity.HouseComfort; }
            set { Entity.HouseComfort = (HouseTypeComfort)value; }
        }


        /// <summary>
        /// Основание проживания в доме
        /// </summary>
        public string HouseDoc
        {
            get { return Entity.HouseDoc; }
            set { Entity.HouseDoc = value; }
        }

        /// <summary>
        /// Тип жилья по приватности
        /// </summary>
        public int HousePrivate
        {
            get { return (int)Entity.HousePrivate; }
            set { Entity.HousePrivate = (HouseTypePrivate)value; }
        }

        /// <summary>
        /// Кол-во комнат
        /// </summary>
        public int RoomCount
        {
            get { return Entity.RoomCount; }
            set { Entity.RoomCount = value; }
        }

        #endregion Binding Properties
    }
}
