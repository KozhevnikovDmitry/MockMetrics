using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLToolkit.Mapping;
using BLToolkit.DataAccess;

namespace ServiceSpecLoader
{
    public abstract class MigDataAccessor : DataAccessor
    {
        [DataSetTable("mig_doc_spec"),
         SqlQuery("select * from mig_doc_spec where service_id = :ServiceId order by doc_num")]
        public abstract void SelectDoc(int ServiceId, [Destination] DataSet ds);

        [DataSetTable("mig_doc_sect_spec"),
         SqlQuery("select * from mig_doc_sect_spec where service_id = :ServiceId order by doc_num, nvl(sort_order, doc_sect_num), doc_sect_num")]
        public abstract void SelectDocSect(int ServiceId, [Destination] DataSet ds);

        [DataSetTable("mig_attr_spec"),
         SqlQuery("select * from mig_attr_spec where service_id = :ServiceId order by doc_sect_num, nvl(sort_order, attr_num), attr_num")]
        public abstract void SelectAttr(int ServiceId, [Destination] DataSet ds);

        [DataSetTable("mig_request_spec"),
         SqlQuery("select * from mig_request_spec where service_id = :ServiceId order by request_num")]
        public abstract void SelectRequest(int ServiceId, [Destination] DataSet ds);

        [DataSetTable("mig_request_attr_spec"),
         SqlQuery("select * from mig_request_attr_spec where service_id = :ServiceId order by request_num, rec_num")]
        public abstract void SelectRequestAttr(int ServiceId, [Destination] DataSet ds);
    }
}
