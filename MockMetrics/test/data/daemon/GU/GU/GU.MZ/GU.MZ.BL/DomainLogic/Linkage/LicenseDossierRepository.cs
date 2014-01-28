using System;
using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    /// <summary>
    /// Класс, представляющий реестр лицензионных дел.
    /// </summary>
    public class LicenseDossierRepository
    {
        /// <summary>
        /// Маппер лицензионных дел.
        /// </summary>
        private readonly IDomainDataMapper<LicenseDossier> _licenseDossierMapper;

        private readonly Func<IDomainDbManager> _getDb;

        protected LicenseDossierRepository()
        {
            
        }

        /// <summary>
        /// Класс, представляющий реестр лицензионных дел. 
        /// </summary>
        /// <param name="licenseDossierMapper">Маппер лицензионных дел</param>
        public LicenseDossierRepository(IDomainDataMapper<LicenseDossier> licenseDossierMapper, Func<IDomainDbManager> getDb)
        {
            _licenseDossierMapper = licenseDossierMapper;
            _getDb = getDb;
        }

        /// <summary>
        /// Добавляет новое лицензионное дело в реестр
        /// </summary>
        /// <param name="licenseDossier">Новое лицензионного дело</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сохранённое лицензионное дело</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        /// <exception cref="DuplicateDossierException">Попытка заведения дублирующего лицензионного дела</exception>
        public virtual LicenseDossier AddNewLicenseDossier(LicenseDossier licenseDossier, IDomainDbManager dbManager)
        {
            if (licenseDossier.LicenseHolderId == 0)
            {
                throw new NoBoundedLicenseHolderException();
            }

            if (DossierExists(licenseDossier.LicensedActivityId, licenseDossier.LicenseHolderId, dbManager))
            {
                throw new DuplicateDossierException();
            }

            return _licenseDossierMapper.Save(licenseDossier, dbManager);
        }

        /// <summary>
        /// Возвращает флаг наличия в реестре лицензионного дела по виду деятельности у лицензиата.
        /// </summary>
        /// <param name="licensedActivityId">Id Лицензируемой деятельсности</param>
        /// <param name="licenseHolderId">Id лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Флаг возможности добавления дела в реестр</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        public virtual bool DossierExists(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager)
        {
            var holder = dbManager.GetDomainTable<LicenseHolder>()
                                  .SingleOrDefault(lh => lh.Id == licenseHolderId);

            if (holder == null)
            {
                return false;
            }

            return dbManager.GetDomainTable<LicenseDossier>()
                             .Any(ld => ld.LicenseHolderId == licenseHolderId &&
                                        ld.LicensedActivityId == licensedActivityId &&
                                        ld.IsActive);
        }

        /// <summary>
        /// Возвращает лицензионное дело для лицензиата по виду деятельности.
        /// </summary>
        /// <param name="licensedActivityId">Id вида лицензируемой деятельности</param>
        /// <param name="licenseHolderId">Id лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Лицензионное дело</returns>
        /// <exception cref="NoBoundedLicenseHolderException">Попытка добавления дела в реестр без привязки к лицензиату</exception>
        public virtual LicenseDossier GetLicenseDossier(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager)
        {
            int dossierId = dbManager.GetDomainTable<LicenseDossier>()
                                     .Where(ld => ld.LicenseHolderId == licenseHolderId &&
                                            ld.LicensedActivityId == licensedActivityId)
                                     .Select(ld => ld.Id)
                                     .SingleOrDefault();
            if (dossierId != 0)
            {
                var dossier = _licenseDossierMapper.Retrieve(dossierId, dbManager);
                return dossier;
            }

            throw new NoBoundedLicenseHolderException();
        }

        /// <summary>
        /// Возвращает регистрационный номер следующего тома в деле.
        /// </summary>
        /// <param name="licenseDossier">Лицензионное дело</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Регистрационный номер следующего тома в деле</returns>
        /// <exception cref="OldDossierWithoutFilesException">Заведённое лицензионное дело не включает ни одного тома</exception>
        public virtual int GetNextFileRegNumber(LicenseDossier licenseDossier, IDomainDbManager dbManager)
        {
            if (licenseDossier.Id == 0)
            {
                return 1;
            }

            try
            {
                int lastRegNumber = dbManager.GetDomainTable<DossierFile>().Where(t => t.LicenseDossierId == licenseDossier.Id).Max(t => t.RegNumber);
                return lastRegNumber + 1;
            }
            catch (InvalidOperationException)
            {
                return 1;
            }
        }

        public virtual int GetNextFileRegNumber(LicenseDossier licenseDossier)
        {
            using (var db = _getDb())
            {
                return GetNextFileRegNumber(licenseDossier, db);
            }
        }
    }
}
