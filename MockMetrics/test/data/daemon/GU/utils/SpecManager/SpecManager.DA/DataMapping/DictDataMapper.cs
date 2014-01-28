using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.DA.DbManagement;

namespace SpecManager.DA.DataMapping
{
    internal class DictDataMapper : AbstractDataMapper<Dict>
    {
        public DictDataMapper(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        protected override Dict RetrieveOperation(int id, IDomainDbManager dbManager)
        {
            throw new System.NotImplementedException();
        }

        protected override Dict SaveOperation(Dict obj, IDomainDbManager dbManager)
        {
            throw new System.NotImplementedException();
        }

        protected override void DeleteOperation(Dict obj, IDomainDbManager dbManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
