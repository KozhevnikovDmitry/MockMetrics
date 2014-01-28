using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключений для обработки ошибок "Заведённое лицензионное дело не включает ни одного тома".
    /// </summary>
    public class OldDossierWithoutFilesException : BLLException
    {
        public OldDossierWithoutFilesException()
            : base("Заведённое лицензионное дело не включает ни одного тома")
        {
            
        }
    }
}