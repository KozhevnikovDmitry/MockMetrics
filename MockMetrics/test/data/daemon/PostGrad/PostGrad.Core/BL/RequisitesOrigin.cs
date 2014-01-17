using System.ComponentModel;

namespace PostGrad.Core.BL
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