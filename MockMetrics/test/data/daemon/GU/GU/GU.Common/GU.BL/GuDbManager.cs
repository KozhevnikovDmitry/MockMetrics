using System;
using System.Collections.Generic;
using System.Linq;
using Common.DA;
using Common.DA.Interface;
using Common.Types;

using GU.BL.Policy;
using GU.DataModel;
using System.Linq.Expressions;

namespace GU.BL
{
    public class GuDbManager : DomainDbManager
    {
        public GuDbManager()
            : this(DefaultConfiguration)
        {
            
        }

        public GuDbManager(string configurationString)
            : base(configurationString)
        {
            foreach (var p in FilterPredicates)
                _filterPredicates.Add(p.Key, p.Value);
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

        #region Filter Predicates

        //TODO: нехорошо что логика лежит в DbManager, нужно как-то передавать предикаты извне
        //TODO: приходится распихивать общие предикаты по всем DbManager-ам, ибо неизвестно с каким менеджером придется работать
        private static readonly Expression<Func<Task, bool>> _taskFilterPredicate = task => UserPolicy.VisibleAgencyIds(GuFacade.GetDbUser()).Contains(task.Agency.Id);

        public static readonly Dictionary<Type, object> FilterPredicates = new Dictionary<Type, object>
            {
                {typeof (Task), _taskFilterPredicate}
            };
        
        #endregion

        #region Model

        public IQueryable<CommonData> CommonData
        {
            get { return GetDomainTable<CommonData>(); }
        }

        public IQueryable<Agency> Agency
        {
            get { return GetDomainTable<Agency>(); }
        }

        public IQueryable<Dict> Dict
        {
            get { return GetDomainTable<Dict>(); }
        }

        public IQueryable<DictDet> DictDet
        {
            get { return GetDomainTable<DictDet>(); }
        }

        public IQueryable<TaskStatus> TaskStatus
        {
            get { return GetDomainTable<TaskStatus>(); }
        }

        public IQueryable<Service> Service
        {
            get { return GetDomainTable<Service>(); }
        }

        public IQueryable<ServiceGroup> ServiceGroup
        {
            get { return GetDomainTable<ServiceGroup>(); }
        }

        public IQueryable<DbUser> DbUser
        {
            get { return GetDomainTable<DbUser>(); }
        }

        public IQueryable<DbUserRole> DbUserRole
        {
            get { return GetDomainTable<DbUserRole>(); }
        }

        public IQueryable<DbRole> DbRole
        {
            get { return GetDomainTable<DbRole>(); }
        }

        public IQueryable<DbRoleChild> DbRoleChild
        {
            get { return GetDomainTable<DbRoleChild>(); }
        }

        #endregion
    }
}
