using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ������ "������������� ���� ��� ���������� �� ���� ������������ �� ������� � ������� ������������ ���."
    /// </summary>
    public class NoDossierFoundInRegistrException : BLLException
    {
        public NoDossierFoundInRegistrException(int licenseHolderId, int licensedActivityId)
            : base(
                string.Format(
                    "������������� ���� ��� ���������� � {0} �� ���� ������������ {1} �� ������� � ������� ������������ ���.",
                    licenseHolderId,
                    licensedActivityId))
        {

        }
    }
}