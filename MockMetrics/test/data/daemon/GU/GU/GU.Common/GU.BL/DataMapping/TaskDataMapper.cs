using System;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

using GU.DataModel;

namespace GU.BL.DataMapping
{
    public class TaskDataMapper : AbstractDataMapper<Task>
    {
        public TaskDataMapper(IDomainDataMapper<Content> contentDataMapper,
                              IDictionaryManager _dictionaryManager,
                              IDomainContext domainContext)
            : base(domainContext)
        {
            this._contentDataMapper = contentDataMapper;
            this._dictionaryManager = _dictionaryManager;
        }
        private readonly IDomainDataMapper<Content> _contentDataMapper;

        private readonly IDictionaryManager _dictionaryManager;

        protected override Task RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Task>(id);
            FillTaskCommonData(obj, dbManager);
            obj.Service = _dictionaryManager.GetDictionaryItem<Service>(obj.ServiceId);
            obj.Agency = _dictionaryManager.GetDictionaryItem<Agency>(obj.AgencyId);

            // загрузили список статусов (TaskStatus)
            var stList =
                (from ts in dbManager.GetDomainTable<TaskStatus>() where ts.TaskId == obj.Id select ts.Id).ToList().
                    Select(val => dbManager.RetrieveDomainObject<TaskStatus>(val)).ToList();
            obj.StatusList = new EditableList<TaskStatus>(stList);

            // TODO: переложить в маппер TaskStatus'ов
            foreach (var s in stList)
            {
                s.User = dbManager.RetrieveDomainObject<DbUser>(s.UserId);
            }

            // загрузка данных заявления (новый формат)
            if (obj.ContentId.HasValue)
            {
                obj.Content = this._contentDataMapper.Retrieve(obj.ContentId.Value, dbManager);
            }

            return obj;
        }

        protected override Task SaveOperation(Task obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            if (tmp.PersistentState == PersistentState.New)
            {
                tmp.CreateDate = DateTime.Now;
            }

            if (tmp.Content != null)
            {
                tmp.Content = this._contentDataMapper.Save(tmp.Content, dbManager, forceSave);
                tmp.ContentId = tmp.Content.Id;
            }

            dbManager.SaveDomainObject(tmp);

            foreach (var ts in tmp.StatusList)
            {
                ts.TaskId = tmp.Id;
                dbManager.SaveDomainObject(ts);
            }
            
            return tmp;
        }

        protected override void DeleteOperation(Task obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            tmp.MarkDeleted();

            if (tmp.Content != null)
                this._contentDataMapper.Delete(tmp.Content, dbManager);

            dbManager.SaveDomainObject(tmp);
        }

        protected override void FillAssociationsOperation(Task obj, IDomainDbManager dbManager)
        {
            // т.к. для залеп верхним вызовом будет linq-запрос по Task'ам (т.е. не будет RetrieveDomainObject),
            // а у тех, кто вызывает (например, StatementDataMapper) будет другой DbManager - то получается,
            // что CommonData не будет заполнена в DomainDbManager, поэтому грузим здесь
            // то же касается и Retrieve
            FillTaskCommonData(obj, dbManager);

            var stList =
                (from ts in dbManager.GetDomainTable<TaskStatus>() where ts.TaskId == obj.Id select ts).ToList();
            obj.StatusList = new EditableList<TaskStatus>(stList);

            obj.Service = _dictionaryManager.GetDictionaryItem<Service>(obj.ServiceId);
            obj.Agency = _dictionaryManager.GetDictionaryItem<Agency>(obj.AgencyId);
        }

        protected void FillTaskCommonData(Task obj, IDomainDbManager dbManager)
        {
            var cd = (from c in dbManager.GetDomainTable<CommonData>()
                      where c.KeyValue == obj.GetKeyValue() && c.Entity == obj.GetType().Name
                      select c).OrderByDescending(x => x.Stamp).Take(1).ToList().FirstOrNull();
            obj.CommonData = cd;
        }
    }
}
