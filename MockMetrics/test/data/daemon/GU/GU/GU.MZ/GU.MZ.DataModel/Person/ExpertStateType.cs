using System.ComponentModel;

namespace GU.MZ.DataModel.Person
{
    /// <summary>
    /// ������������ ����� ��������� ��������
    /// </summary>
    public enum ExpertStateType
    {
        [Description("���������� ����")]
        Individual = 1,

        [Description("����������� ����")]
        Juridical = 2
    }
}