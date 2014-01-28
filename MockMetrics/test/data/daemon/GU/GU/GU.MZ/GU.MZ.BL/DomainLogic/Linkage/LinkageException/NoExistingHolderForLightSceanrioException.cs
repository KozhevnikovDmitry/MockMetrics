using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    public class NoExistingHolderForLightSceanrioException : BLLException
    {
        public NoExistingHolderForLightSceanrioException()
            : base("Не найден существующий лицензиат для тома с облегчённым сценарием")
        {

        }
    }
}