using System.ComponentModel;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// ��� ������ �������������� �� ���� ������������ ������������
    /// </summary>
    public enum LicenseServiceType
    {
        [Description("����")]
        Drug,
        [Description("���")]
        Med,
        [Description("����")]
        Farm
    }
}