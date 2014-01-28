using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.Archive.DataModel;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using CommonData = GU.Archive.DataModel.CommonData;

namespace GU.Archive.BL
{
    public class ArchiveDbManager : DomainDbManager
    {
        public ArchiveDbManager()
            : this(DefaultConfiguration)
        { }

        public ArchiveDbManager(string configurationString)
            : base(configurationString)
        {
            foreach (var p in GU.BL.GuDbManager.FilterPredicates)
                _filterPredicates.Add(p.Key, p.Value);

            _filterPredicates.Add(typeof(Post), _postFilterPredicate);
        }

        protected override void RetrieveCommonData(IPersistentObject obj)
        {
            var cd = (from c in this.GetTable<CommonData>()
                      where c.KeyValue == obj.GetKeyValue() && c.Entity == obj.GetType().Name
                      select c).OrderByDescending(x => x.Stamp).Take(1).ToList().FirstOrNull();

            if (cd != null)
            {
                cd.ConfigurationString = this.ConfigurationString;
            }
            obj.CommonData = cd;
        }

        private static readonly Expression<Func<Post, bool>> _postFilterPredicate = post => UserPolicy.VisibleAgencyIds(GuFacade.GetDbUser()).Contains(post.Task.Agency.Id);

        #region Archive model

        public IQueryable<CommonData> CommonData
        {
            get { return GetDomainTable<CommonData>(); }
        }

        public IQueryable<Post> Post
        {
            get { return GetDomainTable<Post>(); }
        }

        public IQueryable<PostExecutor> PostExecutor
        {
            get { return GetDomainTable<PostExecutor>(); }
        }

        #endregion

        #region Gu model

        public IQueryable<GU.DataModel.CommonData> GuCommonData
        {
            get { return GetDomainTable<GU.DataModel.CommonData>(); }
        }

        public IQueryable<GU.DataModel.Task> GuTask
        {
            get { return GetDomainTable<GU.DataModel.Task>(); }
        }

        #endregion

    }
}
