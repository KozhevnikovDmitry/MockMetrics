using System;
using GU.DataModel;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    [Serializable]
    public class DefaultMzTaskContext : MzTaskContext
    {
        public override IMzTaskWrapper MzTask(Task task)
        {
            return new MzTaskWrapper(task);
        }
    }
}