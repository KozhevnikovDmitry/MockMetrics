using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// Класс исключение для ошибки "Недостаточно данных для приёма или отклонения заявки".
    /// </summary>
    public class BuildingDataNotCompleteException : GUException
    {
        public BuildingDataNotCompleteException()
            : base("Недостаточно данных для приёма или отклонения заявки")
        {

        }
    }
}