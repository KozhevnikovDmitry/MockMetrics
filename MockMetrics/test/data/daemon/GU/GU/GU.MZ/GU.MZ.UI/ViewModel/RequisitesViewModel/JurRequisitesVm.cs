using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.RequisitesViewModel
{
    public class JurRequisitesVm : SmartValidateableVm<JurRequisites>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public JurRequisitesVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(JurRequisites entity)
        {
            base.Initialize(entity);
            LegalFormList = _dictionaryManager.GetDictionary<LegalForm>();
        }

        #region Binding Properties

        public string FullName
        {
            get { return Entity.FullName; }
            set
            {
                if (Entity.FullName != value)
                {
                    Entity.FullName = value;
                    RaisePropertyChanged(() => FullName);
                }
            }
        }

        public string ShortName
        {
            get { return Entity.ShortName; }
            set
            {
                if (Entity.ShortName != value)
                {
                    Entity.ShortName = value;
                    RaisePropertyChanged(() => ShortName);
                }
            }
        }

        public string FirmName
        {
            get { return Entity.FirmName; }
            set
            {
                if (Entity.FirmName != value)
                {
                    Entity.FirmName = value;
                    RaisePropertyChanged(() => FirmName);
                }
            }
        }

        public string HeadName
        {
            get { return Entity.HeadName; }
            set
            {
                if (Entity.HeadName != value)
                {
                    Entity.HeadName = value;
                    RaisePropertyChanged(() => HeadName);
                }
            }
        }

        public string HeadPositionName
        {
            get { return Entity.HeadPositionName; }
            set
            {
                if (Entity.HeadPositionName != value)
                {
                    Entity.HeadPositionName = value;
                    RaisePropertyChanged(() => HeadPositionName);
                }
            }
        }

        public int LegalFormId
        {
            get
            {
                return Entity.LegalFormId;
            }
            set
            {
                if (Entity.LegalFormId != value)
                {
                    Entity.LegalFormId = value;
                    RaisePropertyChanged(() => LegalFormId);
                }
            }
        }

        public List<LegalForm> LegalFormList { get; set; }

        #endregion
    }
}
