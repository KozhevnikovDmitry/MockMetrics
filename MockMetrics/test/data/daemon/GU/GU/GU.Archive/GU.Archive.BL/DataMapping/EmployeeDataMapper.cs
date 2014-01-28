using System;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.Archive.DataModel;

namespace GU.Archive.BL.DataMapping
{
    public class EmployeeDataMapper : AbstractDataMapper<Employee>
    {
        public EmployeeDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override Employee RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            Employee emp = dbManager.RetrieveDomainObject<Employee>(id);
            return emp;
        }

        protected override Employee SaveOperation(Employee obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);
            return tmp;
        }

        protected override void FillAssociationsOperation(Employee obj, IDomainDbManager dbManager)
        {

        }
    }
}
