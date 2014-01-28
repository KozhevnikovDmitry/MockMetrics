using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using GU.DataModel;
using GU.MZ.UI.View.EmployeeView;

namespace GU.MZ.UI.ViewModel.SupervisionViewModel
{
    /// <summary>
    /// Класс ViewModel стабного "В работе" этапа ведения дела. 
    /// </summary>
    public class StubWorkingStepVm : AbstractSupervisionStepVm
    {

        /// <summary>
        /// Осуществляет переход тома на следующий этап ведения
        /// </summary>
        protected override void StepNext()
        {
            try
            {
                if (DossierFile.IsDirty)
                {
                    NoticeUser.ShowInformation("Необходимо сохранить изменения в томе перед переходом к следующему этапу.");
                    return;
                }
                ChooseResponsibleEmployeeVm.Initialize(DossierFile.Employee,
                                                  Superviser.GetNextScenarioStep());
                if (UiFactory.ShowDialogView(
                    new ChooseResponsibleEmployeeView(),
                    ChooseResponsibleEmployeeVm,
                    "Переход к следующему этапу"))
                {
                    var nextResponsibleEmployee =
                        ChooseResponsibleEmployeeVm.EmployeeList.Single(t => t.Id == ChooseResponsibleEmployeeVm.EmployeeId);

                    Superviser.StepNextWithStatus(nextResponsibleEmployee, TaskStatusType.Ready);
                    RaiseStepNextRequested();
                    RaiseRebuildRequested(DossierFile);
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

        protected override void Rebuild()
        {
            
        }
    }
}
