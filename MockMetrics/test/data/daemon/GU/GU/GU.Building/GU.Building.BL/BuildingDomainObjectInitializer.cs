using System;
using Common.DA;
using Common.DA.Interface;
using GU.Building.DataModel;

namespace GU.Building.BL
{
    public class BuildingDomainObjectInitializer : AbstractDomainObjectInitializer
    {

        public BuildingDomainObjectInitializer(string userName)
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
