using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

namespace GU.Archive.DataModel
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Организация
    /// </summary>
    [TableName("gu_archive.organization")]
    [SearchClass("Организация")]
    public abstract class Organization : IdentityDomainObject<Organization>, IPersistentObject
    {
        /// <summary>
        /// Первичный ключ сущности
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gu_archive.organization_seq")]
        [MapField("organization_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        [MapField("full_name")]
        [SearchField("Полное наименование", SearchTypeSpec.String)]
        public abstract string FullName { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        [MapField("short_name")]
        [SearchField("Краткое наименование", SearchTypeSpec.String)]
        public abstract string ShortName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [MapField("inn")]
        [SearchField("ИНН", SearchTypeSpec.String)]
        public abstract string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [MapField("ogrn")]
        [SearchField("ОГРН", SearchTypeSpec.String)]
        public abstract string Ogrn { get; set; }

        /// <summary>
        /// Id связной сущности Адрес
        /// </summary>
        [MapField("address_id")]
        public abstract int AddressId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "AddressId", OtherKey = "Id", CanBeNull = false)]
        public abstract Address Address { get; set; }

        /// <summary>
        /// Фамилия и инициалы руководителя
        /// </summary>
        [MapField("head_name")]
        [SearchField("ФИО главы", SearchTypeSpec.String)]
        public abstract string HeadName { get; set; }

        ///<summary>
        /// phone
        ///</summary>
        [MapField("phone")]
        public abstract string Phone { get; set; }

        ///<summary>
        /// fax
        ///</summary>
        [MapField("fax")]
        public abstract string Fax { get; set; }

        ///<summary>
        /// email
        ///</summary>
        [MapField("email")]
        public abstract string Email { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}
