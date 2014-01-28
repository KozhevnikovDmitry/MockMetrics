using Common.Types.Exceptions;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Licensing
{
    public class WrongTaskServiceException : BLLException
    {
        public WrongTaskServiceException(DossierFile dossierFile)
            :base(string.Format("Неправильный тип услуги тома Id=[{0}] для запрошенного действия с лицензией", dossierFile.Id))
        {
            
        }
    }

    public class FileNotReadyToGrantLicenseException : BLLException
    {
        public FileNotReadyToGrantLicenseException(DossierFile dossierFile)
            : base(string.Format("Статус ведения тома Id=[{0}] не повзвляет сформировать лицензию", dossierFile.Id))
        {

        }
    }

    public class NoGrantOrderException : BLLException
    {
        public NoGrantOrderException(DossierFile dossierFile)
            : base(string.Format("Приказ о предоставлении лицензии в томе Id=[{0}] не найден", dossierFile.Id))
        {

        }
    }

    public class FileNotLinkagedToDossierException : BLLException
    {
        public FileNotLinkagedToDossierException(DossierFile dossierFile)
            : base(string.Format("Том Id=[{0}] не привязан к лицензионному делу. Лицензия не может быть прекращена", dossierFile.Id))
        {

        }
    }

    public class FileNotLinkagedToLicenseException : BLLException
    {
        public FileNotLinkagedToLicenseException(DossierFile dossierFile)
            : base(string.Format("Том Id=[{0}] не привязан к лицензии. Лицензия не может быть прекращена", dossierFile.Id))
        {

        }
    }
}