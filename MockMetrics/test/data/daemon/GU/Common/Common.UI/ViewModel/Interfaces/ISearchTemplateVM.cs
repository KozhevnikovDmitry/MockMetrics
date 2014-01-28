using Common.DA.Interface;

namespace Common.UI.ViewModel.Interfaces
{
    /// <summary>
    /// ��������� ������� �������-������������� ��� ������� ������ ������.
    /// </summary>
    public interface ISearchTemplateVM
    {
        /// <summary>
        /// ���������� �������� ������, ������� ������ �������� ��� ������.
        /// </summary>
        IDomainObject SearchObject { get; set; }
    }
}