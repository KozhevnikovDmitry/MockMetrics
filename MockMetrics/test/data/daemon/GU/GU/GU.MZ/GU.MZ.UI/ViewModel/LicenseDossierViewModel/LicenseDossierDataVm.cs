using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using Common.DA;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseDossierViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных лицензионного дела.
    /// </summary>
    public class LicenseDossierDataVm : SmartValidateableVm<LicenseDossier>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseDossierDataVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(LicenseDossier entity)
        {
            base.Initialize(entity);
            IsActiveList = new Dictionary<bool, string>();
            IsActiveList[true] = "Активное";
            IsActiveList[false] = "Архивное";

            IsNewDossier = Entity.PersistentState == PersistentState.New;
            LicensedActivityList = _dictionaryManager.GetDictionary<LicensedActivity>();
        }

        #region Binding Properties
        
        public bool IsNewDossier { get; private set; }

        public string RegNumber
        {
            get { return Entity.RegNumber; }
            set
            {
                if (Entity.RegNumber != value)
                {
                    Entity.RegNumber = value;
                    RaisePropertyChanged(() => RegNumber);
                }
            }
        }

        public DateTime CreateDate
        {
            get { return Entity.CreateDate; }
            set
            {
                if (Entity.CreateDate != value)
                {
                    Entity.CreateDate = value;
                    RaisePropertyChanged(() => CreateDate);
                }
            }
        }

        public bool IsActive
        {
            get { return Entity.IsActive; }
            set
            {
                if (Entity.IsActive != value)
                {
                    Entity.IsActive = value;
                    RaisePropertyChanged(() => IsActive);
                }
            }
        }

        public Dictionary<bool, string> IsActiveList { get; set; }

        public int LicensedActivityId
        {
            get { return Entity.LicensedActivityId; }
            set
            {
                if (Entity.LicensedActivityId != value)
                {
                    Entity.LicensedActivityId = value;
                    RaisePropertyChanged(() => LicensedActivityId);
                }
            }
        }

        public List<LicensedActivity> LicensedActivityList { get; set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => IsActive);
        }
    }
}
