using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.View.InspectionView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка экспертов, привлечённых к проверке
    /// </summary>
    public class InspectionExpertListVm : SmartListVm<InspectionExpert>
    {
        public ScenarioStep ScenarioStep { get; set; }
        
        private SupervisionFacade _superviser;

        private IDialogUiFactory _uiFactory;

        public InspectionExpertListVm(SupervisionFacade superviser, IDialogUiFactory uiFactory)
        {
            _superviser = superviser;
            _uiFactory = uiFactory;
        }

        /// <summary>
        /// Добавляет новый результат проверки в список.
        /// </summary>
        protected override void AddItem()
        {
            try
            {
                var experts = _superviser.GetAvailableExperts(ScenarioStep);
                if (!experts.Any())
                {
                    NoticeUser.ShowInformation("Больше экспертов к проверке привлечь нельзя");
                    return;
                }

                var vm = new AddInspectionExpertVm(experts);
                if (_uiFactory.ShowDialogView(
                    new AddInspectionExpertView(), vm, "Привлечь эксперта к проверке"))
                {
                    var inspectionExpert = InspectionExpert.CreateInstance();
                    inspectionExpert.Expert = vm.Expert;
                    inspectionExpert.ExpertId = vm.Expert.Id;
                    Items.Add(inspectionExpert);
                }
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
