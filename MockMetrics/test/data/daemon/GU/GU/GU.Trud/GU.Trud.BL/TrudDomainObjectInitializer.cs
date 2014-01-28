using System;
using Common.DA;
using Common.DA.Interface;
using GU.BL;
using GU.Trud.DataModel;

namespace GU.Trud.BL
{
    public class TrudDomainObjectInitializer : AbstractDomainObjectInitializer
    {

        public TrudDomainObjectInitializer(string userName)
            : base(userName)
        {
        }

        public override void InitializeObject<T>(T obj)
        {
            if (obj is TaskExport)
            {
                var taskExport = obj as TaskExport;
                taskExport.Stamp = DateTime.Now;
            }
        }

        protected override ICommonData CreateCommonData()
        {
            return new CommonData();
        }
    }
}
