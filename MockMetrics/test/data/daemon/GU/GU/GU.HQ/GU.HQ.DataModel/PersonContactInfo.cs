using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.HQ.DataModel
{
    [TableName("gu_hq.person_contact_info")]
    public abstract class PersonContactInfo : IdentityDomainObject<PersonContactInfo>, IPersistentObject
    {
        /// <summary>
        /// идентификатор записи
        /// </summary>
        [PrimaryKey]
        [MapField("person_id")]
        public abstract override int Id{get;set;}
        
        /// <summary>
        /// домашний телефон
        /// </summary>
        [MapField("phone_home")]
        public abstract string PhoneHome { get; set; }

        /// <summary>
        /// Рабочий телефон
        /// </summary>
        [MapField("phone_work")]
        public abstract string PhoneWork { get; set; }

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        [MapField("phone_mobile")]
        public abstract string PhoneMobile { get; set; }

        /// <summary>
        /// электронная почта
        /// </summary>
        [MapField("email")]
        public abstract string EMail { get; set; }
    }
}
