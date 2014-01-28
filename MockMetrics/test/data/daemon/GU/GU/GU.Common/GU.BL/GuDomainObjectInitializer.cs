using System;
using Common.DA;
using Common.DA.Interface;
using GU.DataModel;

namespace GU.BL
{

    public class GuDomainObjectInitializer : AbstractDomainObjectInitializer
    {
        public GuDomainObjectInitializer(string userName)
            :base(userName)
        {
        }

        #region Overrides of AbstractDomainObjectInitializer

        public override void InitializeObject<T>(T obj)
        {
            
        }

        protected override ICommonData CreateCommonData()
        {
            return new CommonData();
        }

        #endregion
    }
}
