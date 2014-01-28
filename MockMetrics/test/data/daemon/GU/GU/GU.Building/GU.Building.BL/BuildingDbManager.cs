using System.Collections.Generic;
using System.Linq;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using GU.Building.DataModel;

namespace GU.Building.BL
{
    public class BuildingDbManager : DomainDbManager
    {
        public BuildingDbManager()
            : this(DefaultConfiguration)
        { }

        public BuildingDbManager(string configurationString)
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

        #region Building model

        public IQueryable<CommonData> CommonData
        {
            get { return GetDomainTable<CommonData>(); }
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
