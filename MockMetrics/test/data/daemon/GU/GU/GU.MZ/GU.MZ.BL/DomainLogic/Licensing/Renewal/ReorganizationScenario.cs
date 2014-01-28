using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Реорганизация юридического лица в форме преобразования
    /// </summary>
    public class ReorganizationScenario : ChangeRequisitesScenario
    {
        public ReorganizationScenario(IParser guParser) : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.Reorganization; }
        }
    }
}