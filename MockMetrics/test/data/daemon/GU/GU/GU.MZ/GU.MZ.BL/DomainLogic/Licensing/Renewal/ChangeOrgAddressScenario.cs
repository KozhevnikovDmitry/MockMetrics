using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Изменение адреса места нахождения юридического лица
    /// </summary>
    public class ChangeOrgAddressScenario : ChangeRequisitesScenario
    {
        public ChangeOrgAddressScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.ChangeOrgAddress; }
        }
    }

    /// <summary>
    /// Изменение места жительства индивидуального предпринимателя
    /// </summary>
    public class ChangeAddressScenario : ChangeRequisitesScenario
    {
        public ChangeAddressScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.ChangeAddress; }
        }
    }
}