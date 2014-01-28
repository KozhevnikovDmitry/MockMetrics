using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.Archive.DataModel;
using BLToolkit.EditableObjects;
using GU.BL;
using GU.DataModel;

namespace GU.Archive.BL.DataMapping
{
    public class PostDataMapper : AbstractDataMapper<Post>
    {
        public PostDataMapper(IDomainDataMapper<PostExecutor> postExecutorDataMapper, IDomainContext domainContext)
            : base(domainContext)
        {
            _postExecutorDataMapper = postExecutorDataMapper;
        }

        private readonly IDomainDataMapper<PostExecutor> _postExecutorDataMapper;

        protected override Post RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Post>(id);

            if (obj.OrganizationId != null)
                obj.Organization = dbManager.RetrieveDomainObject<Organization>(obj.OrganizationId);
            obj.Author = ArchiveFacade.GetDictionaryManager().GetDictionaryItem<Author>(obj.AuthorId);

            if (obj.TaskId != null)
                obj.Task = GuFacade.GetDataMapper<Task>().Retrieve(obj.TaskId);

            // загрузили список исполнителей корреспонденции (PostExecutor)
            var executors = (from e in dbManager.GetDomainTable<PostExecutor>()
                             where e.PostId == obj.Id
                             select e.Id).ToList()
                            .Select(val => _postExecutorDataMapper.Retrieve(val, dbManager)).ToList();
            obj.Executors = new EditableList<PostExecutor>(executors);

            // для каждого PostExecutor кладем ссылку на Post
            foreach (var exec in obj.Executors)
            {
                exec.Post = obj;
            }

            return obj;
        }

        protected override Post SaveOperation(Post obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            if (tmp.TaskId != null)
                GuFacade.GetDataMapper<Task>().Save(tmp.Task);

            dbManager.SaveDomainObject(tmp);
            
            foreach (var e in tmp.Executors)
            {
                e.PostId = tmp.Id;
                _postExecutorDataMapper.Save(e, dbManager);
            }

            return tmp;
        }

        protected override void FillAssociationsOperation(Post obj, IDomainDbManager dbManager)
        {
        }
    }
}
