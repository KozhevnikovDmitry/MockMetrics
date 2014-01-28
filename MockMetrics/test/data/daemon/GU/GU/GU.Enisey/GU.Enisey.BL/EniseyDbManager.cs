using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.DataModel;
using GU.Enisey.DataModel;

namespace GU.Enisey.BL
{
    public class EniseyDbManager : DomainDbManager
    {
        public EniseyDbManager()
            : this(DefaultConfiguration)
        {
        }

        public EniseyDbManager(string configurationString)
            : base(configurationString)
        {
            foreach (var p in GU.BL.GuDbManager.FilterPredicates)
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

        #region Model

        public IQueryable<CommonData> CommonData
        {
            get { return GetDomainTable<CommonData>(); }
        }

        public IQueryable<TaskSend> TaskSend
        {
            get { return GetDomainTable<TaskSend>(); }
        }

        public IQueryable<TaskSendDetail> TaskSendDetail
        {
            get { return GetDomainTable<TaskSendDetail>(); }
        }

        public IQueryable<TaskStateSend> TaskStateSend
        {
            get { return GetDomainTable<TaskStateSend>(); }
        }

        public IQueryable<TaskStateSendDetail> TaskStateSendDetail
        {
            get { return GetDomainTable<TaskStateSendDetail>(); }
        }

        public IQueryable<TaskReceive> TaskReceive
        {
            get { return GetDomainTable<TaskReceive>(); }
        }

        #endregion
    }
}
