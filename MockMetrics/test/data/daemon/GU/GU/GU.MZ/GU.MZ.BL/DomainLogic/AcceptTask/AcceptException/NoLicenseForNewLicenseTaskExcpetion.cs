using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    public class NoLicenseForNewLicenseTaskExcpetion : BLLException
    {
        public NoLicenseForNewLicenseTaskExcpetion()
            : base("Невозможно получить данные о существующей лицензии из заявки на предоставление")
        {
            
        }
    }
}