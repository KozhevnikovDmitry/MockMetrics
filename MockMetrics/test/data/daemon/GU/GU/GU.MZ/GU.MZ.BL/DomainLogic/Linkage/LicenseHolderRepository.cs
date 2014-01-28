using System.Linq;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Holder;

namespace GU.MZ.BL.DomainLogic.Linkage
{
    /// <summary>
    /// �����, �������������� ������ �����������.
    /// </summary>
    public class LicenseHolderRepository
    {
        /// <summary>
        /// ������ �����������
        /// </summary>
        private readonly IDomainDataMapper<LicenseHolder> _licenseHolderMapper;

        /// <summary>
        /// ������ ����������.
        /// </summary>
        private readonly IDomainDataMapper<HolderRequisites> _holderRequisitesMapper;

        protected LicenseHolderRepository()
        {
            
        }

        /// <summary>
        /// �����, �������������� ������ �����������.
        /// </summary>
        /// <param name="licenseHolderMapper">������ �����������</param>
        /// <param name="holderRequisitesMapper">������ ����������</param>
        public LicenseHolderRepository(IDomainDataMapper<LicenseHolder> licenseHolderMapper,
                                    IDomainDataMapper<HolderRequisites> holderRequisitesMapper)
        {
            _licenseHolderMapper = licenseHolderMapper;
            _holderRequisitesMapper = holderRequisitesMapper;
        }

        /// <summary>
        /// ��������� ������ ���������� � ������ �����������.   
        /// </summary>
        /// <param name="newLicenseHolder">����� ���������</param>
        /// <returns>��������� ���������� � ������</returns>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <exception cref="DuplicateLicenseHolderException">��������� � ������ ��� �\��� ���� ��� ������ � ������� �����������</exception>
        public virtual LicenseHolder AddNewLicenseHolder(LicenseHolder newLicenseHolder, IDomainDbManager dbManager)
        {
            if (HolderExists(newLicenseHolder.Inn, newLicenseHolder.Ogrn, dbManager))
            {
                throw new DuplicateLicenseHolderException(newLicenseHolder.Inn, newLicenseHolder.Ogrn);
            }

            return _licenseHolderMapper.Save(newLicenseHolder, dbManager);
        }

        /// <summary>
        /// ���������� ���� ������� � ������� ���������� � ��������� ��� �\��� ����
        /// </summary>
        /// <param name="inn">���</param>
        /// <param name="ogrn">����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���� ������� � ������� ����������</returns>
        public virtual bool HolderExists(string inn, string ogrn, IDomainDbManager dbManager)
        {
            return dbManager.GetDomainTable<LicenseHolder>().Any(
                   l => l.Inn == inn || l.Ogrn == ogrn);
        }

        /// <summary>
        /// ���������� ���������� �� ������� �� �������� ��� � ����.
        /// </summary>
        /// <param name="inn">���</param>
        /// <param name="ogrn">����</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>��������� �� �������</returns>
        /// <exception cref="NoHolderFoundInRegistrException">���������� � ������ ��� ��� ���� ��� � ������� �����������</exception>
        /// <exception cref="MultipleHolderFoundInRegistrexception">������� ��������� ����������� � ��������� ��� � ����</exception>
        public virtual LicenseHolder GetLicenseHolder(string inn, string ogrn, IDomainDbManager dbManager)
        {
            var holderIds = dbManager.GetDomainTable<LicenseHolder>()
                            .Where(l => l.Inn == inn || l.Ogrn == ogrn)
                            .Select(l => l.Id);

            if (holderIds.Count() == 1)
            {
                return _licenseHolderMapper.Retrieve(holderIds.Single(), dbManager);
            }

            if (!holderIds.Any())
            {
                throw new NoHolderFoundInRegistrException(inn, ogrn);
            }

            throw new MultipleHolderFoundInRegistrException(inn, ogrn, holderIds);
        }

        /// <summary>
        /// ��������� ��������� ��� ����������
        /// </summary>
        /// <param name="holderRequisites">��������� ����������</param>
        /// <param name="dbManager">�������� ���� ������</param>
        /// <returns>���������� ���������</returns>
        public virtual HolderRequisites SaveLicenseHolderRequisites(HolderRequisites holderRequisites, IDomainDbManager dbManager)
        {
            return _holderRequisitesMapper.Save(holderRequisites, dbManager);
        }
    }
}