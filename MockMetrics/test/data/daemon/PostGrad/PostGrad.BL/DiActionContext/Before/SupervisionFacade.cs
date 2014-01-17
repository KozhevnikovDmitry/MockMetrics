using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;

namespace PostGrad.BL.DiActionContext.Before
{
    public class SupervisionFacade
    {
        private readonly IServiceResultGranter _serviceResultGranter;
        public DossierFile DossierFile { get; protected set; }

        public SupervisionFacade(DossierFile dossierFile,  IServiceResultGranter serviceResultGranter)
        {
            _serviceResultGranter = serviceResultGranter;
            DossierFile = dossierFile;
        }

        public DossierFileServiceResult GrantServiseResult()
        {
            return  _serviceResultGranter.GrantServiseResult(DossierFile);
        }

        public void SaveServiceResult()
        {
            DossierFile =_serviceResultGranter.SaveServiceResult(DossierFile);
        }

        public License GrantNewLicense()
        {
            return _serviceResultGranter.GrantNewLicense(DossierFile);
        }

        public License GrantRenewalLicense()
        {
            return _serviceResultGranter.GrantRenewalLicense(DossierFile);
        }

        public License GrantStopLicense()
        {
            return  _serviceResultGranter.GrantStopLicense(DossierFile);
        }
    }
}
