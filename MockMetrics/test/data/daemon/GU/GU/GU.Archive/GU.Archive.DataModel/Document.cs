using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;


namespace GU.Archive.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Удостовреяющий документ.
    /// </summary>
    [TableName("gu_archive.document")]
    public abstract class Document : IdentityDomainObject<Document>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_archive.document_seq")]
        [MapField("document_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Серия.
        /// </summary>
        [MapField("serial_number")]
        public abstract string SerialNumber { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>
        [MapField("document_number")]
        public abstract string Number { get; set; }

        /// <summary>
        /// Наименование организации, выдавшей документ.
        /// </summary>
        [MapField("distributor")]
        public abstract string Distributor { get; set; }

        /// <summary>
        /// Наименование организации, выдавшей документ.
        /// </summary>
        [MapField("distributor_dept_code")]
        public abstract string DistributorDeptCode { get; set; }

        /// <summary>
        /// Дата выдачи.
        /// </summary>
        [MapField("stamp")]
        public abstract DateTime? Stamp { get; set; }

        public override string ToString()
        {
            return string.Format("серия {0} № {1}, {2}", SerialNumber, Number, Stamp.HasValue ? Stamp.Value.ToShortDateString() : string.Empty);
        }
    }
}
