using Common.Types.Exceptions;

namespace Common.UI.ViewModel.AvalonInteraction.AvalonInteractionException
{
    /// <summary>
    /// ����� ���������� ��� ������ "������ ��� ��������������� � AvalonDock �� �����."
    /// </summary>
    public class NullCallerInteractorException : GUException
    {
        public NullCallerInteractorException()
            :base("������ ��� ��������������� � AvalonDock �� �����.")
        {
            
        }
    }
}