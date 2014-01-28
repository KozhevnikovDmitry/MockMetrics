using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// Класс исключение на ошибку "Попытка сохранения непривязанного тома лицензионного дела".
    /// </summary>
    public class SaveUnlinkagedDossierFileException : GUException
    {
        public SaveUnlinkagedDossierFileException()
            : base("Попытка сохранения непривязанного тома лицензионного дела")
        {
            
        }
    }
}