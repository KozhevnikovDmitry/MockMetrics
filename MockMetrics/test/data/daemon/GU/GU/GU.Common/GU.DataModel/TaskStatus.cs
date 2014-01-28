using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.DataModel
{
    [TableName("gu.task_status")]
    public abstract class TaskStatus : IdentityDomainObject<TaskStatus>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.task_status_seq")]
        [MapField("task_status_id")]
        public abstract override int Id { get; set; }

        [MapField("task_id")]
        public abstract int TaskId { get; set; }

        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = false)]
        public Task Task { get; set; }

        [MapField("task_status_type_id")]
        public abstract TaskStatusType State { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }

        [MapField("user_id")]
        public abstract int UserId { get; set; }

        [NoInstance]
        [Association(ThisKey = "UserId", OtherKey = "Id", CanBeNull = false)]
        public DbUser User { get; set; }
    }
}
