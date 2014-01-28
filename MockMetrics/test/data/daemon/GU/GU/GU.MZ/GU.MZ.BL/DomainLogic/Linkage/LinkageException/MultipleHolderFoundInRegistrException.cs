using System.Collections.Generic;
using System.Linq;
using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.Linkage.LinkageException
{
    /// <summary>
    /// ����� ���������� ��� ������ "������� ��������� ����������� � ��������� ��� � ����."
    /// </summary>
    public class MultipleHolderFoundInRegistrException : BLLException
    {
        /// <summary>
        /// ������ ��������� �����������
        /// </summary>
        public List<int> HolderIds { get; private set; }

        /// <summary>
        /// ����� ���������� ��� ������ "������� ��������� ����������� � ��������� ��� � ����."
        /// </summary>
        /// <param name="inn">���</param>
        /// <param name="ogrn">����</param>
        /// <param name="holderIds">������ ��������� �����������</param>
        public MultipleHolderFoundInRegistrException(string inn, string ogrn, IEnumerable<int> holderIds)
            : base(string.Format("������� ��������� ����������� � ��������� ��� = {0} � ���� = {1}.", inn, ogrn))
        {
            HolderIds = holderIds.ToList();
        }
    }
}