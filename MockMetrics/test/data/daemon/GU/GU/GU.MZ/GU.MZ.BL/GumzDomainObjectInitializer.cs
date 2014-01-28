using Common.DA;
using Common.DA.Interface;
using GU.MZ.DataModel;

namespace GU.MZ.BL
{
    public class GumzDomainObjectInitializer : AbstractDomainObjectInitializer
    {
        public GumzDomainObjectInitializer(string userName)
            : base(userName)
        {
        }

        public override void InitializeObject<T>(T obj)
        {
            
        }

        protected override ICommonData CreateCommonData()
        {
            return new CommonData();
        }
    }
}
