using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.Archive.DataModel;

namespace GU.Archive.BL.DataMapping
{
    public class PostExecutorDataMapper : AbstractDataMapper<PostExecutor>
    {
        public PostExecutorDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override PostExecutor RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<PostExecutor>(id);
            obj.Employee = dbManager.RetrieveDomainObject<Employee>(obj.EmployeeId);
            return obj;
        }

        protected override PostExecutor SaveOperation(PostExecutor obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();
            dbManager.SaveDomainObject(tmp);
            return tmp;
        }

        protected override void FillAssociationsOperation(PostExecutor obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
