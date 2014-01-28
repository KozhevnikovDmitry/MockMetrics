using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Изменение реквизитов документа, удостоверяющего личность индивидуального предпринимателя
    /// </summary>
    public class ChangeDocumentScenario : ChangeRequisitesScenario
    {
        public ChangeDocumentScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.ChangeDocument; }
        }
    }
}