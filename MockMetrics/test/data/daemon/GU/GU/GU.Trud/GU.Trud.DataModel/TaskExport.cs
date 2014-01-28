using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.DataModel;

namespace GU.Trud.DataModel
{
    
    [TableName("gu_trud.task_export")]
    public abstract class TaskExport : IdentityDomainObject<TaskExport>, IPersistentObject 
    {
        ///<summary>
        /// Первичный ключ сущности
        ///</summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_trud.task_export_seq")]
        [MapField("task_export_id")]
        public abstract override int Id { get; set; }

        ///<summary>
        /// Id типа выгрузки
        ///</summary>
        [MapField("export_type_id")]
        public abstract int ExportTypeId { get; set; }

        ///<summary>
        /// Тип выгрузки
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "ExportTypeId", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Тип выгрузки", SearchTypeSpec.Dictionary)]
        public abstract ExportType ExportType { get; set; }

        ///<summary>
        /// Дата выгрузки
        ///</summary>
        [MapField("stamp")]
        [SearchField("Дата выгрузки", SearchTypeSpec.Date)]
        public abstract DateTime? Stamp { get; set; }

        ///<summary>
        /// Файл выгрузки
        ///</summary>
        [MapField("data")]
        [NoInstance]
        public abstract byte[] Data { get; set; }

        ///<summary>
        /// Id ведомства, которому принадлжеит выгрузка.
        ///</summary>
        [MapField("agency_id")]
        public abstract int? AgencyId { get; set; }

        ///<summary>
        /// Ведомство, которому принадлежит выгрузка
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "Agencyid", OtherKey = "Id", CanBeNull = true)]
        public abstract Agency Agency { get; set; }


        ///<summary>
        /// Наименование файла выгрузки
        ///</summary>
        [MapField("filename")]
        [SearchField("Имя файла", SearchTypeSpec.String)]
        public abstract string Filename { get; set; }

        ///<summary>
        /// Список детализаций выгрузки
        ///</summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "TaskExportId", CanBeNull = true)]
        public abstract EditableList<TaskExportDet> TaskExportDets { get; set; }



    }
}
