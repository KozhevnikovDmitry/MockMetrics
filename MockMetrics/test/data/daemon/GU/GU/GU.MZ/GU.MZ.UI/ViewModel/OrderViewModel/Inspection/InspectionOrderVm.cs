using System;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionOrderVm : SmartEditableVm<InspectionOrder>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly InspectionOrderReport _orderReport;

        public InspectionOrderVm(IDialogUiFactory uiFactory,
                                 InspectionOrderReport orderReport,
                                 ISmartValidateableVm<InspectionOrder> inspectionOrderDataVm,
                                 InspectionHolderAddressListVm inspectionHolderAddressListVm,
                                 ISmartListVm<InspectionOrderAgree> inspectionOrderAgreeListVm,
                                 ISmartListVm<InspectionOrderExpert> inspectionOrderExpertListVm) 
        {
            InspectionOrderDataVm = inspectionOrderDataVm;
            InspectionHolderAddressListVm = inspectionHolderAddressListVm;
            InspectionOrderAgreeListVm = inspectionOrderAgreeListVm;
            InspectionOrderExpertListVm = inspectionOrderExpertListVm;
            _uiFactory = uiFactory;
            _orderReport = orderReport;
            PrintCommand = new DelegateCommand(Print);
        }

        protected override void Rebuild()
        {
            InspectionOrderDataVm.Initialize(Entity);
            InspectionOrderExpertListVm.Initialize(Entity.InspectionOrderExpertList);
            InspectionOrderAgreeListVm.Initialize(Entity.InspectionOrderAgreeList);
            InspectionHolderAddressListVm.InspectionOrder = Entity;
            InspectionHolderAddressListVm.Initialize(Entity.InspectionHolderAddressList);
        }

        protected override void Save()
        {
            try
            {
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    InspectionOrderDataVm.RaiseIsValidChanged();
                    InspectionOrderAgreeListVm.RaiseIsValidChanged();
                    InspectionHolderAddressListVm.RaiseIsValidChanged();
                    return;
                }

                var step = Entity.FileScenarioStep;
                base.Save();
                Entity.FileScenarioStep = step;
                Entity.FileScenarioStep.InspectionOrder = Entity;
                Entity.FileScenarioStep.AcceptMemberChanges(Util.GetPropertyName(() => Entity.FileScenarioStep.InspectionOrder));
                Entity.FileScenarioStep.DossierFile.RaiseIsDirtyChanged();
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
        
        public ISmartValidateableVm<InspectionOrder> InspectionOrderDataVm { get; private set; }
        
        public ISmartListVm<InspectionOrderAgree> InspectionOrderAgreeListVm { get; private set; }

        public ISmartListVm<InspectionOrderExpert> InspectionOrderExpertListVm { get; private set; }

        public InspectionHolderAddressListVm InspectionHolderAddressListVm { get; private set; }

        #endregion

        #region Binding Commands

        public DelegateCommand PrintCommand { get; set; }

        private void Print()
        {
            try
            {
                _orderReport.InspectionOrderId = Entity.Id;

                bool isDesigner = false;
#if DEBUG
                isDesigner = true;
#endif
                AvalonInteractor.RaiseOpenReportDockable(Entity.ToString(), _orderReport, isDesigner);
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
