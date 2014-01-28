using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибки "Попытка заведения дублирующего лицензионного дела"
    /// </summary>
    public class DuplicateDossierException : GUException
    {
        public DuplicateDossierException()
            : base("Попытка заведения дублирующего лицензионного дела.")
        {
            
        }
    }
}
