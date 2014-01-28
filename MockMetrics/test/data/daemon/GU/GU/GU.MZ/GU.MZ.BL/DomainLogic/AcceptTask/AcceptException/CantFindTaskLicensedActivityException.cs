using System;

using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// Класс исключение для ошибок "Не удалось найти лицензируемую деятельность, с которой была ассоциирована заявка".
    /// </summary>
    public class CantFindTaskLicensedActivityException : GUException
    {
        public CantFindTaskLicensedActivityException(Exception ex)
            :base("Не удалось найти лицензируемую деятельность, с которой была ассоциирована заявка")
        {
            
        }
    }
}