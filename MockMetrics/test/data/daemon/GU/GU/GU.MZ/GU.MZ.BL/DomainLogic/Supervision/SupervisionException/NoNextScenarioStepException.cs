using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Supervision.SupervisionException
{
    /// <summary>
    /// ����� ���������� ��� ��������� ������ "��� ���������� ����� ������� ����. ������� ���� ������� �������� ���������."
    /// </summary>
    public class NoNextScenarioStepException : BLLException
    {
        public NoNextScenarioStepException()
            : base("��� ���������� ����� ������� ����. ������� ���� ������� �������� ���������.")
        {
            
        }
    }
}