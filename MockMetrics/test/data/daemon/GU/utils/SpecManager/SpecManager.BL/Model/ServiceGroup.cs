using System.Collections.Generic;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using SpecManager.BL.Interface;

namespace SpecManager.BL.Model
{
    [TableName("gu.service_group")]
    public class ServiceGroup : IDomainObject
    {
        /// <summary>
        /// Первичный ключ сущности
        /// </summary>
        [PrimaryKey]
        [MapField("service_group_id")]
        public int Id { get; set; }

        ///<summary>
        /// Наименование группы услуг
        ///</summary>
        [MapField("service_group_name")]
        public string ServiceGroupName { get; set; }

        [MapField("service_group_long_name")]
        public string LongName { get; set; }

        [MapField("agency_id")]
        public int AgencyId { get; set; }

        [NoInstance]
        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = false)]
        public Agency Agency { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ServiceGroupId")]
        public List<Service> ServiceList { get; set; }
    }
}
