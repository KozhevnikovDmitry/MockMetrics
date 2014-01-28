using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    /// <summary>
    /// Класс, представлюящий реестр томов лицензионного дела.
    /// </summary>
    public class DossierFileRepository : TransactionWrapper
    {
        /// <summary>
        /// Реестр лицензиатов
        /// </summary>
        private readonly LicenseHolderRepository _licenseHolderRepository;

        /// <summary>
        /// Реестр дел
        /// </summary>
        private readonly LicenseDossierRepository _licenseDossierRepository;

        /// <summary>
        /// Маппер томов
        /// </summary>
        private readonly IDomainDataMapper<DossierFile> _dossierFileMapper;

        /// <summary>
        /// Маппер описи
        /// </summary>
        private readonly IDomainDataMapper<DocumentInventory> _inventoryMapper;

        private readonly Func<IDomainDbManager> _getDb;

        protected DossierFileRepository()
        {
            
        }

        /// <summary>
        /// Класс, представлюящий реестр томов лицензионного дела.
        /// </summary>
        /// <param name="licenseHolderRepository">Реестр лицензиатов</param>
        /// <param name="licenseDossierRepository">Реестр дел</param>
        /// <param name="dossierFileMapper">Маппер томов</param>
        /// <param name="inventoryMapper">Маппер описи</param>
        /// <param name="domainContext">Доменный контекст</param>
        public DossierFileRepository(LicenseHolderRepository licenseHolderRepository, 
                                  LicenseDossierRepository licenseDossierRepository,
                                  IDomainDataMapper<DossierFile> dossierFileMapper,
                                  IDomainDataMapper<DocumentInventory> inventoryMapper,
                                  Func<IDomainDbManager> getDb)
        {
            _licenseHolderRepository = licenseHolderRepository;
            _licenseDossierRepository = licenseDossierRepository;
            _dossierFileMapper = dossierFileMapper;
            _inventoryMapper = inventoryMapper;
            _getDb = getDb;
        }

        /// <summary>
        /// Сохраняет прототип тома
        /// </summary>
        /// <param name="dossierFile">Протоип тома</param>
        /// <returns>Сохранённый прототи тома</returns>
        /// <exception cref="BLLException">Ошибка при сохранении данных тома</exception>
        public DossierFile AcceptDossierFile(DossierFile dossierFile)
        {
            DossierFile result = null;
            using (var dbManager = _getDb())
            {
                Transaction(dbManager, db =>
                {
                    var tmp = dossierFile.Clone();
                    result = _dossierFileMapper.Save(tmp, db);
                    tmp.DocumentInventory.Id = result.Id;
                    tmp.DocumentInventory = _inventoryMapper.Save(tmp.DocumentInventory, db);
                });
            }
            return result;
        }

        /// <summary>
        /// Сохраняет привязанный том лицензионного дела.
        /// </summary>
        /// <param name="dossierFile">Привязанный том лицензионного дела</param>
        /// <returns>Сохранённый тома лицензионного дела</returns>
        /// <exception cref="SaveUnlinkagedDossierFileException">Попытка сохранения непривязанного тома лицензионного дела</exception>
        /// <exception cref="Exception">Ошибка сохранения тома лицензонного дела</exception>
        /// <exception cref="BLLException">Ошибка при маппинге тома или его ассоциаций</exception>
        public DossierFile SaveLinkagedDossierFile(DossierFile dossierFile)
        {
            var tmp = dossierFile.Clone();

            if (!tmp.IsLinkaged)
            {
                throw new SaveUnlinkagedDossierFileException();
            }
            using (var dbManager = _getDb())
            {
                Transaction(dbManager, db => { tmp = SaveDossierFile(tmp, db); });
            }
            return tmp;
        }

        private DossierFile SaveDossierFile(DossierFile dossierFile, IDomainDbManager dbManager)
        {
            if (dossierFile.IsDossierNew)
            {
                if (dossierFile.IsHolderNew)
                {
                    SaveNewHolder(dossierFile, dbManager);
                }

                SaveNewDossier(dossierFile, dbManager);
            }

            return _dossierFileMapper.Save(dossierFile, dbManager);
        }

        private void SaveNewHolder(DossierFile dossierFile, IDomainDbManager dbManager)
        {
            // сохраняем лицензиата
            dossierFile.LicenseDossier.LicenseHolder =
                _licenseHolderRepository.AddNewLicenseHolder(dossierFile.LicenseDossier.LicenseHolder, dbManager);

            // проставляем новый Id лицензиата
            dossierFile.LicenseDossier.LicenseHolderId = dossierFile.LicenseDossier.LicenseHolder.Id;
        }

        private void SaveNewDossier(DossierFile dossierFile, IDomainDbManager dbManager)
        {
            // сохраняем новое дело
            dossierFile.LicenseDossier =
                _licenseDossierRepository.AddNewLicenseDossier(dossierFile.LicenseDossier, dbManager);

            // проставляем новый Id дела 
            dossierFile.LicenseDossierId = dossierFile.LicenseDossier.Id;
        }
    }
}
