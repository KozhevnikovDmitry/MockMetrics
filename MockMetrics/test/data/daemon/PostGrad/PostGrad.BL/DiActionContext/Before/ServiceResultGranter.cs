using System;
using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.DiActionContext.Before
{
    public interface IServiceResultGranter
    {
        DossierFileServiceResult GrantServiseResult(DossierFile dossierFile);

        DossierFile SaveServiceResult(DossierFile dossierFile);

        License GrantNewLicense(DossierFile dossierFile);

        License GrantRenewalLicense(DossierFile dossierFile);

        License GrantStopLicense(DossierFile dossierFile);
    }

    public class ServiceResultGranter : IServiceResultGranter
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly ITaskStatusPolicy _taskStatusPolicy;
        private readonly IDomainDataMapper<DossierFile> _fileMapper;
        private readonly ILicenseProvider _licenseProvider;
        private readonly ILicenseRenewaller _licenseRenewaller;

        public ServiceResultGranter(ICacheRepository cacheRepository,
                                    ITaskStatusPolicy taskStatusPolicy,
                                    IDomainDataMapper<DossierFile> fileMapper,
                                    ILicenseProvider licenseProvider,
                                    ILicenseRenewaller licenseRenewaller)
        {
            _cacheRepository = cacheRepository;
            _taskStatusPolicy = taskStatusPolicy;
            _fileMapper = fileMapper;
            _licenseProvider = licenseProvider;
            _licenseRenewaller = licenseRenewaller;
        }

        public DossierFileServiceResult GrantServiseResult(DossierFile dossierFile)
        {
            var result = DossierFileServiceResult.CreateInstance();
            result.Stamp = DateTime.Now;

            dossierFile.DossierFileServiceResult = result;

            SetServiceResult(dossierFile);

            return result;
        }

        private void SetServiceResult(DossierFile dossierFile)
        {
            var fileResult = dossierFile.DossierFileServiceResult;

            var serviceResults =
                _cacheRepository.GetCache<ServiceResult>().Where(t => t.ScenarioId == dossierFile.ScenarioId);

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

        public DossierFile SaveServiceResult(DossierFile dossierFile)
        {
            var tmp = dossierFile.Clone();

            if (tmp.IsGrantingPositve)
            {
                _taskStatusPolicy.SetStatus(TaskStatusType.Done, string.Empty, tmp.Task);
            }

            return _fileMapper.Save(tmp);
        }

        

        public License GrantNewLicense(DossierFile dossierFile)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var newLicense = _licenseProvider.GetNewLicense(dossierFile);
                dossierFile.License = newLicense;

                return newLicense;
            }

            throw new WrongStatusForGrantingException();
        }

        public License GrantRenewalLicense(DossierFile dossierFile)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var renewalledLicense = _licenseRenewaller.RenewalLicense(dossierFile);

                return renewalledLicense;
            }

            throw new WrongStatusForGrantingException();
        }

        public License GrantStopLicense(DossierFile dossierFile)
        {
            if (dossierFile.IsGrantingPositve)
            {
                var stoppedLicense = _licenseProvider.GetStopLicense(dossierFile);

                return stoppedLicense;
            }

            throw new WrongStatusForGrantingException();
        }
    }
}
