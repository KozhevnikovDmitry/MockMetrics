using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ��������� ������ "������������ ��� �������� ��� ��������� ��������".
    /// </summary>
    public class WrongSceanrioTypeForLinkagerException : BLLException
    {
        public WrongSceanrioTypeForLinkagerException()
            : base("������������ ��� �������� ��� ��������� ��������")
        {
            
        }
    }
}