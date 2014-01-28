using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BLToolkit.EditableObjects;
using Common.BL.Exceptions;
using Common.BL.Validation;
using Common.Types.Exceptions;

using GU.BL.Extensions;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.DataModel.Inquiry;

namespace GU.BL.Policy
{
    public class InquiryPolicy : IInquiryPolicy
    {
        private readonly DbUser _dbUser;

        private readonly IContentPolicy _contentPolicy;

        private static readonly Dictionary<InquiryStatusType, InquiryStatusType[]>
            _validStatusTransitions = new Dictionary<InquiryStatusType, InquiryStatusType[]>()
            {
                {InquiryStatusType.None, new[] { InquiryStatusType.NotFilled, InquiryStatusType.Rejected }},
                {InquiryStatusType.NotFilled, new[] { InquiryStatusType.Sending, InquiryStatusType.Rejected }},
                {InquiryStatusType.Sending, new[] { InquiryStatusType.Accepted, InquiryStatusType.Invalid, InquiryStatusType.FailedToGetResponse, InquiryStatusType.FailedToSendRequest, InquiryStatusType.ResponseReceived, InquiryStatusType.Rejected }},
                {InquiryStatusType.Accepted, new[] { InquiryStatusType.ResponseReceived, InquiryStatusType.Invalid, InquiryStatusType.FailedToGetResponse, InquiryStatusType.Rejected }}
            };

        public InquiryPolicy(DbUser dbUser, IContentPolicy contentPolicy)
        {
            _dbUser = dbUser;
            _contentPolicy = contentPolicy;
        }

        #region State management

        
        public bool IsValidStatusTransition(InquiryStatusType oldStatusType, InquiryStatusType newStatusType)
        {
            if (!_validStatusTransitions.ContainsKey(oldStatusType))
                return false;

            return _validStatusTransitions[oldStatusType].Contains(newStatusType);
        }

        public ValidationErrorInfo ValidateSetStatus(InquiryStatusType inquiryStatusType, Inquiry inquiry)
        {
            if (!IsValidStatusTransition(inquiry.CurrentState, inquiryStatusType))
                throw new DomainBLLException("Неверный статус запроса", inquiry);

            var validationResult = new ValidationErrorInfo();
            if (inquiry.CurrentState != InquiryStatusType.None)
            {
                //TODO: ValidateBasic for inquiry
                //validationResult.AddResult(ValidateBasic(task));
            }

            //TODO: validate content for inquiry
            /* 
            if (inquiryStatusType == InquiryStatusType.)
            {
                validationResult.AddResult(ValidateDocumentsData(task));
            }*/

            return validationResult;
        }

        public Inquiry SetStatus(InquiryStatusType inquiryStatusType, string comment, Inquiry inquiry)
        {
            var validationResult = ValidateSetStatus(inquiryStatusType, inquiry);
            if (!validationResult.IsValid)
                throw new DomainBLLException(validationResult.ToString(), inquiry);

            DateTime stamp = DateTime.Now;

            inquiry.CurrentState = inquiryStatusType;

            var ts = InquiryStatus.CreateInstance();
            ts.Stamp = stamp;
            ts.Inquiry = inquiry;
            ts.InquiryId = inquiry.Id;
            ts.UserId = _dbUser.Id;
            ts.User = _dbUser;
            ts.State = inquiryStatusType;
            ts.Note = comment;
            inquiry.StatusList.Add(ts);

            return inquiry;
        }

        #endregion


        #region Validation

        public ValidationErrorInfo Validate(Inquiry inquiry)
        {
            var result = new ValidationErrorInfo();

            // для начальных статусов не проверяется заполеннность документов
            if (inquiry.CurrentState != InquiryStatusType.NotFilled &&
               inquiry.CurrentState != InquiryStatusType.None)
            {
                if (inquiry.RequestContent == null)
                    throw new DomainBLLException("Отсутствуют данные запроса", inquiry);
                _contentPolicy.Validate(inquiry.RequestContent);
            }

            if (inquiry.CurrentState == InquiryStatusType.ResponseReceived)
            {
                if (inquiry.ResponseContent == null)
                    throw new DomainBLLException("Отсутствуют данные ответа", inquiry);
            }
            else
            {
                if (inquiry.ResponseContent != null)
                    throw new DomainBLLException("Заполнены данные ответа для статуса отличного от \"получен ответ\"", inquiry);
            }

            return result;
        }

        public ValidationErrorInfo CanSave(Inquiry inquiry)
        {
            return Validate(inquiry);
        }

        #endregion


        public Inquiry CreateEmptyInquiry(InquiryType inquiryType, Task task = null)
        {
            var inquiry = Inquiry.CreateInstance();

            inquiry.InquiryType = inquiryType;
            
            // дата создания проставляется когда запрос будет полностью заполнен (переведен в след. статус)
            inquiry.CreateDate = null;
            inquiry.CurrentState = InquiryStatusType.None;

            inquiry.Task = task;

            inquiry.StatusList = new EditableList<InquiryStatus>();
            this.SetStatus(InquiryStatusType.NotFilled, string.Empty, inquiry);

            inquiry.AcceptChanges();

            return inquiry;
        }
        
    }
}
