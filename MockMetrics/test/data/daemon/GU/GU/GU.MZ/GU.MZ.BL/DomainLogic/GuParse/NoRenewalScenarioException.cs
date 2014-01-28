using Common.Types.Exceptions;
using GU.DataModel;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    public class NoRenewalScenarioException : BLLException
    {
        public Task Task { get; private set; }

        public NoRenewalScenarioException(Task task)
            : base(string.Format("Не найден сценарий переоформления лицензии в заявке id=[{0}]", task.Id))
        {
            Task = task;
        }
    }
}