using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ������ "���������� ��������� ��������� ���������� � ���� ����"
    /// </summary>
    public class CantLinkageRequisitesException : GUException
    {
        public CantLinkageRequisitesException()
            : base("���������� ��������� ��������� ���������� � ���� ����")
        {
            
        }
    }
}