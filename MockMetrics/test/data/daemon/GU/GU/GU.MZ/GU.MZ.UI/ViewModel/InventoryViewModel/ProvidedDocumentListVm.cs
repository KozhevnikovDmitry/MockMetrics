using System;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InventoryViewModel
{
    public class ProvidedDocumentListVm : SmartListVm<ProvidedDocument>
    {
        private DocumentInventory _inventory;

        public ProvidedDocumentListVm(DocumentInventory inventory)
        {
            _inventory = inventory;
        }

        protected override void SetListOptions()
        {
            base.SetListOptions(); 
            Title = "Предоставленные документы";
            CanAddItems = true;
            CanRemoveItems = true;
        }

        protected override void AddItem()
        {
            try
            {
                var providedDocument = ProvidedDocument.CreateInstance();
                providedDocument.Name = "Новый документ";
                providedDocument.Quantity = 1;
                providedDocument.DocumentInventory = _inventory;
                Items.Add(providedDocument);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }
    }
}