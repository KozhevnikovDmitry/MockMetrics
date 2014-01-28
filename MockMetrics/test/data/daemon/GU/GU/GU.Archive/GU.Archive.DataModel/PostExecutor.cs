using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Исполнитель, текущий ответственный
    /// </summary>
    [TableName("gu_archive.post_executor")]
    public abstract class PostExecutor : IdentityDomainObject<PostExecutor>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_archive.post_executor_seq")]
        [MapField("post_executor_id")]
        public abstract override int Id { get; set; }

        [MapField("post_id")]
        public abstract int PostId { get; set; }

        [Association(ThisKey = "PostId", OtherKey = "Id", CanBeNull = false)]
        public Post Post { get; set; }

        [MapField("employee_id")]
        public abstract int EmployeeId { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id", CanBeNull = false)]
        public Employee Employee { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("note")]
        public abstract string Note { get; set; }
    }
}
