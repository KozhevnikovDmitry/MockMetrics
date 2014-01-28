using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Адреса: регистрация/проживание/ выданное жильё 
    /// </summary>
    [TableName("gu_hq.address")]
    public abstract class Address : IdentityDomainObject<Address>, IPersistentObject
    {
        /// <summary>
        /// Уникальный идентификатор таблицы
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName ("gu_hq.address_seq")]
        [MapField("address_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        [MapField("post_index")]
        public abstract string PostIndex { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [MapField("city")]
        public abstract string City { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [MapField("street")]
        public abstract string Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        [MapField("house_num")]
        public abstract string HouseNum { get; set; }

        /// <summary>
        /// номер корпуса
        /// </summary>
        [MapField("korp_num")]
        public abstract string KorpNum { get; set; }

        /// <summary>
        /// номер квартиры
        /// </summary>
        [MapField("kv_num")]
        public abstract string KvNum { get; set; }

        /// <summary>
        /// Описание жилья
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "AddressId", CanBeNull = true)]
        public abstract AddressDesc AddressDesc { get; set; }
    }
}
