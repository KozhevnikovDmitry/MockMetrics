using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.Exceptions;
using GU.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.BL.Policy
{
    public class ClaimPolicy : IClaimStatusPolicy
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
        /// <param name="claimStatusType">Новый статус</param>
        /// <param name="comment">комментарий</param>
        /// <param name="claim">заявка</param>
        /// <returns>заявка с измененным статусом</returns>
        public void SetStatus(ClaimStatusType claimStatusType, string comment, Claim claim)
        {
            if (!IsValidStatusTransition(claim.CurrentCtatusTypeId, claimStatusType))
                throw new DomainBLLException("Неверный статус заявки", claim);

            claim.ClaimStatusHist.Add(CreateStatusHistRow(claimStatusType));
            
            claim.CurrentCtatusTypeId = claimStatusType;
            if (claimStatusType == ClaimStatusType.QueuePrivDeReg)
                claim.CurrentCtatusTypeId = ClaimStatusType.QueueReg;
        }

        /// <summary>
        /// Создать запись в истории заявления
        /// </summary>
        /// <param name="claimStatusType"></param>
        /// <returns></returns>
        private ClaimStatusHist CreateStatusHistRow(ClaimStatusType claimStatusType)
        {
            var claimStatus = ClaimStatusHist.CreateInstance();
            claimStatus.ClaimStatusTypeId = (int) claimStatusType;
            claimStatus.Date = DateTime.Now;

            var uUser = GuFacade.GetDbUser();

            claimStatus.UUserId = uUser.Id;

            return claimStatus;
        }

        /// <summary>
        /// Проверка возможности изменения статуса
        /// </summary>
        /// <param name="claimStatusType"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        public bool CanSetStatus(ClaimStatusType claimStatusType, Claim claim)
        {
            return IsValidStatusTransition(claim.CurrentCtatusTypeId, claimStatusType);
        }

        private bool IsValidStatusTransition(ClaimStatusType oldStatusType, ClaimStatusType newStatusType)
        {
            if (!_validStatusTransitions.ContainsKey(oldStatusType))
                return false;

            return _validStatusTransitions[oldStatusType].Contains(newStatusType);
        }
    }
}
