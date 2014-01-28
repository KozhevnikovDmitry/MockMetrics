using System;
using Common.DA;
using Common.DA.Interface;
using GU.Archive.DataModel;

namespace GU.Archive.BL
{
    public class ArchiveDomainObjectInitializer : AbstractDomainObjectInitializer
    {

        public ArchiveDomainObjectInitializer(string userName)
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
