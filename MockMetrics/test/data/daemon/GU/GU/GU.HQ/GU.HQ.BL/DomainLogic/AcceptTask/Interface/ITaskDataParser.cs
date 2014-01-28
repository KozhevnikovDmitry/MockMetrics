using GU.DataModel;
using GU.HQ.DataModel;


namespace GU.HQ.BL.DomainLogic.AcceptTask.Interface
{
    /// <summary>
    /// Интефейс класса, занимающегося импортом сырых данных из заявок в данные сущностей предметной области "Постановка на учет в качестве нуждающегося в муниципальном жилье".
    /// </summary>
    public interface ITaskDataParser
    {
        /// <summary>
        /// Возвращает экземпляр Claim, заполненный по данным заявки.
        /// </summary>
        /// <param name="task">Объект Заявка</param>
        /// <returns>Экземпляр Claim, заполненный по данным заявки</returns>
        Claim GetClaim(Task task);
    }
}
