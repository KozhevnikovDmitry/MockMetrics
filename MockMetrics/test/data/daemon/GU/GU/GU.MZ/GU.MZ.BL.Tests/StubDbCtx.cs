using Common.BL.DomainContext;
using Common.DA.Interface;

namespace GU.MZ.BL.Tests
{
    public class StubDbCtx : IDomainContext
    {
        private IDomainDbManager _db;

        public StubDbCtx(IDomainDbManager db)
        {
            _db = db;
        }

        public IDomainDbManager GetDbManager(string contextKey)
        {
            return _db;
        }
    }
}
