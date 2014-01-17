using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PostGrad.Core.DomainModel
{
    [TableName("gu.u_user")]
    public abstract class DbUser : IdentityDomainObject<DbUser>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu.u_user_seq")]
        [MapField("user_id")]
        public abstract override int Id { get; set; }

        /// <summary>
        /// логин
        /// </summary>
        [MapField("user_name")]
        public abstract string Name { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [MapField("user_text")]
        public abstract string UserText { get; set; }

        /// <summary>
        /// должность
        /// </summary>
        [MapField("appoint_text")]
        public abstract string AppointText { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
