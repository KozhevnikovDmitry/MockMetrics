using System;
using System.Collections.Generic;
using System.Linq;
using GU.BL;
using GU.DataModel;
using GU.Trud.BL;
using GU.Trud.DataModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.Trud.UI.ViewModel.Search.SearchTemplateViewModel
{
    public class TaskExportSearchTemplateVM : NotificationObject
    {
        public TaskExportSearchTemplateVM(TaskExport taskExport)
        {
            TaskExport = taskExport;
            ExportTypeList = TrudFacade.GetDictionaryManager().GetDictionary<ExportType>();
            ExportTypeId = ExportTypeList.First().Id;
        }

        public TaskExport TaskExport { get; set; }

        #region Binding Properties

        public string Filename
        {
            get
            {
                return TaskExport.Filename;
            }
            set
            {
                if (TaskExport.Filename != value)
                {
                    TaskExport.Filename = value;
                    RaisePropertyChanged(() => Filename);
                }
            }
        }

        public DateTime? Stamp
        {
            get
            {
                return TaskExport.Stamp;
            }
            set
            {
                if (TaskExport.Stamp != value)
                {
                    TaskExport.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public int ExportTypeId
        {
            get
            {
                return TaskExport.ExportTypeId;
            }
            set
            {
                if (TaskExport.ExportTypeId != value)
                {
                    TaskExport.ExportTypeId = value;
                    RaisePropertyChanged(() => ExportTypeId);
                }
            }
        }
        
        public List<ExportType> ExportTypeList { get; set; }

        #endregion
    }
}
