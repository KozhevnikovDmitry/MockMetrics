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
    [TableName("gu_enisey.task_send")]
    public abstract class TaskSend : IdentityDomainObject<TaskSend>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_enisey.task_send_seq")]
        [MapField("task_send_id")]
        public abstract override int Id { get; set; }

        [MapField("task_id")]
        public abstract int TaskId { get; set; }

        [MapField("current_state")]
        public abstract SendState CurrentState { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("enisey_app_id")]
        public abstract string EniseyAppId { get; set; }
    }
}
