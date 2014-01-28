using GU.MZ.BL.DomainLogic.GuParse;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    /// <summary>
    /// ��������� �����, �����, ������� ����������� � ������� ����������� ���� ������������ (� �������, ��������������� �.4 ��.22 �� �� �������������� ��������� ����� ������������)
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
            get { return "��������� ������, ������ � ������� ���� ������������"; }
        }
    }
}