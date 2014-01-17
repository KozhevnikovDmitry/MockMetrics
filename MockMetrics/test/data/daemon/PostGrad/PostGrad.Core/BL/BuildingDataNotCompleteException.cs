using PostGrad.Core.Common;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Класс исключение для ошибки "Недостаточно данных для приёма или отклонения заявки".
    /// </summary>
    public class BuildingDataNotCompleteException : BaseException
    {
        public BuildingDataNotCompleteException()
            : base("Недостаточно данных для приёма или отклонения заявки")
        {

        }
    }
}