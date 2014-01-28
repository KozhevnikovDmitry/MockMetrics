using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    public class CantAddProvidedDocumentWithEmptyNameException : BLLException
    {
        public CantAddProvidedDocumentWithEmptyNameException()
            :base("Невозможно добавить прилагаемый документ с пустым именем")
        {
            
        }
    }
}