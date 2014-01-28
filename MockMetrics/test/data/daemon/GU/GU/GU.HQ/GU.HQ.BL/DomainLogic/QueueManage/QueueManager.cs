using System;
using System.Linq;
using BLToolkit.Data.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.DataModel;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.DomainLogic.QueueManage
{
    public class QueueManager : DomainDependent, IQueueManager
    {
        private readonly IClaimStatusPolicy _claimStatusPolicy;

        public QueueManager(IDomainContext domainContext, IClaimStatusPolicy claimStatusPolicy) 
            : base(domainContext)
        {
            _claimStatusPolicy = claimStatusPolicy;
        }

        #region Claim Queue Manage
        
        /// <summary>
        /// Поставить заявку в очередь.
        /// </summary>
        /// <param name="claim">Заявка</param>
        /// <param name="uUser">Пользователь </param>
        /// <param name="claimDataMapper">Маппер для сохранения заявки </param>
        /// <returns>Сохраненная заявка</returns>
        public Claim RegistrClaimInQueue(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper)
        {
            using (var dbManager = GetDbManager())
            {
                try
                {
                    dbManager.BeginDomainTransaction();
                    
                    QueueInsertClaim(dbManager, claim, uUser, QueueType.QueueSimple);

                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.QueueReg, uUser, "");

                    var newClaim = claimDataMapper.Save(claim, dbManager);

                    dbManager.CommitDomainTransaction();

                    return newClaim;
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception ex)
                {
                    dbManager.RollbackDomainTransaction();
                    throw new BLLException(string.Format("Ошибка при сохранении данных сущности."), ex);
                }
            }
        }

        /// <summary>
        /// Поставить заявку в очередь внеочередников
        /// </summary>
        /// <param name="claim">Заявка</param>
        /// <param name="uUser">Пользователь </param>
        /// <param name="claimDataMapper">Маппер для сохранения заявки </param>
        /// <returns>Сохраненная заявка</returns>
        public Claim RegistrClaimInQueuePriv(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper)
        {
            using (var dbManager = GetDbManager())
            {
                try
                {
                    dbManager.BeginDomainTransaction();

                    QueueRejectClaim(dbManager, claim);

                    QueueInsertClaim(dbManager, claim, uUser, QueueType.QueuePriv);                    

                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.QueuePrivReg, uUser, "");

                    var newClaim = claimDataMapper.Save(claim, dbManager);

                    dbManager.CommitDomainTransaction();

                    return newClaim;
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception ex)
                {
                    dbManager.RollbackDomainTransaction();
                    throw new BLLException(string.Format("Ошибка при сохранении данных сущности."), ex);
                }
            }
        }

        /// <summary>
        /// удалить из списка внеочередников
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="uUser"></param>
        /// <param name="claimDataMapper"></param>
        /// <returns></returns>
        public Claim DeRegClaimInQueuePriv(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper)
        {
            using (var dbManager = GetDbManager())
            {
                try
                {
                    dbManager.BeginDomainTransaction();
                    
                    QueueRejectClaim(dbManager, claim);

                    QueueInsertClaim(dbManager,claim,uUser,QueueType.QueueSimple);

                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.QueuePrivDeReg, uUser, "");
                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.QueueReg, uUser, "");

                    var newClaim = claimDataMapper.Save(claim, dbManager);

                    dbManager.CommitDomainTransaction();

                    return newClaim;
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception ex)
                {
                    dbManager.RollbackDomainTransaction();
                    throw new BLLException(string.Format("Ошибка при сохранении данных сущности."), ex);
                }
            }
        }

        /// <summary>
        /// Жилье предоставленно
        /// </summary>
        /// <param name="claim">заявка</param>
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        public Claim ClaimHouseProvided(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper)
        {
            using (var dbManager = GetDbManager())
            {
                try
                {
                    dbManager.BeginDomainTransaction();

                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.HouseProvided, uUser, "");

                    var newClaim = claimDataMapper.Save(claim, dbManager);

                    dbManager.CommitDomainTransaction();

                    return newClaim;
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception ex)
                {
                    dbManager.RollbackDomainTransaction();
                    throw new BLLException(string.Format("Ошибка при сохранении данных сущности."), ex);
                }
            }
        }

        /// <summary>
        /// Исключить из очереди
        /// </summary>
        /// <param name="claim">заявка</param>
        /// <param name="uUser"> пользователь </param>
        /// <param name="claimDataMapper"> маппер для сохранения заявки </param>
        /// <returns>Заявка сохраненная в базе</returns>
        public  Claim DeRegClaimInQueue(Claim claim, DbUser uUser, IDomainDataMapper<Claim> claimDataMapper)
        {
            using (var dbManager = GetDbManager())
            {
                try
                {
                    dbManager.BeginDomainTransaction();

                    QueueRejectClaim(dbManager, claim);

                    _claimStatusPolicy.SetStatus(claim, ClaimStatusType.Rejected, uUser, "");

                    var newClaim = claimDataMapper.Save(claim, dbManager);

                    dbManager.CommitDomainTransaction();

                    return newClaim;
                }
                catch (BLLException)
                {
                    throw;
                }
                catch (TransactionControlException ex)
                {
                    throw new BLLException(ex);
                }
                catch (Exception ex)
                {
                    dbManager.RollbackDomainTransaction();
                    throw new BLLException(string.Format("Ошибка при сохранении данных сущности."), ex);
                }
            }
        }

        #endregion Claim Queue Manage

        #region Common

        /// <summary>
        /// Получить идентификатор очереди
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="agencyId"> Ведомство</param>
        /// <param name="queueTypeId"> Тип очереди</param>
        /// <returns></returns>
        private int GetQueueId(IDomainDbManager dbManager, int agencyId, QueueType queueTypeId)
        {
            return (from q in dbManager.GetDomainTable<Queue>()
                    where q.AgencyId == agencyId && q.QueueTypeId == (int)queueTypeId
                    select q.Id).Single();
        }

        /// <summary>
        /// Получить номер в очереди, который необходимо присвоить поступившей заявке.
        /// И с которого необходимо пересчитать очередь
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="claim">заявка</param>
        /// <param name="queueTypeId"> </param>
        /// <returns>номер очереди</returns>
        private int GetQueueClaimNum(IDomainDbManager dbManager, Claim claim, QueueType queueTypeId)
        {
            var queueNums = (from qc in dbManager.GetDomainTable<QueueClaim>()
                             join q in dbManager.GetDomainTable<Queue>() on qc.QueueId equals q.Id
                             join c in dbManager.GetDomainTable<Claim>() on q.AgencyId equals c.AgencyId
                             where c.ClaimDate > claim.ClaimDate && q.QueueTypeId == (int)queueTypeId
                             select qc.QueueNum);

            return queueNums.Any() ? queueNums.Max() : 1;
        }

        #region Common Claim

        /// <summary>
        /// Заполнить объект информацией об очереди и сохранить
        /// </summary>
        private void ClaimQueueRegInfoSave(Claim claim, int queueId, int queueNum, IDomainDbManager dbManager)
        {
            // присваиваем нашему делу найденный номер
            var queueClaim = QueueClaim.CreateInstance();
            queueClaim.QueueId = queueId;
            queueClaim.QueueNum = queueNum;
            queueClaim.ClaimId = claim.Id;

            dbManager.SaveDomainObject(queueClaim);
            claim.QueueClaim = queueClaim;
        }

        /// <summary>
        /// Удалить заявление из очереди
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="claim"></param>
        private void ClaimQueueDeReg(IDomainDbManager dbManager, Claim claim)
        {
            dbManager.GetDomainTable<QueueClaim>().Delete(x => x.Id == claim.QueueClaim.Id);
            claim.QueueClaim = null;
        }

        #endregion Common Claim 

        #region Common Queue

        /// <summary>
        /// Блокировка очереди для изменения 
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="queueId">идентификатор очереди</param>
        private void QueueLock(IDomainDbManager dbManager, int queueId)
        {
            dbManager.GetDomainTable<Queue>()
                .Where(q => q.Id == queueId)
                .Set(q => q.Locked, 1)
                .Update();
        }

        /// <summary>
        /// Включить в очередь + пересчитать очередь
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="claim"></param>
        /// <param name="uUser"> </param>
        /// <param name="queueType"> </param>
        private void QueueInsertClaim(IDomainDbManager dbManager, Claim claim, DbUser uUser, QueueType queueType)
        {
            var queueId = GetQueueId(dbManager, claim.AgencyId, queueType);

            QueueLock(dbManager, queueId);

            var queueClaimNum = GetQueueClaimNum(dbManager, claim, queueType);

            QueueRecalc(dbManager, queueId, queueClaimNum, 1);

            ClaimQueueRegInfoSave(claim, queueId, queueClaimNum, dbManager);
        }

        /// <summary>
        /// исключить из очереди + пересчитать очередь
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="claim"></param>
        private void QueueRejectClaim(IDomainDbManager dbManager, Claim claim)
        {
            // запоминаем идентификатор очереди из которой исключаем человека
            var queueId = claim.QueueClaim.QueueId;

            // заблокировать очередь
            QueueLock(dbManager, queueId);

            // запоминаем номер в очереди с которого нужно начать пересчет
            var queueClaimNum = claim.QueueClaim.QueueNum;

            // удалить из очереди
            ClaimQueueDeReg(dbManager, claim);

            // пересчитать очередь
            QueueRecalc(dbManager, queueId, queueClaimNum, -1);
        }


        /// <summary>
        /// пересчитываем очередь
        /// </summary>
        /// <param name="dbManager"></param>
        /// <param name="queueId">идентификатор очереди</param>
        /// <param name="queueNum">Номер учетного дела с которого необходимо произвести пересчет</param>
        /// <param name="upDown"> шаг на который нужно заявлени еопустить вниз или поднять вверх </param>
        private void QueueRecalc(IDomainDbManager dbManager, int queueId, int queueNum, int upDown)
        {
            dbManager.GetDomainTable<QueueClaim>()
                .Where(qc => qc.QueueId == queueId && qc.QueueNum >= queueNum)
                .Set(qc => qc.QueueNum, qc => qc.QueueNum + upDown)
                .Update();
        }

        #endregion Common Queueu

        #endregion Common
    }
}