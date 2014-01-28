using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ��������� ������ "��������� ������������ ���� �� �������� �� ������ ����".
    /// </summary>
    public class OldDossierWithoutFilesException : BLLException
    {
        public OldDossierWithoutFilesException()
            : base("��������� ������������ ���� �� �������� �� ������ ����")
        {
            
        }
    }
}