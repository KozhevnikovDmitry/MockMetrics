using System;
using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.DiActionContext.After
{
    public interface IServiceResultGranter
    {
        DossierFileServiceResult GrantServiseResult(DossierFile dossierFile, ICacheRepository cacheRepository);

        DossierFile SaveServiceResult(DossierFile dossierFile, ITaskStatusPolicy taskPolicy, IDomainDataMapper<DossierFile> fileMapper);

        License GrantNewLicense(DossierFile dossierFile, ILicenseProvider licenseProvider);

        License GrantRenewalLicense(DossierFile dossierFile, ILicenseRenewaller licenseRenewaller);

        License GrantStopLicense(DossierFile dossierFile, ILicenseProvider licenseProvider);
    }

    public class ServiceResultGranter : IServiceResultGranter
    {
        public DossierFileServiceResult GrantServiseResult(DossierFile dossierFile, ICacheRepository cacheRepository)
        {
            var result = DossierFileServiceResult.CreateInstance();
            result.Stamp = DateTime.Now;

            dossierFile.DossierFileServiceResult = result;

            SetServiceResult(dossierFile, cacheRepository);

            return result;
        }

        private void SetServiceResult(DossierFile dossierFile, ICacheRepository cacheRepository)
        {
            var fileResult = dossierFile.DossierFileServiceResult;

            var serviceResults =
                cacheRepository.GetCache<ServiceResult>().Where(t => t.ScenarioId == dossierFile.ScenarioId);

            try
            {
                if (dossierFile.ScenarioType == ScenarioType.Light)
                {
                    fileResult.ServiceResult = serviceResults.Single();
                    fileResult.ServiceResultId = fileResult.ServiceResult.Id;
                }
                else
                {
                    var isPositive = dossierFile.IsGrantingPositve;
                    fileResult.ServiceResult = serviceResults.Single(t => t.IsPositive == isPositive);
                    fileResult.ServiceResultId = fileResult.ServiceResult.Id;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new NotSingleResulstOnLightScenarioException(ex);
            }
        }

        public DossierFile SaveServiceResult(DossierFile dossierFile, ITaskStatusPolicy taskPolicy, IDomainDataMapper<DossierFile> fileMapper)
        {
            var tmp = dossierFile.Clone();

            var isPositive = tmp.IsGrantingPositve;

            if (isPositive)
            {
                taskPolicy.SetStatus(TaskStatusType.Done, string.Empty, tmp.Task);
            }

            return fileMapper.Save(tmp);
        }

        public License GrantNewLicense(DossierFile dossierFile, ILicenseProvider licenseProvider)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var newLicense = licenseProvider.GetNewLicense(dossierFile);
                dossierFile.License = newLicense;

                return newLicense;
            }

            throw new WrongStatusForGrantingException();
        }

        public License GrantRenewalLicense(DossierFile dossierFile, ILicenseRenewaller licenseRenewaller)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var renewalledLicense = licenseRenewaller.RenewalLicense(dossierFile);

                return renewalledLicense;
            }

            throw new WrongStatusForGrantingException();
        }

        public License GrantStopLicense(DossierFile dossierFile, ILicenseProvider licenseProvider)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var stoppedLicense = licenseProvider.GetStopLicense(dossierFile);

                return stoppedLicense;
            }

            throw new WrongStatusForGrantingException();
        }
    }
}
