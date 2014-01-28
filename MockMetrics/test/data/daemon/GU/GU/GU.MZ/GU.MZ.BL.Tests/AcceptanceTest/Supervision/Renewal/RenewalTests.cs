using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision.Renewal
{
    public abstract class RenewalTests : SupervisionFixture
    {
        protected abstract int NewLicenseServiceId { get; }

        protected DossierFile Reneawal(int newLicenseServiceId, string xmlContentPath)
        {
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(newLicenseServiceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);
            var existinglicense = ActCreateExistingLicense(existingDossier);
            existingDossier.LicenseList.Add(existinglicense);

            var task = ArrangeTask(xmlContentPath, inn, ogrn);

            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActExpertiseOrder();
            ActSupervisionStepNext();
            ActProvideExpertise();
            ActSupervisionStepNext();
            ActInspectionOrder();
            ActSupervisionStepNext();
            ActProvideInspection();
            ActSupervisionStepNext();
            ActRenewalLicenseOrder();
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            ActRenewalLicense();
            Superviser.SaveRenewaledLicense();
            return Superviser.DossierFile;
        }
    }
}