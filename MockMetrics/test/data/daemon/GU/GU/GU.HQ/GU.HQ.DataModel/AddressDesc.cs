using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Описание жилья
    /// </summary>
    [TableName("gu_hq.address_desc")]
    public abstract class AddressDesc : IdentityDomainObject<AddressDesc>, IPersistentObject
    {
        /// <summary>
        /// Ссылка на адресс
        /// </summary>
        [PrimaryKey]
        [MapField("address_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        [MapField("floor")]
        public abstract int Floor { get; set; }


        /// <summary>
        /// Колличество комнат
        /// </summary>
        [MapField("room_count")]
        public abstract int RoomCount { get; set; }


        /// <summary>
        /// общая площадь
        /// </summary>
        [MapField("area_general")]
        public abstract decimal AreaGenegal { get; set; }


        /// <summary>
        /// жилая площадь
        /// </summary>
        [MapField("area_living")]
        public abstract decimal AreaLiving { get; set; }

        /// <summary>
        /// Основание проживания в жилом помещении
        /// </summary>
        [MapField("house_doc")]
        public abstract string HouseDoc { get; set; }


        /// <summary>
        /// Тип комфортабельности
        /// </summary>
        [MapField("house_type_comf_id")]
        public abstract HouseTypeComfort HouseComfort { get; set; }


        /// <summary>
        /// Тип жилья по приватности
        /// </summary>
        [MapField("house_type_priv_id")]
        public abstract HouseTypePrivate HousePrivate { get; set; }
    }
}
