using System;
using System.Linq;

using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;
using GU.DataModel;
using GU.DataModel.Inquiry;

namespace GU.BL.DataMapping
{
    public class InquiryDataMapper : AbstractDataMapper<Inquiry>
    {
        public InquiryDataMapper(IDomainDataMapper<Task> taskDataMapper,
                                 IDomainDataMapper<Content> contentDataMapper,
                                 IDictionaryManager _dictionaryManager,
                                 IDomainContext domainContext)
            : base(domainContext)
        {
            this._contentDataMapper = contentDataMapper;
            this._taskDataMapper = taskDataMapper;
            this._dictionaryManager = _dictionaryManager;
        }

        private readonly IDomainDataMapper<Content> _contentDataMapper;
        private readonly IDomainDataMapper<Task> _taskDataMapper;

        private readonly IDictionaryManager _dictionaryManager;

        protected override Inquiry RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Inquiry>(id);
            
            obj.InquiryType = _dictionaryManager.GetDictionaryItem<InquiryType>(obj.InquiryTypeId);
            
            // история статусов
            var stList = dbManager
                .GetDomainTable<InquiryStatus>()
                .Where(t => t.InquiryId == obj.Id)
                .Select(t => t.Id)
                .ToList()
                .Select(t => dbManager.RetrieveDomainObject<InquiryStatus>(t))
                .ToList();
            obj.StatusList = new EditableList<InquiryStatus>(stList);

            foreach (var s in stList)
            {
                s.User = dbManager.RetrieveDomainObject<DbUser>(s.UserId);
            }

            // загрузка данных запроса
            if (obj.RequestContentId.HasValue)
                obj.RequestContent = this._contentDataMapper.Retrieve(obj.RequestContentId.Value, dbManager);

            // загрузка данных ответа
            if (obj.ResponseContentId.HasValue)
                obj.ResponseContent = this._contentDataMapper.Retrieve(obj.ResponseContentId.Value, dbManager);

            // загрузка связанного таска в минимальном виде
            if (obj.TaskId.HasValue)
            {
                obj.Task = dbManager.RetrieveDomainObject<Task>(obj.TaskId.Value);
                _taskDataMapper.FillAssociations(obj.Task, dbManager);
            }

            return obj;
        }

        protected override Inquiry SaveOperation(Inquiry obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            tmp.InquiryTypeId = tmp.InquiryType.Id;

            if (tmp.PersistentState == PersistentState.New)
            {
                tmp.CreateDate = DateTime.Now;
            }

            if (tmp.RequestContent != null)
            {
                tmp.RequestContent = this._contentDataMapper.Save(tmp.RequestContent, dbManager, forceSave);
                tmp.RequestContentId = tmp.RequestContent.Id;
            }

            if (tmp.ResponseContent != null)
            {
                tmp.ResponseContent = this._contentDataMapper.Save(tmp.ResponseContent, dbManager, forceSave);
                tmp.ResponseContentId = tmp.ResponseContent.Id;
            }

            if (tmp.Task != null)
            {
                tmp.TaskId = tmp.Task.Id;
            }

            dbManager.SaveDomainObject(tmp);

            foreach (var ts in tmp.StatusList)
            {
                ts.InquiryId = tmp.Id;
                dbManager.SaveDomainObject(ts);
            }

            return tmp;
        }

        protected override void DeleteOperation(Inquiry obj, IDomainDbManager dbManager)
        {
            var tmp = obj.Clone();

            tmp.MarkDeleted();

            if (tmp.RequestContent != null)
                this._contentDataMapper.Delete(tmp.RequestContent, dbManager);
            
            if (tmp.ResponseContent != null)
                this._contentDataMapper.Delete(tmp.ResponseContent, dbManager);

            dbManager.SaveDomainObject(tmp);
        }

        protected override void FillAssociationsOperation(Inquiry obj, IDomainDbManager dbManager)
        {
            obj.InquiryType = _dictionaryManager.GetDictionaryItem<InquiryType>(obj.InquiryTypeId);
        }

    }
}
