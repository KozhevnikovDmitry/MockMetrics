using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.Core.BL
{
    public interface ILicenseHolderRepository
    {
        /// <summary>
        /// Добавляет нового лицензиата в реестр лицензиатов.   
        /// </summary>
        /// <param name="newLicenseHolder">Новый лицензиат</param>
        /// <returns>Лицензиат добавленый в реестр</returns>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <exception cref="DuplicateLicenseHolderException">Лицензиат с такими ИНН и\или ОГРН уже заведён в реестре лицензиатов</exception>
        LicenseHolder AddNewLicenseHolder(LicenseHolder newLicenseHolder, IDomainDbManager dbManager);

        /// <summary>
        /// Возвращает флаг наличия в реестре лицензиата с заданными ИНН и\или ОГРН
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Флаг наличия в реестре лицензиата</returns>
        bool HolderExists(string inn, string ogrn, IDomainDbManager dbManager);

        /// <summary>
        /// Возвращает лицензиата из реестра по заданным ИНН и ОГРН.
        /// </summary>
        /// <param name="inn">ИНН</param>
        /// <param name="ogrn">ОГРН</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Лицензиат из реестра</returns>
        /// <exception cref="NoHolderFoundInRegistrException">Лицензиата с такими ИНН или ОГРН нет в реестре лицензиатов</exception>
        /// <exception cref="MultipleHolderFoundInRegistrexception">Найдено несколько лицензиатов с заданными ИНН и ОГРН</exception>
        LicenseHolder GetLicenseHolder(string inn, string ogrn, IDomainDbManager dbManager);

        /// <summary>
        /// Сохраняет реквизиты для лицензиата
        /// </summary>
        /// <param name="holderRequisites">реквизиты лицензиата</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сохранённые реквизиты</returns>
        HolderRequisites SaveLicenseHolderRequisites(HolderRequisites holderRequisites, IDomainDbManager dbManager);
    }
}