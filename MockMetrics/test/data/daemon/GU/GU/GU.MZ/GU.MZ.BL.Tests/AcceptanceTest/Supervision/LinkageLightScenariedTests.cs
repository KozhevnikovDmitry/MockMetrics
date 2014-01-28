using System.Linq;
using Common.DA;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision
{
    /// <summary>
    /// Приёмочные тесты на логику привязки томов с облегчёнными сценариями сценариями ведения
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class LinkageLightScenariedTests : SupervisionFixture
    {
        /// <summary>
        /// Тест на привязку заявления по облегчённой схеме
        /// </summary>
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void LinkageDossierFileTest(int serviceId)
        {
            // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);
            var existinglicense = ActCreateExistingLicense(existingDossier);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.True(Superviser.AvailableRequisites.ContainsKey(RequisitesOrigin.FromRegistr),
                        "Linkager actualRequisites null assert");

            Assert.False(Superviser.AvailableRequisites.ContainsKey(RequisitesOrigin.FromTask),
                            "Linkager taskRequisites not null assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.Old,
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.License.Id, existinglicense.Id,
                            "dossierFile license is existing license assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.PersistentState, PersistentState.Old,
                            "dossierFile dossier new holder assert");

            Assert.AreEqual(dossierFile.HolderRequisites, dossierFile.LicenseDossier.LicenseHolder.RequisitesList.Single(),
                            "dossier requisites equals new holder single requisistes");

            Assert.AreEqual(dossierFile.RegNumber, 2,
                            "dossierFile regnumber equals 2 for new dossier");

            var licensedActivity =
                DictionaryManager.GetDictionaryItem<LicensedActivity>(
                    dossierFile.LicenseDossier.LicensedActivityId);

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                            string.Format("ЛО-24-{0}-{1}", licensedActivity.Code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Тест на привязку тома к новому лицензионному делу и существующему лицензиату c неполным совпадением данных
        /// </summary>
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        public void LinkageDossierFileWithDoubtFullHolderDataTest(int serviceId)
        {
             // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            existingHolder.Ogrn = "WRONG";
            existingHolder = MzLogicFactory.ResolveDataMapper<LicenseHolder>().Save(existingHolder);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);
            var existinglicense = ActCreateExistingLicense(existingDossier);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
           var linkWrapper = ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.True(linkWrapper.IsHolderDataDoubtfull, 
                "Assert that holder data doubtfull");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }
    }
}
