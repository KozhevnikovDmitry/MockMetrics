using System;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.MZ.DataModel.Person
{
    /// <summary>
    /// Класс представляющий сущность Эксперт
    /// </summary>
    [TableName("gumz.expert")]
    [SearchClass("Эксперт")]
    public abstract class Expert : IdentityDomainObject<Expert>, IPersistentObject
    {
        /// <summary>
        /// Класс представляющий сущность Эксперт
        /// </summary>
        protected Expert()
        {
            this.AccreditateActivityid = 1;
            this.AccreditationDueDate = DateTime.Today.AddYears(1);
            this.ExpertStateType = ExpertStateType.Individual;

            this.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == Util.GetPropertyName(() => ExpertStateType))
                    {
                        ExpertState = this.ExpertStateType == ExpertStateType.Individual
                                          ? (IExpertState)IndividualExpertState.CreateInstance()
                                          : (IExpertState)JuridicalExpertState.CreateInstance();

                        ExpertState.Id = this.Id;

                        if (ExpertStateTypeChanged != null)
                        {
                            ExpertStateTypeChanged(this.ExpertStateType);
                        }
                    }
                };
        }

        /// <summary>
        /// Событие оповещающее об изменении типа состояния эксперта
        /// </summary>
        public event Action<ExpertStateType> ExpertStateTypeChanged;

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [Identity]
        [SequenceName("gumz.expert_seq")]
        [MapField("expert_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Номер свидетельства аккредитации
        /// </summary>
        [MapField("acc_doc_number")]
        [SearchField("№ свидетельства аккредитации", SearchTypeSpec.String)]
        public abstract string AccreditateDocumentNumber { get; set; }

        /// <summary>
        /// Срок истечения аккредитации
        /// </summary>
        [MapField("accreditation_due_date")]
        [SearchField("Срок истечения аккредитации", SearchTypeSpec.Date)]
        public abstract DateTime? AccreditationDueDate { get; set; }

        /// <summary>
        /// Id сущности Деятельность, на которую аккредитован эксперт
        /// </summary>
        [MapField("accreditate_activity_id")]
        public abstract int AccreditateActivityid { get; set; }

        /// <summary>
        /// Деятельность, на которую аккредитован эксперт
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AccreditateActivityid", OtherKey = "Id", CanBeNull = false)]
        [SearchField("Деятельность", SearchTypeSpec.Dictionary)]
        public abstract AccreditateActivity AccreditateActivity { get; set; }

        /// <summary>
        /// Тип состояния эксперта
        /// </summary>
        [MapField("state_type")]
        public abstract ExpertStateType ExpertStateType { get; set; }

        /// <summary>
        /// Состояние эксперта - физическое или юридическое лицо
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public abstract IExpertState ExpertState { get; set; }

        #region Overrides of EditableObject

        /// <summary>
        /// Флаг, указывающий на наличие несохранённых изменений в объекте 
        /// </summary>
        [MapIgnore]
        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || this.ExpertState.IsDirty;
            }
        }

        /// <summary>
        /// Помечает поля объекта как сохранённые.
        /// </summary>
        public override void AcceptChanges()
        {
            base.AcceptChanges();
            if (ExpertState != null)
            {
                ExpertState.AcceptChanges();
            }
        }

        #endregion

        #region Overrides of Object

        public override string ToString()
        {
            return this.ExpertState != null ? this.ExpertState.GetName() : "Ошибка. Данные эксперта не ассоциированы";
        }

        #endregion
    }
}