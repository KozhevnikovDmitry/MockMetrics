using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// ������������� ������������ ���� � ����� ��������������
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