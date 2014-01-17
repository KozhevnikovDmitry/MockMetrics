using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.BL
{
    public interface ILicenseDossierRepository
    {
        /// <summary>
        /// Добавляет новое лицензионное дело в реестр
        /// </summary>
        /// <param name="licenseDossier">Новое лицензионного дело</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сохранённое лицензионное дело</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        /// <exception cref="DuplicateDossierException">Попытка заведения дублирующего лицензионного дела</exception>
        LicenseDossier AddNewLicenseDossier(LicenseDossier licenseDossier, IDomainDbManager dbManager);

        /// <summary>
        /// Возвращает флаг наличия в реестре лицензионного дела по виду деятельности у лицензиата.
        /// </summary>
        /// <param name="licensedActivityId">Id Лицензируемой деятельсности</param>
        /// <param name="licenseHolderId">Id лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Флаг возможности добавления дела в реестр</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        bool DossierExists(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager);

        /// <summary>
        /// Возвращает лицензионное дело для лицензиата по виду деятельности.
        /// </summary>
        /// <param name="licensedActivityId">Id вида лицензируемой деятельности</param>
        /// <param name="licenseHolderId">Id лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Лицензионное дело</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        LicenseDossier GetLicenseDossier(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager);

        /// <summary>
        /// Возвращает регистрационный номер следующего тома в деле.
        /// </summary>
        /// <param name="licenseDossier">Лицензионное дело</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Регистрационный номер следующего тома в деле</returns>
        /// <exception cref="OldDossierWithoutFilesException">Заведённое лицензионное дело не включает ни одного тома</exception>
        int GetNextFileRegNumber(LicenseDossier licenseDossier, IDomainDbManager dbManager);

        int GetNextFileRegNumber(LicenseDossier licenseDossier);
    }
}