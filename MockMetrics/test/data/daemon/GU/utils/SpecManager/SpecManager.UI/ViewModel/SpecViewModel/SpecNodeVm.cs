using System.Collections.Generic;

using SpecManager.BL.Model;

using SpecManager.BL;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class SpecNodeVm : SpecVmBase
    {
        private readonly SpecNode _specNode;

        public SpecNodeVm(SpecNode specItem)
        {
            _specNode = specItem;
            AttrDataTypeList = AttrDataType.String.ToDictionary<AttrDataType>();
            if (_specNode.SpecNodeType == BL.Model.SpecNodeType.Simple 
                && _specNode.AttrDataType == null)
            {
                AttrDataType = AttrDataType.String;
            }
            this.ResetSpecNode();
        }

        #region Binding Properties

        #region Common

        public string Name
        {
            get
            {
                return _specNode.Name;
            }
            set
            {
                if (_specNode.Name != value)
                {
                    _specNode.Name = value;
                    this.RaisePropertyChanged(() => this.Name);
                    this.OnNameChanged(_specNode.Name);
                }
            }
        }

        public string Tag
        {
            get
            {
                return _specNode.Tag;
            }
            set
            {
                if (_specNode.Tag != value)
                {
                    _specNode.Tag = value;
                    this.RaisePropertyChanged(() => this.Tag);
                }
            }
        }

        public SpecNodeType SpecNodeType
        {
            get
            {
                return _specNode.SpecNodeType;
            }
        }

        public string SpecNodeTypeDesc
        {
            get
            {
                return _specNode.SpecNodeType.GetDescription();
            }
        }

        public int MinOccurs
        {
            get
            {
                return _specNode.MinOccurs;
            }
            set
            {
                if (_specNode.MinOccurs != value)
                {
                    _specNode.MinOccurs = value;
                    RaisePropertyChanged(() => MinOccurs);
                    OnMinOccursChanged(_specNode.MinOccurs);
                }
            }
        }

        public int? MaxOccurs
        {
            get
            {
                return _specNode.MaxOccurs;
            }
            set
            {
                if (_specNode.MaxOccurs != value)
                {
                    _specNode.MaxOccurs = value;
                    RaisePropertyChanged(() => MaxOccurs);
                    OnMaxOccursChanged(_specNode.MaxOccurs);
                }
            }
        }

        public string Note
        {
            get
            {
                return _specNode.Note;
            }
            set
            {
                if (_specNode.Note != value)
                {
                    _specNode.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }
        
        #endregion

        #region Specific

        public string FormatDescription
        {
            get
            {
                return _specNode.FormatDescription;
            }
            set
            {
                if (_specNode.FormatDescription != value)
                {
                    _specNode.FormatDescription = value;
                    RaisePropertyChanged(() => FormatDescription);
                }
            }
        }

        public string FormatRegexp
        {
            get
            {
                return _specNode.FormatRegexp;
            }
            set
            {
                if (_specNode.FormatRegexp != value)
                {
                    _specNode.FormatRegexp = value;
                    RaisePropertyChanged(() => FormatRegexp);
                }
            }
        }

        public string RefSpecUri
        {
            get
            {
                return _specNode.RefSpecUri;
            }
            set
            {
                if (_specNode.RefSpecUri != value)
                {
                    _specNode.RefSpecUri = value;
                    RaisePropertyChanged(() => RefSpecUri);
                }
            }
        }

        public int DictId
        {
            get
            {
                return _specNode.DictId ?? 0;
            }
            set
            {
                if (_specNode.DictId != value)
                {
                    _specNode.DictId = value;
                    RaisePropertyChanged(() => DictId);
                }
            }
        }

        public bool IsEditableDict
        {
            get
            {
                return _specNode.IsEditableDict ?? false;
            }
            set
            {
                if (_specNode.IsEditableDict != value)
                {
                    _specNode.IsEditableDict = value;
                    RaisePropertyChanged(() => IsEditableDict);
                }
            }
        }

        public AttrDataType AttrDataType
        {
            get
            {
                return _specNode.AttrDataType ?? AttrDataType.String;
            }
            set
            {
                if (_specNode.AttrDataType != value)
                {
                    _specNode.AttrDataType = value;
                    this.ResetSpecNode();
                    RaisePropertyChanged(() => AttrDataType);
                    OnAttrDataTypeChanged(_specNode.AttrDataType);
                }
            }
        }

        private void ResetSpecNode()
        {
            if (_specNode.AttrDataType.HasValue)
            {
                if (AttrDataType != AttrDataType.String)
                {
                    FormatDescription = string.Empty;
                    FormatRegexp = string.Empty;
                }

                if (AttrDataType == AttrDataType.List)
                {
                    _specNode.DictId = _specNode.DictId ?? 0;
                    _specNode.IsEditableDict = _specNode.IsEditableDict ?? false;
                }
                else
                {
                    _specNode.DictId = null;
                    _specNode.IsEditableDict = null;
                }
            }
        }

        public Dictionary<AttrDataType, string> AttrDataTypeList { get; private set; }

        #endregion

        #endregion
    }
}
