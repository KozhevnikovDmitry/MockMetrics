using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Licensing
{
    public interface ILicenseProvider
    {
        /// <summary>
        /// Возвращает новую лицензию, которую нужно предоставить по итогу ведения тома
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Новая лицензия</returns>
        License GetNewLicense(DossierFile dossierFile);

        /// <summary>
        /// Возвращает заведённую лицензию, действие которому нужно прекратить по итогу ведения тома
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Прекращённая лицензия</returns>
        License GetStopLicense(DossierFile dossierFile);
    }
}