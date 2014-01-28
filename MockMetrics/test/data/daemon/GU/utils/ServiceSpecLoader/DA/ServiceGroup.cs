using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Mapping;
using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace ServiceSpecLoader
{
    [TableName("SERVICE_GROUP")]
    public class ServiceGroup
    {
        [MapField("SERVICE_GROUP_ID")]
        public int ServiceGroupId { get; set; }
        
        [MapField("SERVICE_GROUP_NAME")]
        public string ServiceGroupName { get; set; }

        [MapField("AGENCY_ID")]
        public int AgencyId { get; set; }

        [NoInstance]
        [Association(ThisKey = "AgencyId", OtherKey = "AgencyId", CanBeNull = false)]
        public Agency Agency { get; set; }

        public override string ToString()
        {
            return ServiceGroupName;
        }
    }

    public abstract class ServiceGroupAccessor : DataAccessor<ServiceGroup, ServiceGroupAccessor>
    {
        [SqlQuery("select * from service_group where service_group_id = :ServiceGroupId")]
        public abstract ServiceGroup SelectByKey(int ServiceGroupId);

        [SqlQuery("select * from service_group where agency_id = :AgencyId order by service_group_name")]
        public abstract List<ServiceGroup> SelectByAgency(int AgencyId);
    }
}
