using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Mapping;
using BLToolkit.DataAccess;

namespace ServiceSpecLoader
{
    [TableName("AGENCY")]
    public class Agency
    {
        [MapField("AGENCY_ID")]
        public int AgencyId { get; set; }
        [MapField("AGENCY_NAME")]
        public string AgencyName { get; set; }

        public override string ToString()
        {
            return AgencyName;
        }
    }

    public abstract class AgencyAccessor : DataAccessor<Agency, AgencyAccessor>
    {
        [SqlQuery("select * from agency where agency_id = :AgencyId")]
        public abstract Agency SelectByKey(int AgencyId);
        
        [SqlQuery("select * from agency order by agency_name")]
        public abstract List<Agency> SelectAll();
    }
}
