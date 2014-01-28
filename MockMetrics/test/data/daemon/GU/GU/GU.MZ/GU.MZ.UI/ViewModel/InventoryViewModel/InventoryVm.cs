using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.InventoryViewModel
{
    public class InventoryVm : SmartEditableVm<DocumentInventory>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly InventoryReport _inventoryReport;

        public InventoryVm(IDialogUiFactory uiFactory, 
                           InventoryReport inventoryReport,
                           ISmartValidateableVm<DocumentInventory> inventoryDataVm, 
                           ISmartListVm<ProvidedDocument> providedDocumentListVm)
        {
            InventoryDataVm = inventoryDataVm;
            ProvidedDocumentListVm = providedDocumentListVm;
            _uiFactory = uiFactory;
            _inventoryReport = inventoryReport;
            PrintCommand = new DelegateCommand(Print);
        }

        public override void Initialize(DocumentInventory entity, IEditableFacade editableFacade, bool isEditable = false)
        {
            base.Initialize(entity, editableFacade, isEditable);
            Entity.ServiceId = entity.ServiceId;
        }

        protected override void Rebuild()
        {
            InventoryDataVm.Initialize(Entity);
            ProvidedDocumentListVm.Initialize(Entity.ProvidedDocumentList);
        }

        protected override void Save()
        {
            try
            {
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    InventoryDataVm.RaiseIsValidChanged();
                    ProvidedDocumentListVm.RaiseIsValidChanged();
                    return;
                }

                var serviceId = Entity.ServiceId;
                base.Save();
                Entity.ServiceId = serviceId;

            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #region Binding Properties
        
        public ISmartValidateableVm<DocumentInventory> InventoryDataVm { get; private set; }

        public ISmartListVm<ProvidedDocument> ProvidedDocumentListVm { get; private set; }

        #endregion

        #region Binding Commands

        public DelegateCommand PrintCommand { get; set; }

        private void Print()
        {
            try
            {
                _inventoryReport.InventoryId = Entity.Id;

                bool isDesigner = false;
#if DEBUG
                isDesigner = true;
#endif
                AvalonInteractor.RaiseOpenReportDockable(Entity.ToString(), _inventoryReport, isDesigner);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}
