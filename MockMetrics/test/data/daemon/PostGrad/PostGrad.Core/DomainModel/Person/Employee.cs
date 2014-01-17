using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using PostGrad.Core.DomainModel.Dossier;

namespace PostGrad.Core.DomainModel.Person
{
    /// <summary>
    /// Класс, предназначенный для хранения данных сущности Сотрудник отдела лицензирования
    /// </summary>
    [TableName("gumz.employee")]
    public abstract class Employee : IdentityDomainObject<Employee>, IPersistentObject
    {
        /// <summary>
        /// Класс, предназначенный для хранения данных сущности Сотрудник отдела лицензирования
        /// </summary>
        protected Employee()
        {
            this.DossierFileList = new EditableList<DossierFile>();
        }

        /// <summary>
        /// Первичный ключ сущности.
        /// </summary>
        [PrimaryKey, Identity]
        [SequenceName("gumz.employee_seq")]
        [MapField("employee_id")]
        public abstract override int Id { get; set; }
        
        /// <summary>
        /// Телефон сотрудника.
        /// </summary>
        [MapField("phone")]
        public abstract string Phone { get; set; }

        /// <summary>
        /// Электронная почта сотрудника.
        /// </summary>
        [MapField("email")]
        public abstract string Email { get; set; }

        /// <summary>
        /// Флаг указывающий на то, является ли сотрудник штатным.
        /// </summary>
        [MapField("staff")]
        public abstract bool IsStaff { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        [MapField("u_user_id")]
        public abstract int DbUserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "DbUserId", OtherKey = "Id", CanBeNull = false)]
        public abstract DbUser DbUser { get; set; }

        #region DbUser Proxy

        [MapIgnore]
        public virtual string Name
        {
            get
            {
                return DbUser != null ? DbUser.UserText : string.Empty;
            }
            set
            {
                if(DbUser != null)
                {
                    DbUser.UserText = value;
                }
            }
        }

        [MapIgnore]
        public virtual string Position
        {
            get
            {
                return DbUser != null ? DbUser.AppointText : string.Empty;
            }
            set
            {
                if (DbUser != null)
                {
                    DbUser.AppointText = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Id сущности сотрудник - начальник сотрудника
        /// </summary>
        [MapField("chief_id")]
        public abstract int? ChiefId { get; set; }

        /// <summary>
        /// Начальник сотрудника    
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "ChiefId", OtherKey = "Id", CanBeNull = true)]
        public abstract Employee Chief { get; set; }

        /// <summary>
        /// Список сотрудников находящихся в подчинении у данного сотрдуника
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "ChiefId", CanBeNull = true)]
        public abstract EditableList<Employee> SubordinateList { get; set; }

        /// <summary>
        /// Список томов, для которых сотрудник является отвественным исполнителем.
        /// </summary>
        [NoInstance]
        [Association(ThisKey = "Id", OtherKey = "MyPropertyId", CanBeNull = true)]
        public EditableList<DossierFile> DossierFileList { get; set; }

        public override string ToString()
        {
            return this.DbUser.UserText;
        }
    }
}
