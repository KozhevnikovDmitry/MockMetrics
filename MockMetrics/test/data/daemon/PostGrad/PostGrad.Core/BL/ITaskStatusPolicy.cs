using PostGrad.Core.DomainModel;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// ��������� ������ �������� ���������� ��������� ������
    /// </summary>
    public interface ITaskStatusPolicy
    {
        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        /// <param name="taskStatusType">��� �������</param>
        /// <param name="comment">����������� � �������� ��������� �������</param>
        /// <param name="task">������ ������</param>
        /// <returns>������ � ����������� ��������</returns>
        Task SetStatus(TaskStatusType taskStatusType, string comment, Task task);

        /// <summary>
        /// ���������� ���� ����������� ��������� ������� ������ � ������ ���� ��������
        /// <seealso cref="ValidateSetStatus"/>
        /// <seealso cref="IsValidStatusTransition"/>
        /// </summary>
        /// <param name="taskStatusType">��� �������</param>
        /// <param name="task">������ ������</param>
        /// <returns>���� ����������� ���������� ��������</returns>
        bool CanSetStatus(TaskStatusType taskStatusType, Task task);
        
        /// <summary>
        /// �������� ������������ �������� �� ������ ������� � ������
        /// </summary>
        /// <param name="oldStatusType">������� ������</param>
        /// <param name="newStatusType">����� ������</param>
        /// <returns>���� ����������� ��������</returns>
        bool IsValidStatusTransition(TaskStatusType oldStatusType, TaskStatusType newStatusType);
    }
}