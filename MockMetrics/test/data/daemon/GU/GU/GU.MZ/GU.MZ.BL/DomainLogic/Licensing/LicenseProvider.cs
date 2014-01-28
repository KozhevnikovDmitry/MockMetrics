using System;
using System.Collections.Generic;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.DomainLogic.Licensing
{
    /// <summary>
    /// Провайдер лицензий - ищет данные лицензий в контенте заявки и сопоставляет с лицензиями в реестре
    /// </summary>
    public class LicenseProvider : ILicenseProvider
    {
        private readonly ILicenseObjectProvider _licenseObjectProvider;
        private readonly ILicenseRenewaller _licenseRenewaller;

        public LicenseProvider(ILicenseObjectProvider licenseObjectProvider, ILicenseRenewaller licenseRenewaller)
        {
            _licenseObjectProvider = licenseObjectProvider;
            _licenseRenewaller = licenseRenewaller;
        }

        /// <summary>
        /// Возвращает новую лицензию, которую нужно предоставить по итогу ведения тома
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Новая лицензия</returns>
        public License GetNewLicense(DossierFile dossierFile)
        {
            if (!dossierFile.IsNewLicense)
            {
                throw new WrongTaskServiceException(dossierFile);
            }

            if (!dossierFile.IsReady && !dossierFile.IsDone)
            {
                throw new FileNotReadyToGrantLicenseException(dossierFile);
            }

            var license = License.CreateInstance();

            if (dossierFile.LicenseDossierId.HasValue)
            {
                license.LicenseDossierId = dossierFile.LicenseDossierId.Value;
            }
            else
            {
                throw new FileNotLinkagedToDossierException(dossierFile);
            }

            license.CurrentStatus = LicenseStatusType.Project;
            var status = LicenseStatus.CreateInstance();
            status.Stamp = DateTime.Now;
            status.LicenseStatusType = LicenseStatusType.Project;
            license.LicenseStatusList.Add(status);

            license.LicensedActivity = dossierFile.LicensedActivity;
            license.LicensedActivityId = dossierFile.LicensedActivity.Id;
            license.LicensiarHeadName = "В.Н. Янин";
            license.LicensiarHeadPosition = "Министр здравоохранения Красноярского края";
            license.LicenseRequisitesList.Add(dossierFile.HolderRequisites.ToLicenseRequisites());

            var order = dossierFile.GetStandartOrderOfType(StandartOrderType.GrantLicense);
            if (order == null)
            {
                throw new NoGrantOrderException(dossierFile);
            }

            license.GrantOrderRegNumber = order.RegNumber;
            license.GrantOrderStamp = order.Stamp;

            license.LicenseObjectList = _licenseObjectProvider.GetNewLicenseObjects(dossierFile.Task);
            foreach (var licenseObject in license.LicenseObjectList)
            {
                licenseObject.GrantOrderRegNumber = order.RegNumber;
                licenseObject.GrantOrderStamp = order.Stamp;
            }

            return license;
        }

        /// <summary>
        /// Возвращает переоформленную лицензию тома
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Переоформленная лицензия</returns>
        public License GetRenewalLicense(DossierFile dossierFile)
        {
            return _licenseRenewaller.RenewalLicense(dossierFile);
        }

        public List<string> RenewalScenaries(DossierFile dossierFile)
        {
            return _licenseRenewaller.RenewalScenaries(dossierFile);
        }

        /// <summary>
        /// Возвращает заведённую лицензию, действие которому нужно прекратить по итогу ведения тома
        /// </summary>
        /// <param name="dossierFile">Том лицензионного дела</param>
        /// <returns>Прекращённая лицензия</returns>
        public License GetStopLicense(DossierFile dossierFile)
        {
            if (!dossierFile.IsStopLicense)
            {
                throw new WrongTaskServiceException(dossierFile);
            }

            if (!dossierFile.IsReady && !dossierFile.IsDone)
            {
                throw new FileNotReadyToGrantLicenseException(dossierFile);
            }

            if (dossierFile.License == null)
            {
                throw new FileNotLinkagedToLicenseException(dossierFile);
            }

            dossierFile.License.AddStatus(LicenseStatusType.Stop, DateTime.Now, string.Empty);
            return dossierFile.License;
        }



    }
}
