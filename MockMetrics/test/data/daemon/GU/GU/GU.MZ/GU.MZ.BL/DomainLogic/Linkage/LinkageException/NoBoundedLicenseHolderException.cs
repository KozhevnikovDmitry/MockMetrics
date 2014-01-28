using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ������ "������� ���������� ���� � ������ ��� �������� � ����������".
    /// </summary>
    public class NoBoundedLicenseHolderException : GUException
    {
        public NoBoundedLicenseHolderException()
            : base("������� ���������� ���� � ������ ��� �������� � ����������")
        {
            
        }
    }
}