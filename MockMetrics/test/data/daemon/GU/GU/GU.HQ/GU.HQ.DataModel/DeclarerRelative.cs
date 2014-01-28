
using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Родственник заявителя
    /// </summary>
    [TableName("gu_hq.declarer_relative")]
    public abstract class DeclarerRelative : IdentityDomainObject<DeclarerRelative>, IPersistentObject
    {
        /// <summary>
        /// уникальный иденотификатор объекта
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.declarer_relative_seq")]
        [MapField("declarer_relative_id")]
        public abstract override int Id { get; set; }
        
        /// <summary>
        /// Идентификатор заявления
        /// </summary>
        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        /// <summary>
        /// идентификатор заявителя
        /// </summary>
        [MapField("person_id")]
        public abstract int PersonId { get; set; }

        /// <summary>
        /// прокси поле для Фамилии родственника
        /// нужно для валидации
        /// </summary>
        [MapIgnore]
        public string Sname
        {
            get
            {
                return this.Person.Sname;
            }
            set
            {
                this.Person.Sname = value;
            }
        }

        /// <summary>
        /// прокси поле для Имени родственника
        /// нужно для валидации
        /// </summary>
        [MapIgnore]
        public string Name
        {
            get
            {
                return this.Person.Name;
            }
            set
            {
                this.Person.Name = value;
            }
        }

       
        /// <summary>
        /// прокси поле для Отчества родственника
        /// нужно для валидации
        /// </summary>
        [MapIgnore]
        public string Patronymic
        {
            get
            {
                return this.Person.Patronymic;
            }
            set
            {
                this.Person.Patronymic = value;
            }
        }

        /// <summary>
        /// прокси поле для Даты рождения родственника
        /// нужно для валидации
        /// </summary>
        [MapIgnore]
        public DateTime BirthDate
        {
            get
            {
                return this.Person.BirthDate;
            }
            set
            {
                this.Person.BirthDate = value;
            }
        }

        /// <summary>
        /// информация о родственнике
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "PersonId", OtherKey = "Id", CanBeNull = false)]
        public abstract Person Person { get; set; }

        /// <summary>
        /// родственные отношения
        /// </summary>
        [MapField("relative_type_id")]
        public abstract int RelativeTypeId { get; set; }

        /// <summary>
        /// Степень родства из справочника
        /// </summary>
        [NoInstance]
        public abstract RelativeType RelativeType { get; set; }
    }
}
