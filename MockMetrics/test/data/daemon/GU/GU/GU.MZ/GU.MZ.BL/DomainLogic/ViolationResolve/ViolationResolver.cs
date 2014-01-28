using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.BL.DomainLogic.ViolationResolve
{
    /// <summary>
    /// Класс, ответсвенный за устранение нарушений
    /// </summary>
    public class ViolationResolver
    {
        public ViolationResolveInfo PrepareViolationResolveInfo(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            var violationResolveInfo = ViolationResolveInfo.CreateInstance();

            var step = dossierFile.GetStep(scenarioStep);

            step.ViolationResolveInfo = violationResolveInfo;

            return violationResolveInfo;
        }

        public ViolationNotice PrepareViolationNotice(DossierFile dossierFile, ScenarioStep scenarioStep)
        {
            if (!dossierFile.TaskStamp.HasValue)
            {
                throw new NullTaskCreateDateException(dossierFile.TaskId);
            }

            var step = dossierFile.GetStep(scenarioStep);

            var violationNotice = ViolationNotice.CreateInstance();
            violationNotice.TaskRegNumber = dossierFile.TaskId;
            violationNotice.TaskStamp = dossierFile.TaskStamp.Value;

            violationNotice.EmployeeName = dossierFile.EmployeeName;
            violationNotice.EmployeePosition = dossierFile.EmployeePosition;
            violationNotice.Address = dossierFile.HolderAddress;
            violationNotice.LicenseHolder = dossierFile.HolderFullName;
            violationNotice.LicensedActivity = dossierFile.LicensedActivityName;

            step.ViolationNotice = violationNotice;

            return violationNotice;
        }
    }
}
