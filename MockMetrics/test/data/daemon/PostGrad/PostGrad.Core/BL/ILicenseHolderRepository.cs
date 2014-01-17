using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.Core.BL
{
    public interface ILicenseHolderRepository
    {
        /// <summary>
        /// ��������� ������ ���������� � ������ �����������.   
        /// </summary>
        /// <param name="newLicenseHolder">����� ���������</param>
        /// <returns>��������� ���������� � ������</returns>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <exception cref="DuplicateLicenseHolderException">��������� � ������ ��� �\��� ���� ��� ������ � ������� �����������</exception>
        LicenseHolder AddNewLicenseHolder(LicenseHolder newLicenseHolder, IDomainDbManager dbManager);

        /// <summary>
        /// ���������� ���� ������� � ������� ���������� � ��������� ��� �\��� ����
        /// </summary>
        /// <param name="inn">���</param>
        /// <param name="ogrn">����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���� ������� � ������� ����������</returns>
        bool HolderExists(string inn, string ogrn, IDomainDbManager dbManager);

        /// <summary>
        /// ���������� ���������� �� ������� �� �������� ��� � ����.
        /// </summary>
        /// <param name="inn">���</param>
        /// <param name="ogrn">����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>��������� �� �������</returns>
        /// <exception cref="NoHolderFoundInRegistrException">���������� � ������ ��� ��� ���� ��� � ������� �����������</exception>
        /// <exception cref="MultipleHolderFoundInRegistrexception">������� ��������� ����������� � ��������� ��� � ����</exception>
        LicenseHolder GetLicenseHolder(string inn, string ogrn, IDomainDbManager dbManager);

        /// <summary>
        /// ��������� ��������� ��� ����������
        /// </summary>
        /// <param name="holderRequisites">��������� ����������</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���������� ���������</returns>
        HolderRequisites SaveLicenseHolderRequisites(HolderRequisites holderRequisites, IDomainDbManager dbManager);
    }
}