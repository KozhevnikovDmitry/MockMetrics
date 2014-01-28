using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA;
using Common.DA.Interface;
using GU.DataModel;

namespace GU.Enisey.BL
{
    public class EniseyDomainObjectInitializer : AbstractDomainObjectInitializer
    {

        public EniseyDomainObjectInitializer(string userName)
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
