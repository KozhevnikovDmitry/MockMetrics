using System.ComponentModel;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    /// <summary>
    /// ������������ ����� ���������� ���������� ���������� � ���� ��������
    /// </summary>
    public enum RequisitesOrigin
    {
        /// <summary>
        /// �� ������� �����������
        /// </summary>
        [Description("���������")]
        FromRegistr,

        /// <summary>
        /// �� ������ ���������
        /// </summary>
        [Description("���������")]
        FromTask
    }
}