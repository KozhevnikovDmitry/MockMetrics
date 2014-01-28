using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace GU.MZ.DataModel.MzOrder
{
    /// <summary>
    /// Детализация типового договора
    /// Печатается в приложении в списке
    /// </summary>
    [TableName("gumz.standart_order_detail")]
    public abstract class StandartOrderDetail : IdentityDomainObject<StandartOrderDetail>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.standart_order_detail_seq")]
        [MapField("standart_order_detail_id")]
        public abstract override int Id { get; set; }
        
        /// <summary>
        /// Номер предмета детализации - номер лицензии или заявления
        /// </summary>
        [MapField("detail_subject_id")]
        public abstract string SubjectId { get; set; }

        /// <summary>
        /// Дата предмета детализации - дата оформления лицензии или подачи заявления
        /// </summary>
        [MapField("detail_subject_stamp")]
        public abstract DateTime? SubjectStamp { get; set; }

        [MapField("full_name")]
        public abstract string FullName { get; set; }

        [MapField("short_name")]
        public abstract string ShortName { get; set; }

        [MapField("firm_name")]
        public abstract string FirmName { get; set; }

        [MapField("address")]
        public abstract string Address { get; set; }

        [MapField("inn")]
        public abstract string Inn { get; set; }

        [MapField("ogrn")]
        public abstract string Ogrn { get; set; }

        [MapField("comment")]
        public abstract string Comment { get; set; }

        [MapField("standart_order_id")]
        public abstract int OrderId { get; set; }

        [NoInstance]
        [Association(ThisKey = "OrderId", OtherKey = "Id", CanBeNull = false)]
        public StandartOrder StandartOrder { get; set; }

        [MapIgnore]
        [CloneIgnore]
        public virtual string SubjectIdName
        {
            get
            {
                if (StandartOrder == null)
                {
                    throw new BLLException("Приказ для детализации не найден");
                }

                return StandartOrder.SubjectIdName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string SubjectStampName
        {
            get
            {
                if (StandartOrder == null)
                {
                    throw new BLLException("Приказ для детализации не найден");
                }

                return StandartOrder.SubjectStampName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string DetailComment
        {
            get
            {
                if (StandartOrder == null)
                {
                    throw new BLLException("Приказ для детализации не найден");
                }

                return StandartOrder.DetailComment;
            }
        }
    }
}