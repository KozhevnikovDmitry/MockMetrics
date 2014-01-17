using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class CantAddProvidedDocumentWithNegativeQuantityException : BLLException
    {
        public CantAddProvidedDocumentWithNegativeQuantityException()
            : base("Невозможно добавить прилагаемый документ отрицательным или нулевым количеством")
        {

        }
    }
}