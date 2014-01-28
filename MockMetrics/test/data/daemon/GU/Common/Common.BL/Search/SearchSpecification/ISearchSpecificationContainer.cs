using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL.Search.SearchSpecification
{
    public interface ISearchPresetContainer
    {
        /// <summary>
        /// ���������� ������ ������ �������� �������� ���� T
        /// </summary>
        /// <typeparam name="T">�������� ���</typeparam>
        /// <exception cref="VMException">������ ��� �������� ���������� ������� ������ ��� ����</exception>
        /// <returns>������ ������</returns>
        SearchPreset ResolveSearchPreset<T>() where T : IPersistentObject;
    }
}