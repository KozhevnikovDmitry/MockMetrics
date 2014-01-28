using Common.Types.Exceptions;

using GU.DataModel;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// Класс исключение для ошибки "Невозможно установить статус для заявки"
    /// </summary>
    public class CantSetStatusException : GUException
    {
        /// <summary>
        /// Возвращает статус, который не удалось установить для заявки.
        /// </summary>
        public TaskStatusType Status { get; private set; }

        /// <summary>
        /// Класс исключение для ошибки "Невозможно установить статус для заявки".
        /// </summary>
        /// <param name="status">Возвращает статус, который не удалось установить для заявки</param>
        public CantSetStatusException(TaskStatusType status)
            : base("Невозможно установить статус для заявки")
        {
            Status = status;
        }
    }
}