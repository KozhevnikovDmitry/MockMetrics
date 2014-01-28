using GU.MZ.UI.ViewModel.SupervisionViewModel.Event;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel.Interface
{
    /// <summary>
    /// �������� ������� ��������� ������ ������ �� ������ vm'a �������������� ����
    /// </summary>
    public interface IRebuildRequestPublisher
    {
        /// <summary>
        /// ������� ������� �� ������������� vm'a �������������� ����
        /// </summary>
        event ViewModelRebuildRequested RebuildRequested;
    }
}