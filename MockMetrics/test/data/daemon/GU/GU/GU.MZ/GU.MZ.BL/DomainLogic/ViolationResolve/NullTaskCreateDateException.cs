using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.ViolationResolve
{
    public class NullTaskCreateDateException : BLLException
    {
        public int TaskId { get; set; }

        public NullTaskCreateDateException(int taskId)
            : base(string.Format("Не проставлена дата приёма заявления id=[{0}]", taskId))
        {
            TaskId = taskId;
        }
    }
}