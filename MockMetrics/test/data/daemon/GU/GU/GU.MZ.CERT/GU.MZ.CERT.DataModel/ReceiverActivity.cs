using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.DataAccess;
using Common.Types;
using Common.DA;
using BLToolkit.Mapping;

namespace GU.MZ.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Тип лицензируемой деятельности организации.
    /// </summary>
    [TableName("gumz.person_document_type")]
    public abstract class ReceiverActivity : IdentityDomainObject<ReceiverActivity>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("receiver_activity_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Id связной сущности Лицензируемая деятельность
        /// </summary>
        [MapField("licensed_activity_id")]
        public abstract int LicensedActivityId { get; set; }

        /// <summary>
        /// Id связной сущности Получатель услуги
        /// </summary>
        [MapField("receiver_id")]
        public abstract int ReceiverId { get; set; }

        /// <summary>
        /// Лицензируемая деятельность
        /// </summary>
        [MapIgnore]
        [Association(ThisKey = "LicensedActivityId", OtherKey = "Id", CanBeNull = false)]
        public LicensedActivity LicensedActivity { get; set; }

    }
}
