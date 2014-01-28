using System.Linq;
using BLToolkit.EditableObjects;

using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.Trud.DataModel;

namespace GU.Trud.BL.DataMapping
{
    public class TaskExportDataMapper : AbstractDataMapper<TaskExport>
    {
        public TaskExportDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override TaskExport RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<TaskExport>(id);
            var taskExportDets =
                (from t in dbManager.GetDomainTable<TaskExportDet>() where t.TaskExportId == obj.Id select t).ToList();
            obj.TaskExportDets = new EditableList<TaskExportDet>(taskExportDets);
            obj.TaskExportDets.ForEach(t => t.TaskExport = obj);
            return obj;
        }

        protected override TaskExport SaveOperation(TaskExport obj, IDomainDbManager dbManager, bool forceSave = false)
        {
                var tmp = obj.Clone();
                dbManager.SaveDomainObject(tmp);
                if(tmp.TaskExportDets != null)
                {
                    foreach (var taskExportDet in tmp.TaskExportDets)
                    {
                        taskExportDet.TaskExportId = tmp.Id;
                        dbManager.SaveDomainObject(taskExportDet);
                    }
                }
                return tmp;
        }

        protected override void FillAssociationsOperation(TaskExport obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
