using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;


namespace GU.HQ.DataModel
{
    /// <summary>
    /// информация о адресе проживания/ регистрации человека
    /// </summary>
    [TableName("gu_hq.person_address")]
    public abstract class PersonAddress : IdentityDomainObject<PersonAddress>, IPersistentObject
    {
        /// <summary>
        /// идентификатор записи в таблице
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.person_address_seq")]
        [MapField("person_address_id")]
        public abstract override int Id{get;set;}

      
        /// <summary>
        /// Идентификатор персоны
        /// </summary>
        [MapField("person_id")]
        public abstract int PersonId { get; set; }

        /// <summary>
        /// идентификатор адреса
        /// </summary>
        [MapField("address_id")]
        public abstract int AddressId { get; set; }

        /// <summary>
        /// Информация об адресе
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// тип адреса
        /// </summary>
        [MapField("address_type_id")]
        public abstract AddressType AddressTypeId { get; set; }

        /// <summary>
        /// дата регистрации
        /// </summary>
        [MapField("date_reg")]
        public abstract DateTime? DateReg { get; set; }

        /// <summary>
        /// Дата начала действия записи
        /// </summary>
        [MapField("date1")]
        public abstract DateTime DateStart { get; set; }

        /// <summary>
        /// Датат окончаиня действия записи
        /// </summary>
        [MapField("date2")]
        public abstract DateTime DateEnd { get; set; }
     }
}
