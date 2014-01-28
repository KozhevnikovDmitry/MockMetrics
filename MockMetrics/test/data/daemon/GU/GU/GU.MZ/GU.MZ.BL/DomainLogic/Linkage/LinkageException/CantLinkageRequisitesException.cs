using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение для ошибок "Невозможно привязать реквизиты лицензиата к делу тома"
    /// </summary>
    public class CantLinkageRequisitesException : GUException
    {
        public CantLinkageRequisitesException()
            : base("Невозможно привязать реквизиты лицензиата к делу тома")
        {
            
        }
    }
}