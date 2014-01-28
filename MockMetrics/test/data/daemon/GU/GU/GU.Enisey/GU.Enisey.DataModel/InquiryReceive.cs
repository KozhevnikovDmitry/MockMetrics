using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;

namespace GU.Enisey.DataModel
{
    [TableName("gu_enisey.inquiry_receive")]
    public abstract class InquiryReceive : IdentityDomainObject<InquiryReceive>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_enisey.inquiry_receive_seq")]
        [MapField("receive_id")]
        public abstract override int Id { get; set; }

        [MapField("inquiry_type")]
        public abstract InquiryType InquiryType { get; set; }

        [MapField("task_id")]
        public abstract int? TaskId { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("enisey_app_id")]
        public abstract string EniseyAppId { get; set; }

        [MapField("enisey_inquiry_id")]
        public abstract string EniseyInquiryId { get; set; }

        [MapField("enisey_inquiry_type_name")]
        public abstract string EniseyInquiryTypeName { get; set; }

        [MapField("enisey_inquiry_state")]
        public abstract int EniseyInquiryState { get; set; }

        // TODO: это поле избыточно, вся инфа отсюда должна быть распихана по другим таблицам
        [MapField("request_content")]
        public abstract string RequestContent { get; set; }

        [MapField("error_message")]
        public abstract string ErrorMessage { get; set; }

        [MapField("received_message")]
        public abstract string ReceivedMessage { get; set; }

        [MapField("sended_message")]
        public abstract string SendedMessage { get; set; }
    }
}
