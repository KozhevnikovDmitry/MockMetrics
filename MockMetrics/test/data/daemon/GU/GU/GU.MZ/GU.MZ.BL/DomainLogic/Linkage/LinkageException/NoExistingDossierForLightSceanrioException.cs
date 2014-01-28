using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    public class NoExistingDossierForLightSceanrioException : BLLException
    {
        public NoExistingDossierForLightSceanrioException()
            : base("Не найден существующее дело для тома с облегчённым сценарием")
        {

        }
    }
}