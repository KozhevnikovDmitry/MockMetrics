using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Маппер сущностей Сотрудник 
    /// </summary>
    public class EmployeeDataMapper : AbstractDataMapper<Employee>
    {
        /// <summary>
        /// Маппер томов
        /// </summary>
        public IDomainDataMapper<DossierFile> DossierFileMapper { get; set; }

        /// <summary>
        /// Маппер пользователей
        /// </summary>
        private readonly IDomainDataMapper<DbUser> _dbUserDataMapper;

        /// <summary>
        /// Маппер сущностей Сотрудник 
        /// </summary>
        /// <param name="dossierFileMapper">Маппер томов</param>
        /// <param name="dbUserDataMapper"> </param>
        /// <param name="domainContext">Доменный контекст</param>
        public EmployeeDataMapper(IDomainDataMapper<DbUser> dbUserDataMapper,IDomainContext domainContext)
            : base(domainContext)
        {
            _dbUserDataMapper = dbUserDataMapper;
        }

        /// <summary>
        /// Получает сущность Сотрудник из базы данных по Id
        /// </summary>
        /// <param name="id">Id сотрудника</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <returns>Сотрудник из юазы</returns>
        protected override Employee RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var employee = dbManager.RetrieveDomainObject<Employee>(id);

            employee.DbUser = _dbUserDataMapper.Retrieve(employee.DbUserId, dbManager);
            
            var fileIds = dbManager.GetDomainTable<DossierFile>()
                                   .Where(d => d.EmployeeId == employee.Id &&
                                          d.CurrentStatus != DossierFileStatus.Closed)
                                   .Select(d => d.Id)
                                   .ToList();

            employee.DossierFileList = new EditableList<DossierFile>(fileIds.Select(d => dbManager.RetrieveDomainObject<DossierFile>(d)).ToList());
            
            return employee;
        }

        /// <summary>
        /// Сохраняет данные сотрудника в базу, возвращает сохранённого сотрудника.
        /// </summary>
        /// <param name="obj">Сотрудник для сохранения</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <param name="forceSave">Флаг бузесловного сохранения</param>
        /// <returns>Сохранённый сотрудник</returns>
        protected override Employee SaveOperation(Employee obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            tmp.DbUser = _dbUserDataMapper.Save(tmp.DbUser, dbManager);

            tmp.DbUserId = tmp.DbUser.Id;

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        /// <summary>
        /// Заполняет отображаемые данные сотрудника.
        /// </summary>
        /// <param name="obj">Объект сотрудник</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        protected override void FillAssociationsOperation(Employee obj, IDomainDbManager dbManager)
        {
            obj.DbUser = dbManager.RetrieveDomainObject<DbUser>(obj.DbUserId);
        }
    }
}
