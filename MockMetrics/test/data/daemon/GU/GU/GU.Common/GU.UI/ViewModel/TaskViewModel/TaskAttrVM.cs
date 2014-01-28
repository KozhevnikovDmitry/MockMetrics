using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using GU.DataModel;

namespace GU.UI.ViewModel.TaskViewModel
{
    public class TaskAttrVM : NotificationObject
    {
        private TaskAttr _taskAttr;

        public TaskAttrVM(TaskAttr taskAttr)
        {
            _taskAttr = taskAttr;
            StringValue = GetAttrValue();
        }

        private string GetAttrValue()
        {
            if (_taskAttr.AttrValue.GetAttrValue() != null)
            {
                switch (_taskAttr.AttrValue.AttrDataType)
                {
                    case AttrDataType.String:
                        {
                            return _taskAttr.AttrValue.GetAttrValue().ToString();
                        }
                    case AttrDataType.Number:
                        {
                            return _taskAttr.AttrValue.GetAttrValue().ToString();
                        }
                    case AttrDataType.Boolean:
                        {
                            return ((bool)_taskAttr.AttrValue.GetAttrValue()) ? "Истина" : "Ложь";
                        }
                    case AttrDataType.Date:
                        {
                            return ((DateTime)_taskAttr.AttrValue.GetAttrValue()).ToLongDateString();
                        }
                    case AttrDataType.File:
                        {
                            return string.Format("{0} ({1}), {2} b", _taskAttr.AttrValue.BlobName, _taskAttr.AttrValue.BlobType, _taskAttr.AttrValue.BlobSize);
                        }
                    default:
                        {
                            return "Значение неизвестного типа";
                        }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #region Binding Properties

        public string Name
        {
            get
            {
                return _taskAttr.AttrSpec.Name;
            }
        }

        public AttrSpec AttrSpec 
        {
            get
            {
                return _taskAttr.AttrSpec;
            }
        }

        public string StringValue { get; private set; }

        #endregion        
    }
}
