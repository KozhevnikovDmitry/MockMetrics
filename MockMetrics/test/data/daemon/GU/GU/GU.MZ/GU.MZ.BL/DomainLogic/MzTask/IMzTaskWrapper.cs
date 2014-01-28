namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// ��������� MZ-������ ������
    /// </summary>
    public interface IMzTaskWrapper
    {
        /// <summary>
        /// ��� ������ �������������� �� ���� ������������ ������������
        /// </summary>
        LicenseServiceType LicenseServiceType { get; }

        /// <summary>
        /// ��� ������ �������������� �� ���� �������� ��� ���������
        /// </summary>
        LicenseActionType LicenseActionType { get; }
    }
}