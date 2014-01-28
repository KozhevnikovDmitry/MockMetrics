using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Inspect.ExpertiseException
{
    public class AddEmployeeBeforePrepareInspectionException : BLLException
    {
        public AddEmployeeBeforePrepareInspectionException()
            : base("Попытка добавить сотрудника в выездную проверку до заведения самой проверки")
        {

        }
    }
}