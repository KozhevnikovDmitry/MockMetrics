using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.Common;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.DomainModel.MzOrder
{
    /// <summary>
    /// Типовой приказ с настройкой
    /// Может быть приказом:
    /// 1 - Возврат заявления
    /// 2 - Принятие к рассмотрению
    /// 3 - Предоставление лицензии
    /// 4 - Отказ в предоставлении лицензии
    /// 5 - Переоформление лицензии
    /// 6 - Отказ в переоформлении лицензии
    /// 7 - Прекращение действия лицензии
    /// </summary>
    [TableName("gumz.standart_order")]
    public abstract class StandartOrder : IdentityDomainObject<StandartOrder>, IPersistentObject, IOrder
    {
        public StandartOrder()
        {
            DetailList = new EditableList<StandartOrderDetail>();
            AgreeList = new EditableList<StandartOrderAgree>();
        }

        [PrimaryKey, Identity]
        [SequenceName("gumz.standart_order_seq")]
        [MapField("standart_order_id")]
        public abstract override int Id { get; set; }
        
        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("reg_number")]
        public abstract string RegNumber { get; set; }

        [MapField("activity_info")]
        public abstract string ActivityInfo { get; set; }

        [MapField("employee_name")]
        public abstract string EmployeeName { get; set; }

        [MapField("employee_position")]
        public abstract string EmployeePosition { get; set; }

        [MapField("employee_contacts")]
        public abstract string EmployeeContacts { get; set; }

        [MapField("licensiar_head_position")]
        public abstract string LicensiarHeadPosition { get; set; }

        [MapField("licensiar_head_name")]
        public abstract string LicensiarHeadName { get; set; }

        [MapField("file_scenario_step_id")]
        public abstract int FileScenarioStepId { get; set; }

        [NoInstance]
        [Association(ThisKey = "FileScenarioStepId", OtherKey = "Id", CanBeNull = true)]
        public DossierFileScenarioStep FileScenarioStep { get; set; }

        [MapField("standart_order_option_id")]
        public abstract int OrderOptionId { get; set; }

        [NoInstance]
        [Association(ThisKey = "OrderOptionId", OtherKey = "Id", CanBeNull = false)]
        public abstract StandartOrderOption OrderOption { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "OrderId", CanBeNull = false)]
        public abstract EditableList<StandartOrderDetail> DetailList { get; set; }

        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "OrderId", CanBeNull = false)]
        public abstract EditableList<StandartOrderAgree> AgreeList { get; set; }

        public override string ToString()
        {
            var ordName = OrderOption != null ? OrderOption.OrderType.GetDescription() : "Приказ";

            return string.Format("{0} №{1} от {2}", ordName, RegNumber, Stamp.ToShortDateString());
        }

        #region Proxy Properties

        [MapIgnore]
        [CloneIgnore]
        public virtual string PreamblePattern
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.Preamble;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string AnnexPreamblePattern
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.AnnexPreamble;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string DetailCommentPattern
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.DetailCommentPattern;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string Name
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.Name;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string SubjectIdName
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.SubjectIdName;
            }
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string SubjectStampName
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.SubjectStampName;
            }
        }
        
        [MapIgnore]
        [CloneIgnore]
        public virtual StandartOrderType OrderType
        {
            get
            {
                if (OrderOption == null)
                {
                    throw new BLLException("Настройка приказа не найдена");
                }

                return OrderOption.OrderType;
            }
        }
        
        #endregion

        #region Calculated Fields

        public void FormatFields(DossierFile dossierFile)
        {
            if (OrderType == StandartOrderType.AcceptTask)
            {
                Preamble = string.Format(PreamblePattern, dossierFile.TaskId, dossierFile.TaskStampShortString, dossierFile.HolderFullName, dossierFile.HolderInn);
                AnnexPreamble = AnnexPreamblePattern;
                return;
            }

            if (OrderType == StandartOrderType.ReturnTask)
            {
                Preamble = string.Format(PreamblePattern, ActivityInfo);
                AnnexPreamble = string.Format(AnnexPreamblePattern, ActivityInfo);
                DetailComment = string.Format(DetailCommentPattern, dossierFile.TaskId, dossierFile.TaskStampShortString);
                return;
            }

            if (OrderType == StandartOrderType.GrantLicense ||
                OrderType == StandartOrderType.RenewalLicense ||
                OrderType == StandartOrderType.StopLicense)
            {
                Preamble = string.Format(PreamblePattern, ActivityInfo);
                AnnexPreamble = string.Format(AnnexPreamblePattern, ActivityInfo);
                DetailComment = string.Format(DetailCommentPattern, dossierFile.TaskId, dossierFile.TaskStampShortString);
                return;
            }

            Preamble = string.Format(PreamblePattern, ActivityInfo);
            AnnexPreamble = string.Format(AnnexPreamblePattern, ActivityInfo);
            DetailComment = DetailCommentPattern;
        }

        [MapIgnore]
        [CloneIgnore]
        public virtual string Preamble { get; set; }

        [MapIgnore]
        [CloneIgnore]
        public virtual string AnnexPreamble { get; set; }

        [MapIgnore]
        [CloneIgnore]
        public virtual string DetailComment { get; set; } 
        
        #endregion
    }
}