using System;

namespace PostGrad.Core.BL
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