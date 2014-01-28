using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Появление работ, услуг, которые выполняются в составе конкретного вида деятельности (в порядке, предусмотренном ч.4 ст.22 ФЗ «О лицензировании отдельных видов деятельности»)
    /// </summary>
    public class NewWorksScenario : LicenseObjectsScenraio
    {
        public NewWorksScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.NewWorks; }
        }

        protected override string RenewalComment
        {
            get { return "Добавлены работы, услуги в составе вида деятельности"; }
        }
    }
}