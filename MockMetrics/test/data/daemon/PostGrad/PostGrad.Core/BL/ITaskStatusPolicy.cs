using PostGrad.Core.DomainModel;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Интерфейс класса политики управления статусами заявки
    /// </summary>
    public interface ITaskStatusPolicy
    {
        /// <summary>
        /// Изменение статуса заявки
        /// </summary>
        /// <param name="taskStatusType">Тип статуса</param>
        /// <param name="comment">Комментарий к операции изменения статуса</param>
        /// <param name="task">Объект заявка</param>
        /// <returns>Заявка с добавленным статусом</returns>
        Task SetStatus(TaskStatusType taskStatusType, string comment, Task task);

        /// <summary>
        /// Возвращает флаг возможности изменения статуса заявки с учетом всех проверок
        /// <seealso cref="ValidateSetStatus"/>
        /// <seealso cref="IsValidStatusTransition"/>
        /// </summary>
        /// <param name="taskStatusType">Тип статуса</param>
        /// <param name="task">Объект заявка</param>
        /// <returns>Флаг возможности добавления статусов</returns>
        bool CanSetStatus(TaskStatusType taskStatusType, Task task);
        
        /// <summary>
        /// Проверка допустимости перехода из одного статуса в другой
        /// </summary>
        /// <param name="oldStatusType">Текущий статус</param>
        /// <param name="newStatusType">Новый статус</param>
        /// <returns>Флаг возможности перехода</returns>
        bool IsValidStatusTransition(TaskStatusType oldStatusType, TaskStatusType newStatusType);
    }
}