using Common.BL.DataMapping;
using GU.DataModel;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainLogic.QueueManage
{
    public interface IQueueManager
    {
        /// <summary>
        /// Поставить заявку в очередь.
        /// </summary>
        /// <param name="claim">заявка</param;
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        Claim RegistrClaimInQueue(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper);

        /// <summary>
        /// Поставить заявку в очередь внеочередников.
        /// </summary>
        /// <param name="claim">заявка</param;
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        Claim RegistrClaimInQueuePriv(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper);

        /// <summary>
        /// Исключить заявку из очереди внеочередников
        /// </summary>
        /// <param name="claim">заявка</param;
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        Claim DeRegClaimInQueuePriv(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper);

        /// <summary>
        /// Предоставить жильё
        /// </summary>
        /// <param name="claim">заявка</param;
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        Claim ClaimHouseProvided(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper);

        /// <summary>
        /// Исключить из очереди
        /// </summary>
        /// <param name="claim">заявка</param;
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        Claim DeRegClaimInQueue(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper);
    }
}