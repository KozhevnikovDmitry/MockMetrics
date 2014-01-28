using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA;
using Common.DA.Interface;
using BLToolkit.DataAccess;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;

namespace GU.Enisey.DataModel
{
    [TableName("gu_enisey.task_state_send_det")]
    public abstract class TaskStateSendDetail : IdentityDomainObject<TaskStateSendDetail>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_enisey.task_state_send_det_seq")]
        [MapField("task_send_state_det_id")]
        public abstract override int Id { get; set; }

        [MapField("task_state_send_id")]
        public abstract int TaskStateSendId { get; set; }

        [MapField("state")]
        public abstract SendState State { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("error_message")]
        public abstract string ErrorMessage { get; set; }

        [MapField("sended_message")]
        public abstract string SendedMessage { get; set; }

        [MapField("received_message")]
        public abstract string ReceivedMessage { get; set; }
    }
}