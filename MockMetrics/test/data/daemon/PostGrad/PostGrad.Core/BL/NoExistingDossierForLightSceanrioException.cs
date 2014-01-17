using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class NoExistingDossierForLightSceanrioException : BLLException
    {
        public NoExistingDossierForLightSceanrioException()
            : base("Не найден существующее дело для тома с облегчённым сценарием")
        {

        }
    }
}