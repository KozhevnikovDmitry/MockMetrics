using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    public class WrongStatusForGrantingException : BLLException
    {
        public WrongStatusForGrantingException()
            : base("������� ������ � ����������� ���� �� ������� � ������������ �������")
        {
            
        }
    }
}