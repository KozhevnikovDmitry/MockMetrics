using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InventoryViewModel
{
    public class ProvidedDocumentListItemVm : SmartListItemVm<ProvidedDocument>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public ProvidedDocumentListItemVm(IDictionaryManager dictionaryManager) 
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(ProvidedDocument entity)
        {
            base.Initialize(entity);
            ExpertedDocumentList = _dictionaryManager.GetDictionary<ExpertedDocument>()
                                                     .Where(t => t.ServiceId == Entity.DocumentInventory.ServiceId)
                                                     .ToList();
        }

        #region Binding Properties

        public string Name
        {
            get
            {
                return Entity.Name;
            }
            set
            {
                if (Entity.Name != value)
                {
                    Entity.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public int Quantity
        {
            get
            {
                return Entity.Quantity;
            }
            set
            {
                if (Entity.Quantity != value)
                {
                    Entity.Quantity = value;
                    RaisePropertyChanged(() => Quantity);
                }
            }
        }

        public List<ExpertedDocument> ExpertedDocumentList { get; set; }

        #endregion

    }
}