using System;

using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// ����� ���������� ��� ������ "�� ������� ����� ������������� ������������, � ������� ���� ������������� ������".
    /// </summary>
    public class CantFindTaskLicensedActivityException : GUException
    {
        public CantFindTaskLicensedActivityException(Exception ex)
            :base("�� ������� ����� ������������� ������������, � ������� ���� ������������� ������")
        {
            
        }
    }
}