using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.Core.BL
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

    public interface ILicenseRenewaller
    {
        License RenewalLicense(DossierFile dossierFile);
    }
}