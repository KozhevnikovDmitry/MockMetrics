using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    /// <summary>
    /// Класс, представляющий реестр лицензиатов.
    /// </summary>
    public class LicenseHolderRepository
    {
        /// <summary>
        /// Маппер лицензиатов
        /// </summary>
        private readonly IDomainDataMapper<LicenseHolder> _licenseHolderMapper;

        /// <summary>
        /// Маппер реквизитов.
        /// </summary>
        private readonly IDomainDataMapper<HolderRequisites> _holderRequisitesMapper;

        protected LicenseHolderRepository()
        {
            
        }

        /// <summary>
        /// Класс, представляющий реестр лицензиатов.
        /// </summary>
        /// <param name="licenseHolderMapper">Маппер лицензиатов</param>
        /// <param name="holderRequisitesMapper">Маппер реквизитов</param>
        public LicenseHolderRepository(IDomainDataMapper<LicenseHolder> licenseHolderMapper,
                                    IDomainDataMapper<HolderRequisites> holderRequisitesMapper)
        {
            _licenseHolderMapper = licenseHolderMapper;
            _holderRequisitesMapper = holderRequisitesMapper;
        }

        /// <summary>
        /// Добавляет нового лицензиата в реестр лицензиатов.   
        /// </summary>
        /// <param name="newLicenseHolder">Новый лицензиат</param>
        /// <returns>Лицензиат добавленый в реестр</returns>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <exception cref="DuplicateLicenseHolderException">Лицензиат с такими ИНН и\или ОГРН уже заведён в реестре лицензиатов</exception>
        public virtual LicenseHolder AddNewLicenseHolder(LicenseHolder newLicenseHolder, IDomainDbManager dbManager)
        {
            if (HolderExists(newLicenseHolder.Inn, newLicenseHolder.Ogrn, dbManager))
            {
                throw new DuplicateLicenseHolderException(newLicenseHolder.Inn, newLicenseHolder.Ogrn);
            }

            return _licenseHolderMapper.Save(newLicenseHolder, dbManager);
        }

        /// <summary>
        /// Возвращает флаг наличия в реестре лицензиата с заданными ИНН и\или ОГРН
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Флаг наличия в реестре лицензиата</returns>
        public virtual bool HolderExists(string inn, string ogrn, IDomainDbManager dbManager)
        {
            return dbManager.GetDomainTable<LicenseHolder>().Any(
                   l => l.Inn == inn || l.Ogrn == ogrn);
        }

        /// <summary>
        /// Возвращает лицензиата из реестра по заданным ИНН и ОГРН.
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Лицензиат из реестра</returns>
        /// <exception cref="NoHolderFoundInRegistrException">Лицензиата с такими ИНН или ОГРН нет в реестре лицензиатов</exception>
        /// <exception cref="MultipleHolderFoundInRegistrexception">Найдено несколько лицензиатов с заданными ИНН и ОГРН</exception>
        public virtual LicenseHolder GetLicenseHolder(string inn, string ogrn, IDomainDbManager dbManager)
        {
            var holderIds = dbManager.GetDomainTable<LicenseHolder>()
                            .Where(l => l.Inn == inn || l.Ogrn == ogrn)
                            .Select(l => l.Id);

            if (holderIds.Count() == 1)
            {
                return _licenseHolderMapper.Retrieve(holderIds.Single(), dbManager);
            }

            if (!holderIds.Any())
            {
                throw new NoHolderFoundInRegistrException(inn, ogrn);
            }

            throw new MultipleHolderFoundInRegistrException(inn, ogrn, holderIds);
        }

        /// <summary>
        /// Сохраняет реквизиты для лицензиата
        /// </summary>
        /// <param name="holderRequisites">реквизиты лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сохранённые реквизиты</returns>
        public virtual HolderRequisites SaveLicenseHolderRequisites(HolderRequisites holderRequisites, IDomainDbManager dbManager)
        {
            return _holderRequisitesMapper.Save(holderRequisites, dbManager);
        }
    }
}