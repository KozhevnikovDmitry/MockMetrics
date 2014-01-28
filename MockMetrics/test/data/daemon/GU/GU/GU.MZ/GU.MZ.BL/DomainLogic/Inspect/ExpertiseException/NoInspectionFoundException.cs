using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    public class NoInspectionFoundException : BLLException
    {
        public NoInspectionFoundException()
            : base("Выездная проверка не найдена")
        {

        }
    }
}