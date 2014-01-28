using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.Exceptions;
using GU.DataModel;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.BL.Policy
{
    public class ClaimStatusPolicy : IClaimStatusPolicy
    {

        private static readonly Dictionary<ClaimStatusType, ClaimStatusType[]>
         _validStatusTransitions = new Dictionary<ClaimStatusType, ClaimStatusType[]>()
            {
                {ClaimStatusType.DataCheck , new[] { ClaimStatusType.QueueReg, ClaimStatusType.Rejected }},
                {ClaimStatusType.QueueReg, new[] { ClaimStatusType.QueuePrivReg, ClaimStatusType.HouseProvided,  ClaimStatusType.Rejected }},
                {ClaimStatusType.QueuePrivReg, new[] { ClaimStatusType.QueuePrivDeReg, ClaimStatusType.Rejected }},
                {ClaimStatusType.QueuePrivDeReg, new[] { ClaimStatusType.QueueReg, ClaimStatusType.HouseProvided, ClaimStatusType.Rejected }},
                {ClaimStatusType.HouseProvided, new[] { ClaimStatusType.Rejected }}
            };


        /// <summary>
        /// Смена статуса заявки
        /// </summary>
        /// <param name="claim">заявка</param>
        /// <param name="claimStatusType">Новый статус</param>
        /// <param name="uUser"> пользователь</param>
        /// <param name="comment">комментарий</param>
        /// <returns>заявка с измененным статусом</returns>
        public void SetStatus(Claim claim, ClaimStatusType claimStatusType, DbUser uUser, string comment)
        {
            if (!IsValidStatusTransition(claim.CurrentStatusTypeId, claimStatusType))
                throw new DomainBLLException("Неверный статус заявки", claim);

            claim.CurrentStatusTypeId = claimStatusType;
            
            if (claim.ClaimStatusHist == null)
                 claim.ClaimStatusHist = new EditableList<ClaimStatusHist>();

            claim.ClaimStatusHist.Add(CreateStatusHistRow(claimStatusType, uUser));
        }

        /// <summary>
        /// Создать запись в истории заявления
        /// </summary>
        /// <param name="claimStatusType"></param>
        /// <param name="uUser">Пользователь</param>
        /// <returns></returns>
        private ClaimStatusHist CreateStatusHistRow(ClaimStatusType claimStatusType, DbUser uUser)
        {
            var claimStatus = ClaimStatusHist.CreateInstance();
            claimStatus.ClaimStatusTypeId = (int)claimStatusType;
            claimStatus.Date = DateTime.Now;
            claimStatus.UUserId = uUser.Id;

            return claimStatus;
        }

        /// <summary>
        /// Проверка возможности изменения статуса
        /// </summary>
        /// <param name="claim">Заявка</param>
        /// <param name="claimStatusType">Статус</param>
        /// <returns>возможность перевода заявки в указанный статус</returns>
        public bool CanSetStatus(Claim claim, ClaimStatusType claimStatusType)
        {
            return IsValidStatusTransition(claim.CurrentStatusTypeId, claimStatusType);
        }

        /// <summary>
        /// Проверка возможности перехода из статуса1 в статус2
        /// </summary>
        /// <param name="oldStatusType">старый статус</param>
        /// <param name="newStatusType">новый статус</param>
        /// <returns></returns>
        private bool IsValidStatusTransition(ClaimStatusType oldStatusType, ClaimStatusType newStatusType)
        {
            if (!_validStatusTransitions.ContainsKey(oldStatusType))
                return false;

            return _validStatusTransitions[oldStatusType].Contains(newStatusType);
        }

    }
}
