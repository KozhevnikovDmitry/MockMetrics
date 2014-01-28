using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using Common.DA;
using Common.DA.Interface;

namespace GU.MZ.DataModel.Violation
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности "Информация об устранении нарушений"
    /// </summary>
    [TableName("gumz.violation_resolve")]
    public abstract class ViolationResolveInfo : IdentityDomainObject<ViolationResolveInfo>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("file_scenario_step_id")] 
        public abstract override int Id { get; set; }

        /// <summary>
        /// Возвращает флаг успешного устранения нарушений
        /// </summary>
        [MapField("is_resolved")]
        public abstract bool IsResolved { get; set; }

        /// <summary>
        /// Дата предоставления корректных документов
        /// </summary>
        [MapField("resolve_stamp")]
        public abstract DateTime? ResolveStamp { get; set; }

        /// <summary>
        /// Дата возврата документов заявителю
        /// </summary>
        [MapField("return_stamp")]
        public abstract DateTime? ReturnStamp { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }
    }
}
