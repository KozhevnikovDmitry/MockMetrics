using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Standart
{
    public class StandartOrderVm : SmartEditableVm<StandartOrder>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly StandartOrderReport _orderReport;
        private ISmartValidateableVm<StandartOrder> _standartOrderDataVm;

        public StandartOrderVm(IDialogUiFactory uiFactory,
                               StandartOrderReport orderReport,
                               ISmartValidateableVm<StandartOrder> standartOrderDataVm,
                               StandartOrderDetailListVm standartOrderDetailListVm) 
        {
            StandartOrderDataVm = standartOrderDataVm;
            StandartOrderDetailListVm = standartOrderDetailListVm;
            _uiFactory = uiFactory;
            _orderReport = orderReport;
            PrintCommand = new DelegateCommand(Print);
            
        }

        protected override void Rebuild()
        {
            _standartOrderDataVm.Initialize(Entity);
            RaisePropertyChanged(() => StandartOrderDataVm);
            StandartOrderDataVm.RaiseAllPropertyChanged();

            StandartOrderDetailListVm.StandartOrder = Entity;
            StandartOrderDetailListVm.Initialize(Entity.DetailList);
            RaisePropertyChanged(() => StandartOrderDetailListVm);
            StandartOrderDetailListVm.RaiseItemsValidatingPropertyChanged();
        }

        protected override void Save()
        {
            try
            {
                var index = Entity.FileScenarioStep.StandartOrderList.IndexOf(Entity);
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResult.Errors), "Ошибочно заполненные поля");
                    StandartOrderDataVm.RaiseIsValidChanged();
                    StandartOrderDetailListVm.RaiseIsValidChanged();
                    return;
                }

                var step = Entity.FileScenarioStep;
                base.Save();
                Entity.FileScenarioStep = step;
                Entity.FileScenarioStep.StandartOrderList.RemoveAt(index);
                Entity.FileScenarioStep.StandartOrderList.Insert(index, Entity);
                Entity.FileScenarioStep.StandartOrderList.AcceptChanges();
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

        public ISmartValidateableVm<StandartOrder> StandartOrderDataVm
        {
            get { return _standartOrderDataVm.Entity != null? _standartOrderDataVm: null; }
            private set { _standartOrderDataVm = value; }
        }

        public StandartOrderDetailListVm StandartOrderDetailListVm { get; private set; }

        #endregion

        #region Binding Commands
        
        public DelegateCommand PrintCommand { get; set; }

        private void Print()
        {
            try
            {
                _orderReport.StandartOrder = Entity;

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
