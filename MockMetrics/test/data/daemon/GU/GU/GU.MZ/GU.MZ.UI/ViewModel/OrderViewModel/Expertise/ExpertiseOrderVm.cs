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

namespace GU.MZ.UI.ViewModel.OrderViewModel.Expertise
{
    public class ExpertiseOrderVm : SmartEditableVm<ExpertiseOrder>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly ExpertiseOrderReport _orderReport;

        public ExpertiseOrderVm(IDialogUiFactory uiFactory, 
                                ExpertiseOrderReport orderReport,
                                ISmartValidateableVm<ExpertiseOrder> expertiseOrderDataVm,
                                ISmartListVm<ExpertiseOrderAgree> expertiseOrderAgreeListVm,
                                ExpertiseHolderAddressListVm expertiseHolderAddressListVm) 
        {
            ExpertiseOrderDataVm = expertiseOrderDataVm;
            ExpertiseOrderAgreeListVm = expertiseOrderAgreeListVm;
            ExpertiseHolderAddressListVm = expertiseHolderAddressListVm;
            _uiFactory = uiFactory;
            _orderReport = orderReport;
            PrintCommand = new DelegateCommand(Print);
        }

        protected override void Rebuild()
        {
            ExpertiseOrderDataVm.Initialize(Entity);
            ExpertiseOrderAgreeListVm.Initialize(Entity.ExpertiseOrderAgreeList);
            ExpertiseHolderAddressListVm.ExpertiseOrder = Entity;
            ExpertiseHolderAddressListVm.Initialize(Entity.ExpertiseHolderAddressList);
        }

        protected override void Save()
        {
            try
            {
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    ExpertiseOrderDataVm.RaiseIsValidChanged();
                    ExpertiseOrderAgreeListVm.RaiseIsValidChanged();
                    ExpertiseHolderAddressListVm.RaiseIsValidChanged();
                    return;
                }

                var step = Entity.FileScenarioStep;
                base.Save();
                Entity.FileScenarioStep = step;
                Entity.FileScenarioStep.ExpertiseOrder = Entity;
                Entity.FileScenarioStep.AcceptMemberChanges(Util.GetPropertyName(() => Entity.FileScenarioStep.ExpertiseOrder));
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

        public ISmartValidateableVm<ExpertiseOrder> ExpertiseOrderDataVm { get; private set; }

        public ISmartListVm<ExpertiseOrderAgree> ExpertiseOrderAgreeListVm { get; private set; }

        public ExpertiseHolderAddressListVm ExpertiseHolderAddressListVm { get; private set; } 
        

        #endregion

        #region Binding Commands

        public DelegateCommand PrintCommand { get; set; }

        private void Print()
        {
            try
            {
                _orderReport.ExpertiseOrderId = Entity.Id;

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
