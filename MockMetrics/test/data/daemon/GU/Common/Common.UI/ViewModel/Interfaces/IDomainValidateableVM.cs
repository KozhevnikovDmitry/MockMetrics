using Common.DA.Interface;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// ��������� ��� ViewModel'�� � ������������ ����������� ��������� ����� �������� ��������
    /// </summary>
    /// <typeparam name="T">�������� ���</typeparam>
    public interface IDomainValidateableVM<T> : IValidateableVM
        where T : IDomainObject
    {
        T Entity { get; }
    }
}