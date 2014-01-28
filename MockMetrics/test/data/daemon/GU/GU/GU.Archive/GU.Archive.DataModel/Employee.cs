using System;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.DataModel;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Сотрудник министерства
    /// </summary>
    [TableName("gu_archive.employee")]
    [SearchClass("Сотрудник")]
    public abstract class Employee : IdentityDomainObject<Employee>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_archive.employee_seq")]
        [MapField("employee_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [MapField("surname")]
        [SearchField("Фамилия", SearchTypeSpec.String)]
        public abstract string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [MapField("name")]
        [SearchField("Имя", SearchTypeSpec.String)]
        public abstract string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [MapField("patronymic")]
        [SearchField("Отчество", SearchTypeSpec.String)]
        public abstract string Patronymic { get; set; }

        /// <summary>
        /// Телефон сотрудника
        /// </summary>
        [MapField("phone")]
        [SearchField("Телефон", SearchTypeSpec.String)]
        public abstract string Phone { get; set; }

        /// <summary>
        /// Электронная почта сотрудника
        /// </summary>
        [MapField("email")]
        [SearchField("Электронная почта", SearchTypeSpec.String)]
        public abstract string Email { get; set; }

        /// <summary>
        /// agency/sub-agency id
        /// </summary>
        [MapField("agency_id")]
        public abstract int AgencyId { get; set; }

        [Association(ThisKey = "AgencyId", OtherKey = "Id", CanBeNull = false)]
        public Agency Agency { get; set; }

        /// <summary>
        /// Должность сотрудника
        /// </summary>
        [MapField("position")]
        [SearchField("Должность", SearchTypeSpec.String)]
        public abstract string Position { get; set; }

        public override string ToString()
        {
            return GetPersonShortName();
        }

        /// <summary>
        /// Возвращает форматированную строку данных: "{Фамилия} {Инициал имени}. {Инициал Отчества}."
        /// </summary>
        /// <returns></returns>
        public string GetPersonShortName()
        {
            string surname = Surname;
            string nameLiter = string.IsNullOrEmpty(Name) ? string.Empty : Name.First().ToString();
            string patronymicLiter = string.IsNullOrEmpty(Name) ? string.Empty : Patronymic.First().ToString();
            if (!string.IsNullOrEmpty(surname) &&
                !string.IsNullOrEmpty(nameLiter) &&
                !string.IsNullOrEmpty(patronymicLiter))
            {
                return string.Format("{0} {1}.{2}.", surname, nameLiter, patronymicLiter);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
