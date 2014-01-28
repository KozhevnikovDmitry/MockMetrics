using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Осуществление лицензируемого вида деятельности по адресу, не указанному в лицензии
    /// </summary>
    public class AnotherAddressScenario : LicenseObjectsScenraio
    {
        public AnotherAddressScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.AnotherAddress; }
        }

        protected override string RenewalComment
        {
            get { return "Добавлен вид осуществляемой деятельности"; }
        }
    }
}