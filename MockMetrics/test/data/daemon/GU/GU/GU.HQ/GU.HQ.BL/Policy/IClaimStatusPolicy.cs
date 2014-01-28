using GU.DataModel;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.Policy
{
    /// <summary>
    /// Интерфейс класса политики управления статусами заявки
    /// </summary>
    public interface IClaimStatusPolicy
    {
        /// <summary>
        /// Изменение статуса заявки
        /// </summary>
        /// <param name="claim">Объект заявка</param>
        /// <param name="claimStatusType">Тип статуса</param>
        /// <param name="uUser"> </param>
        /// <param name="comment">Комментарий к операции изменения статуса</param>
        /// <returns>Заявка с добавленным статусом</returns>
        void SetStatus(Claim claim, ClaimStatusType claimStatusType, DbUser uUser, string comment);

        /// <summary>
        /// Возвращает флаг возможности изменения статуса заявки с учетом всех проверок
        /// </summary>
        /// <param name="claim">Объект заявка</param>
        /// <param name="claimStatusType">Тип статуса</param>
        /// <returns>Флаг возможности добавления статусов</returns>
        bool CanSetStatus(Claim claim, ClaimStatusType claimStatusType);
    }
}