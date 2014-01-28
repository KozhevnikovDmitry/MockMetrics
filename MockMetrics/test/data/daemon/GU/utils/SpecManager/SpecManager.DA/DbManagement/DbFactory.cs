using BLToolkit.Data.DataProvider;

using SpecManager.BL.Interface;

namespace SpecManager.DA.DbManagement
{
    internal class DbFactory : IDbFactory
    {
        public string ConnectionString { get; set; }

        public IDomainDbManager GetDbManager()
        {
            return new DomainDbManager(new PostgreSQLDataProvider(), ConnectionString);
        }
    }
}
