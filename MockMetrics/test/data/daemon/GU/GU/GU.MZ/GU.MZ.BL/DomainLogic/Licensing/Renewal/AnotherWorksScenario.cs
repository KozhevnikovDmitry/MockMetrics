using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// Выполнение работ, оказание услуг, не указанных в лицензии
    /// </summary>
    public class AnotherWorksScenario : LicenseObjectsScenraio
    {
        public AnotherWorksScenario(IParser guParser)
            : base(guParser)
        {
        }

        public override RenewalType RenewalType
        {
            get { return RenewalType.AnotherWorks; }
        }

        protected override string RenewalComment
        {
            get { return "Добавлено осуществление работ, услуг"; }
        }
        
    }
}