using System.Threading;
using GU.DataModel;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// Ambient Context для получения MZ-обёртки для заявки
    /// </summary>
    public abstract class MzTaskContext
    {
        private const string ContextSlotName = "MzTaskContext";

        public static MzTaskContext Current
        {
            get
            {
                var ctx =  Thread.GetData(Thread.GetNamedDataSlot(ContextSlotName)) as MzTaskContext;
                if (ctx == null)
                {
                    ctx = new DefaultMzTaskContext();
                    Thread.SetData(Thread.GetNamedDataSlot(ContextSlotName), ctx);
                }
                return ctx;
            }
            set
            {
                Thread.SetData(Thread.GetNamedDataSlot(ContextSlotName), value);
            }
        }

        public abstract IMzTaskWrapper MzTask(Task task);
    }
}