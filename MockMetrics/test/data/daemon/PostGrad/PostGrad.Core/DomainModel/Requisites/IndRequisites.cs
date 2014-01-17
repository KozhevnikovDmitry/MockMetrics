using System;
using System.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace PostGrad.Core.DomainModel.Requisites
{
    [TableName("gumz.individual_requisites")]
    public abstract class IndRequisites : IdentityDomainObject<IndRequisites>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gumz.individual_requisites_seq")]
        [MapField("individual_requisites_id")]
        public abstract override int Id { get; set; }

        [MapField("name")]
        public abstract string Name { get; set; }

        [MapField("surname")]
        public abstract string Surname { get; set; }

        [MapField("patronymic")]
        public abstract string Patronymic { get; set; }

        [MapField("serial_num")]
        public abstract string Serial { get; set; }

        [MapField("num")]
        public abstract string Number { get; set; }

        [MapField("stamp")]
        public abstract DateTime Stamp { get; set; }

        [MapField("organization")]
        public abstract string Organization { get; set; }

        [MapField("note")]
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