using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.Types;
using BLToolkit.TypeBuilder;

namespace GU.MZ.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Глава организации заявителя 
    /// </summary>
    [TableName("gumz.receiver_head")]
    public abstract class ReceiverHead : IdentityDomainObject<ReceiverHead>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey]
        [MapField("person_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Физическое лицо сотрудник
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "Id", CanBeNull = false)]
        public IndividualPerson IndividualPerson { get; set; }

        #region IndividualPerson Proxy Properties

        /// <summary>
        /// Имя сотрудника.
        /// </summary>
        [MapIgnore]
        public string Name
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.Name : "Not personalized";
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.Name = value;
                }
            }
        }

        /// <summary>
        /// Фамилия сотрудника.
        /// </summary>
        [MapIgnore]
        public string Surname
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.Surname : "Not personalized";
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.Surname = value;
                }
            }
        }

        /// <summary>
        /// Отчество сотрудника.
        /// </summary>
        [MapIgnore]
        public string Patronymic
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.Patronymic : "Not personalized";
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.Patronymic = value;
                }
            }
        }

        /// <summary>
        /// Пол сотрудника
        /// </summary>
        [MapIgnore]
        public Sex Sex
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.Sex : DataModel.Sex.Male;
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.Sex = value;
                }
            }
        }

        /// <summary>
        /// Место рождения сотрудника.
        /// </summary>
        [MapIgnore]
        public string BirthPlace
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.BirthPlace : "Not personalized";
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.BirthPlace = value;
                }
            }
        }

        /// <summary>
        /// Место рождения сотрудника.
        /// </summary>
        [MapIgnore]
        public DateTime? BirthDate
        {
            get
            {
                return IndividualPerson != null ? IndividualPerson.BirthDate : null;
            }
            set
            {
                if (IndividualPerson != null)
                {
                    IndividualPerson.BirthDate = value;
                }
            }
        }
        
        #endregion       

    }
}
