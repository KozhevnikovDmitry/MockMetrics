using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonVM : DomainValidateableVM<Person>
    {
        public PersonVM(Person entity, IDomainValidator<Person> validator, bool isValidateable = true)
            : base(entity, validator, isValidateable)
        {
            ListSex = HqFacade.GetDictionaryManager().GetEnumDictionary<Sex>();
        }

        #region Binding Properties

        public Dictionary<int, string> ListSex { get; private set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Sname
        {
            get { return Entity.Sname; }
            set { Entity.Sname = value; }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get { return Entity.Name; }
            set { Entity.Name = value; }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic
        {
            get { return Entity.Patronymic; }
            set { Entity.Patronymic = value; }
        }

        /// <summary>
        /// Текущее ФИО
        /// </summary>
        public string FioCurrent
        {
            get { return Entity.FioCurrent; }
            set { Entity.FioCurrent = value; }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate
        {
            get { return Entity.BirthDate; }
            set { Entity.BirthDate = value; }
        }

        /// <summary>
        /// Пол
        /// </summary>
        public int SexVal
        {
            get { return (int)Entity.Sex; }
            set {  Entity.Sex = (Sex)value; }
        }

        #endregion Binding Properties
    }
}
