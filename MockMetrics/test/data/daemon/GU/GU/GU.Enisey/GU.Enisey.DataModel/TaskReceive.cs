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
    [TableName("gu_enisey.task_receive")]
    public abstract class TaskReceive : IdentityDomainObject<TaskReceive>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_enisey.task_receive_seq")]
        [MapField("task_receive_id")]
        public abstract override int Id { get; set; }

        [MapField("task_id")]
        public abstract int? TaskId { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("enisey_app_id")]
        public abstract string EniseyAppId { get; set; }

        [MapField("error_message")]
        public abstract string ErrorMessage { get; set; }

        [MapField("received_message")]
        public abstract string ReceivedMessage { get; set; }

        [MapField("sended_message")]
        public abstract string SendedMessage { get; set; }
    }
}
