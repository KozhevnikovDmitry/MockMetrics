using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.BL
{
    public interface ILicenseDossierRepository
    {
        /// <summary>
        /// ��������� ����� ������������ ���� � ������
        /// </summary>
        /// <param name="licenseDossier">����� ������������� ����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���������� ������������ ����</returns>
        /// <exception cref="NoBoundedLicenseHolderException">������� ���������� ���� � ������ ��� �������� � ����������</exception>
        /// <exception cref="DuplicateDossierException">������� ��������� ������������ ������������� ����</exception>
        LicenseDossier AddNewLicenseDossier(LicenseDossier licenseDossier, IDomainDbManager dbManager);

        /// <summary>
        /// ���������� ���� ������� � ������� ������������� ���� �� ���� ������������ � ����������.
        /// </summary>
        /// <param name="licensedActivityId">Id ������������� �������������</param>
        /// <param name="licenseHolderId">Id ����������</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���� ����������� ���������� ���� � ������</returns>
        /// <exception cref="NoBoundedLicenseHolderException">������� ���������� ���� � ������ ��� �������� � ����������</exception>
        bool DossierExists(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager);

        /// <summary>
        /// ���������� ������������ ���� ��� ���������� �� ���� ������������.
        /// </summary>
        /// <param name="licensedActivityId">Id ���� ������������� ������������</param>
        /// <param name="licenseHolderId">Id ����������</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>������������ ����</returns>
        /// <exception cref="NoBoundedLicenseHolderException">������� ���������� ���� � ������ ��� �������� � ����������</exception>
        LicenseDossier GetLicenseDossier(int licensedActivityId, int licenseHolderId, IDomainDbManager dbManager);

        /// <summary>
        /// ���������� ��������������� ����� ���������� ���� � ����.
        /// </summary>
        /// <param name="licenseDossier">������������ ����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>��������������� ����� ���������� ���� � ����</returns>
        /// <exception cref="OldDossierWithoutFilesException">��������� ������������ ���� �� �������� �� ������ ����</exception>
        int GetNextFileRegNumber(LicenseDossier licenseDossier, IDomainDbManager dbManager);

        int GetNextFileRegNumber(LicenseDossier licenseDossier);
    }
}