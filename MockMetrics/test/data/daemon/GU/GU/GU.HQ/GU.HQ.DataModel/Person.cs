using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using GU.HQ.DataModel.Types;

namespace GU.HQ.DataModel
{
    /// <summary>
    /// Информация о заявителе/ ченах его семьи
    /// </summary>
    [TableName("gu_hq.person")]
    public abstract class Person : IdentityDomainObject<Person>,  IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.person_seq")]
        [MapField("person_id")]
        public abstract override int Id{get;set;}

        /// <summary>
        /// Фамилия
        /// </summary>
        [MapField("name1")]
        public abstract string Sname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [MapField("name2")]
        public abstract string Name { get; set; }


        /// <summary>
        /// Отчество
        /// </summary>
        [MapField("name3")]
        public abstract string Patronymic { get; set; }


        /// <summary>
        /// ФИО в настоящее время	
        /// </summary>
        [MapField("fio_current")]
        public abstract string FioCurrent { get; set; }


        /// <summary>
        /// Дата рождения
        /// </summary>
        [MapField("birth_date")]
        public abstract DateTime BirthDate { get; set; }


        /// <summary>
        /// пол
        /// </summary>
        [MapField("sex")]
        public abstract Sex Sex { get; set; }

        /// <summary>
        /// Контактная информация
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "PersonId", CanBeNull = true)]
        public abstract PersonContactInfo ContactInfo { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "PersonId", CanBeNull = true)]
        public abstract EditableList<PersonDoc> Documents { get; set; }

        /// <summary>
        /// Адреса регистрации/проживания
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "PersonId", CanBeNull = true)]
        public abstract EditableList<PersonAddress> Addresses { get; set; }

        /// <summary>
        /// Информация об инвалидности 
        /// </summary>
        [NoInstance]
        [Association(ThisKey  = "Id", OtherKey = "PersonId", CanBeNull = true)]
        public abstract PersonDisability Disability { get; set; }
    }
}
