using System;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using Common.DA;
using Common.DA.Attributes;
using Common.DA.Interface;
using Common.Types;

namespace GU.MZ.DataModel.Requisites
{
    [TableName("gumz.individual_requisites")]
    [SearchClass("Физическое лицо")]
    public abstract class IndRequisites : IdentityDomainObject<IndRequisites>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.individual_requisites_seq")]
        [MapField("individual_requisites_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        [SearchField("Имя", SearchTypeSpec.String)]
        public abstract string Name { get; set; }

        [MapField("surname")]
        [SearchField("Фамилия", SearchTypeSpec.String)]
        public abstract string Surname { get; set; }

        [MapField("patronymic")]
        [SearchField("Отчество", SearchTypeSpec.String)]
        public abstract string Patronymic { get; set; }

        [MapField("serial_num")]
        [SearchField("Серия документа", SearchTypeSpec.String)]
        public abstract string Serial { get; set; }

        [MapField("num")]
        [SearchField("Номер документа", SearchTypeSpec.String)]
        public abstract string Number { get; set; }

        [MapField("stamp")]
        [SearchField("Дата выдачи", SearchTypeSpec.Date)]
        public abstract DateTime Stamp { get; set; }

        [MapField("organization")]
        [SearchField("Выдавшая организация", SearchTypeSpec.String)]
        public abstract string Organization { get; set; }

        [MapField("note")]
        [SearchField("Примечание", SearchTypeSpec.String)]
        public abstract string Note { get; set; }

        #region Proxy Properties

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string ShortName
        {
            get
            {
                return string.Format("{0} {1}.{2}.", Surname, Name.First(), Patronymic.First());
            }
        }

        [MapIgnore]
        [CloneIgnore]
        [NoInstance]
        public virtual string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", Surname, Name, Patronymic);
            }
        }

        #endregion
    }
}