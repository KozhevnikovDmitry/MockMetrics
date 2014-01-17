using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class CantAddProvidedDocumentWithEmptyNameException : BLLException
    {
        public CantAddProvidedDocumentWithEmptyNameException()
            :base("Невозможно добавить прилагаемый документ с пустым именем")
        {
            
        }
    }
}