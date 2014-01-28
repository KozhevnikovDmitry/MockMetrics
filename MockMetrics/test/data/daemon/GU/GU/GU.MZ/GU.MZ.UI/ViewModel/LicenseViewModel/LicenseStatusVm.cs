using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    public class LicenseStatusVm : SmartValidateableVm<LicenseStatus>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseStatusVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(LicenseStatus entity)
        {
            base.Initialize(entity);
            StatusTypes = _dictionaryManager.GetEnumDictionary<LicenseStatusType>().ToDictionary(pair => (LicenseStatusType)pair.Key, pair => pair.Value);
        }

        #region Binding Properties

        public DateTime Stamp
        {
            get { return Entity.Stamp; }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public LicenseStatusType LicenseStatusType
        {
            get { return Entity.LicenseStatusType; }
            set
            {
                if (Entity.LicenseStatusType != value)
                {
                    Entity.LicenseStatusType = value;
                    RaisePropertyChanged(() => LicenseStatusType);
                }
            }
        }

        public Dictionary<LicenseStatusType, string> StatusTypes { get; set; }

        public string Note
        {
            get { return Entity.Note; }
            set
            {
                if (Entity.Note != value)
                {
                    Entity.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => LicenseStatusType);
            RaisePropertyChanged(() => Note);
        }
    }
}
