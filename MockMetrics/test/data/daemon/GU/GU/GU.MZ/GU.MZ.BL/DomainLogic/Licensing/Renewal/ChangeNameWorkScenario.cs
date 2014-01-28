using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Изменение наименования вида деятельности (в порядке, предусмотренном ч.4 ст.22 ФЗ «О лицензировании отдельных видов деятельности»)
    /// </summary>
    public class ChangeNameWorkScenario : LicenseObjectsScenraio
    {
        public ChangeNameWorkScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.ChangeNameWork; }
        }

        protected override string RenewalComment
        {
            get { return "Изменённо наимнование деятельности"; }
        }
    }
}