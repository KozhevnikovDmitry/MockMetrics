using System;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// ����� ���������� ��� ������ "�������� ������� ���� �� �����������"
    /// </summary>
    public class WrongScenarioSteplistException : Exception
    {
        public WrongScenarioSteplistException()
            :base("�������� ������� ���� �� �����������.")
        {
            
        }
    }
}