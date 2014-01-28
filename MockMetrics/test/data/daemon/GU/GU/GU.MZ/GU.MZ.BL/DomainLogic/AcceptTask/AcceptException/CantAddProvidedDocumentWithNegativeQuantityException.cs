using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    public class CantAddProvidedDocumentWithNegativeQuantityException : BLLException
    {
        public CantAddProvidedDocumentWithNegativeQuantityException()
            : base("Невозможно добавить прилагаемый документ отрицательным или нулевым количеством")
        {

        }
    }
}