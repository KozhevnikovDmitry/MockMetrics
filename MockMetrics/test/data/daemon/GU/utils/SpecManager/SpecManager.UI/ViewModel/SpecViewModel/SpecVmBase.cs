using System;

using Microsoft.Practices.Prism.ViewModel;
using SpecManager.BL.Model;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public abstract class SpecVmBase : NotificationObject
    {
        public event Action<string> NameChanged;

        protected void OnNameChanged(string name)
        {
            Action<string> handler = this.NameChanged;
            if (handler != null)
            {
                handler(name);
            }
        }

        public event Action<int?> MinOccursChanged;

        protected void OnMinOccursChanged(int? minOccurs)
        {
            var handler = this.MinOccursChanged;
            if (handler != null)
            {
                handler(minOccurs);
            }
        }

        public event Action<int?> MaxOccursChanged;

        protected void OnMaxOccursChanged(int? maxOccurs)
        {
            var handler = this.MaxOccursChanged;
            if (handler != null)
            {
                handler(maxOccurs);
            }
        }

        public event Action<SpecNodeType?> SpecNodeTypeChanged;

        protected void OnSpecNodeTypeChanged(SpecNodeType? specNodeType)
        {
            var handler = this.SpecNodeTypeChanged;
            if (handler != null)
            {
                handler(specNodeType);
            }
        }

        public event Action<AttrDataType?> AttrDataTypeChanged;

        protected void OnAttrDataTypeChanged(AttrDataType? attrDataType)
        {
            var handler = this.AttrDataTypeChanged;
            if (handler != null)
            {
                handler(attrDataType);
            }
        }
    }
}