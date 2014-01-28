using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Изменение наименования юридического лица
    /// </summary>
    public class RenameScenario : ChangeRequisitesScenario
    {
        public RenameScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.Rename; }
        }
    }
}