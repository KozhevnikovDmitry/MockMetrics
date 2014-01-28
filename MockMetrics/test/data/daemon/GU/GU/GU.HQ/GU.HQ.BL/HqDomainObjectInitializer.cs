using System;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL
{
    public class HqDomainObjectInitializer : AbstractDomainObjectInitializer
    {

        public HqDomainObjectInitializer(string userName)
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
