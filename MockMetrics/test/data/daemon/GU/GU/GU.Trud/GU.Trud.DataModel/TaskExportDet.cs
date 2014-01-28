using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.DataModel;

namespace GU.Trud.DataModel
{
    [TableName("gu_trud.task_export_det")]
    public abstract class TaskExportDet : DomainObject<TaskExportDet>, IPersistentObject
    {
        #region DomainObject

        public override string GetKeyValue()
        {
            return string.Format("{0};{1}", TaskExportId, TaskId);
        }

        public override void SetKeyValue(object val)
        {
            try
            {
                if (val != null)
                {
                    TaskExportId = Convert.ToInt32(val.ToString().Split(';')[0]);
                    TaskId = Convert.ToInt32(val.ToString().Split(';')[1]);
                }
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка присвоения значения составному ключу сущности TaskExportDet", ex);
            }
        }

        [MapIgnore]
        public override PersistentState PersistentState { get; set; }

        #endregion

        ///<summary>
        /// Id выгрузки
        ///</summary>
        [MapField("task_export_id")]
        public abstract int TaskExportId { get; set; }

        ///<summary>
        /// Выгрузка
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "TaskExportId", OtherKey = "Id", CanBeNull = false)]
        public TaskExport TaskExport { get; set; }

        ///<summary>
        /// Id заявления
        ///</summary>
        [MapField("task_id")]
        public abstract int TaskId { get; set; }

        ///<summary>
        /// Заявление
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = false)]
        public Task Task { get; set; }

    }
}
