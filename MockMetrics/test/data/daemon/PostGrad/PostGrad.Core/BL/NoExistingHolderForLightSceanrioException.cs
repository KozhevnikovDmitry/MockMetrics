using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class NoExistingHolderForLightSceanrioException : BLLException
    {
        public NoExistingHolderForLightSceanrioException()
            : base("Не найден существующий лицензиат для тома с облегчённым сценарием")
        {

        }
    }
}