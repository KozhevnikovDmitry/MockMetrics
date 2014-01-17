using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel
{
    [TableName("gu.service_group")]
    public abstract class ServiceGroup : IdentityDomainObject<ServiceGroup>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности
        /// </summary>
        [PrimaryKey]
        [MapField("service_group_id")]
        public abstract override int Id { get; set; }

        ///<summary>
        /// Наименование группы услуг
        ///</summary>
        [MapField("service_group_name")]
        public abstract string ServiceGroupName { get; set; }

        [MapField("service_group_long_name")]
        public abstract string LongName { get; set; }

        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }
        
        [Association(ThisKey = "Id", OtherKey = "ServiceGroupId")]
        public List<Service> ServiceList { get; set; }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual bool IsOnlyForJuridical
        {
            get
            {
                // Только услуги по наркотической деятельности не могут выполняться индивидуальными предпринимателями
                return Id == 1;
            }
        }
    }
}
