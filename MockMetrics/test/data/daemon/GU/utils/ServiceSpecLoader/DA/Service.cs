using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Mapping;
using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace ServiceSpecLoader
{
    [TableName("SERVICE")]
    public class Service
    {
        [MapField("SERVICE_ID")]
        public int ServiceId { get; set; }
        
        [MapField("SERVICE_NAME")]
        public string ServiceName { get; set; }
        
        [MapField("SERVICE_GROUP_ID")]
        public int ServiceGroupId { get; set; }

        [NoInstance]
        [Association(ThisKey = "ServiceGroupId", OtherKey = "ServiceGroupId", CanBeNull = false)]
        public ServiceGroup ServiceGroup { get; set; }

        public override string ToString()
        {
            if (ServiceGroup == null)
                return ServiceName;
            else
                return ServiceName + " - " + ServiceGroup.ServiceGroupName;
        }
    }

    public abstract class ServiceAccessor : DataAccessor<Service, ServiceAccessor>
    {
        [SqlQuery("select * from service where service_id = :ServiceId")]
        public abstract Service SelectByKey(int ServiceId);

        [SqlQuery("select * from service where service_group_id = :ServiceGroupId order by sort_order, service_name")]
        public abstract List<Service> SelectByServiceGroup(int ServiceGroupId);
    }
}
