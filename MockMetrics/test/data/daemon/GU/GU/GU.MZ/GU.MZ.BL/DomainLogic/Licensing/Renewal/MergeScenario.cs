using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.LicenseChange;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Реорганизация юридических лиц в форме слияния
    /// </summary>
    public class MergeScenario : BaseRenewalScenario
    {
        public MergeScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.Merge; }
        }

        public override ChangeLicenseSession Renewal(DossierFile dossierFile, ContentNode renewalNode)
        {
            throw new System.NotImplementedException();
        }
        
    }
}