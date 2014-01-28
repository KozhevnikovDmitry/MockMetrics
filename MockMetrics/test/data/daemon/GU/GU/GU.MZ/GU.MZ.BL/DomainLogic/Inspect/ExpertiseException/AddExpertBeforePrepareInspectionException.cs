using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    public class AddExpertBeforePrepareInspectionException : BLLException
    {
        public AddExpertBeforePrepareInspectionException()
            : base("Попытка добавить эксперта в выездную проверку до заведения самой проверки")
        {

        }
    }
}