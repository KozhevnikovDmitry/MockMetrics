using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� �� ������ "������� ���������� �������������� ���� ������������� ����".
    /// </summary>
    public class SaveUnlinkagedDossierFileException : GUException
    {
        public SaveUnlinkagedDossierFileException()
            : base("������� ���������� �������������� ���� ������������� ����")
        {
            
        }
    }
}