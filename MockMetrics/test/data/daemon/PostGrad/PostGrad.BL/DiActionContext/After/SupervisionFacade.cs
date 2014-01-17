using PostGrad.Core.BL;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.DiActionContext.After
{
    public class SupervisionFacade
    {
        private readonly IDiActionContext _diActionContext;
        public readonly IServiceResultGranter _serviceResultGranter;
        public DossierFile DossierFile { get; protected set; }

        public SupervisionFacade(DossierFile dossierFile, IDiActionContext diActionContext, IServiceResultGranter serviceResultGranter)
        {
            _diActionContext = diActionContext;
            _serviceResultGranter = serviceResultGranter;
            DossierFile = dossierFile;
        }

        public DossierFileServiceResult GrantServiseResult()
        {
            return _diActionContext.Get<ICacheRepository, DossierFileServiceResult>(d => _serviceResultGranter.GrantServiseResult(DossierFile, d));
        }

        public void SaveServiceResult()
        {
            DossierFile = _diActionContext.Get<ITaskStatusPolicy, IDomainDataMapper<DossierFile>, DossierFile>((t, m) => _serviceResultGranter.SaveServiceResult(DossierFile, t, m));
        }

        public License GrantNewLicense()
        {
            return _diActionContext.Get<ILicenseProvider, License>(l => _serviceResultGranter.GrantNewLicense(DossierFile, l));
        }

        public License GrantRenewalLicense()
        {
            return _diActionContext.Get<ILicenseRenewaller, License>(l => _serviceResultGranter.GrantRenewalLicense(DossierFile, l));
        }

        public License GrantStopLicense()
        {
            return _diActionContext.Get<ILicenseProvider, License>(l => _serviceResultGranter.GrantStopLicense(DossierFile, l));
        }
    }
}
