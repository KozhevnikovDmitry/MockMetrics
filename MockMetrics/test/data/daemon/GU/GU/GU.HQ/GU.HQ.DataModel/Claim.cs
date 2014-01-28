
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Список заявок/учетных дел от граждан, для постановки на учет
    /// </summary>
    [TableName("gu_hq.claim")]
    public abstract class Claim : IdentityDomainObject<Claim>, IPersistentObject
    {
        /// <summary>
        /// Идентификатор заявки/учетного дела
        /// </summary>
        [PrimaryKey,Identity]
        [SequenceName("gu_hq.claim_seq")]
        [MapField("claim_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Статус заявки/учетного дела
        /// </summary>
        [MapField("claim_status_type_id")]
        public abstract ClaimStatusType CurrentStatusTypeId { get; set; }

        /// <summary>
        /// Территориальное подразделение, ведущее заявку/учетное дело (наименования  муниципальных образований и «администрация края» по справочнику) (1)
        /// </summary>
        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }

        [NoInstance]
        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = false)]
        public abstract Agency Agency { get; set; }

        /// <summary>
        /// Номер учетного дела в книге учета 
        /// </summary>
        [MapField("area_file_num")]
        public abstract string AreaFileNum { get; set; }

        /// <summary>
        /// дата подачи заявки
        /// </summary>
        [MapField("claim_date")]
        public abstract DateTime? ClaimDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [MapField("note")]
        public abstract string Note { get; set; }

        #region Declarer 

        /// <summary>
        /// Заявитель
        /// </summary>
        [MapField("declarer_id")]
        public abstract int DeclarerId { get; set; }

        /// <summary>
        /// Данные заявителя
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "DeclarerId", OtherKey = "Id", CanBeNull = false)]
        public abstract Person Declarer { get; set; }

        /// <summary>
        /// Основания учета которые указывает заявитель
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract DeclarerBaseReg DeclarerBaseReg { get; set; }

        /// <summary>
        /// списко родственников
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract EditableList<DeclarerRelative> Relatives { get; set; }


        #endregion Declarer 

        #region Task

        /// <summary>
        /// Идентификатор заявки на получение гос услуги из схемы GU
        /// </summary>
        [MapField("task_id")]
        public abstract int TaskId { get; set; }

        /// <summary>
        /// Ссылка на Task
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "TaskId", OtherKey = "Id", CanBeNull = false)]
        public Task Task { get; set; }

        #endregion Task

        #region RegQueue

        /// <summary>
        /// Список категорий учета
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = false)]
        public abstract EditableList<ClaimCategory> ClaimCategories { get; set; }

        /// <summary>
        /// Информация о регистраци заявления
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract ClaimQueueReg QueueReg { get; set; }

        /// <summary>
        /// Уведомление о постановке на учет
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract EditableList<Notice> Notices { get; set; }

        #endregion RegQueue

        #region PrivQueue

        /// <summary>
        /// информация о внеочередном предоставлении жилья
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract EditableList<QueuePriv> QueuePrivList { get; set; }

        #endregion RegPrivQueue

        #region DeRegQueue

        /// <summary>
        /// Информация о снятии заявления с регистрации
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract ClaimQueueDeReg QueueDeReg { get; set; }

        /// <summary>
        /// информация о предоставленном жилье
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract HouseProvided HouseProvided { get; set; }

        #endregion DeRegQueue

        /// <summary>
        /// Информация о регистрации заявления
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = true)]
        public abstract QueueClaim QueueClaim { get; set; }

        /// <summary>
        /// история изменения статуса заявления
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ClaimId", CanBeNull = false)]
        public abstract EditableList<ClaimStatusHist> ClaimStatusHist { get; set; }

        /// <summary>
        /// выводится в заголовке AvalonDoc
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("№ {0} от {1:dd.MM.yyyy}", Id, ClaimDate); 
        }
    }
}