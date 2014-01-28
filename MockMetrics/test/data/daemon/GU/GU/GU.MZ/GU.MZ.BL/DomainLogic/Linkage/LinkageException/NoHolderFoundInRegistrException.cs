using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ������ "���������� � ������ ��� � ���� ��� � ������� �����������"
    /// </summary>
    public class NoHolderFoundInRegistrException : GUException
    {
        public NoHolderFoundInRegistrException(string inn, string ogrn)
            : base(string.Format("���������� � ���={0} � ����={1} ��� � ������� �����������", inn, ogrn))
        {
            
        }
    }
}