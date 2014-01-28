using System;
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
    /// Приёмочные тесты на логику привязки заявок на предоставление лицензии
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class LinkageFullScenariedTaskTests : SupervisionFixture
    {
        /// <summary>
        /// Привязка заявки на предоставление лицензии к существующему лицензионному делу и существующему лицензиату
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void LinkageNewLicenseFileTest(int serviceId)
        {
            // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Console.WriteLine();
            Assert.AreEqual(dossierFile.PersistentState, PersistentState.Old, 
                "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.Old, 
                "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.Id, existingHolder.Id, 
                "dossierFile dossier holder is existing assert");

            Assert.AreEqual(dossierFile.LicenseDossier.Id, existingDossier.Id, 
                "dossierFile dossier is existing assert");

            Assert.AreEqual(dossierFile.RegNumber, 2,
                "dossierFile regnumber equals 1 for old dossier");

            string code = DictionaryManager.GetDictionaryItem<LicensedActivity>(dossierFile.LicenseDossier.LicensedActivityId).Code;

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                string.Format("ЛО-24-{0}-{1}", code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Привязка заявки на предоставление лицензии, заведение лицензиата и дела по данными заявки
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void LinkageNewLicenseFileCreateHolderAndDossierTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.False(Superviser.AvailableRequisites.ContainsKey(RequisitesOrigin.FromRegistr),
                        "Linkager actualRequisites null assert");

            Assert.True(Superviser.AvailableRequisites.ContainsKey(RequisitesOrigin.FromTask),
                            "Linkager taskRequisites not null assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.New, 
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.PersistentState, PersistentState.New, 
                            "dossierFile dossier new holder assert");

            Assert.AreEqual(dossierFile.HolderRequisites, dossierFile.LicenseDossier.LicenseHolder.RequisitesList.Single(),
                            "dossier requisites equals new holder single requisistes");

            Assert.AreEqual(dossierFile.RegNumber, 1,
                            "dossierFile regnumber equals 1 for new dossier");

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber, 
                            string.Format("ЛО-24-{0}-{1}", dossierFile.LicenseDossier.LicensedActivity.Code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Привязка заявки на предоставление лицензии, заведение дела для существующего лицензиата по данными заявки
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void LinkageNewLicenseFileCreateDossierForHolderTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            var exisitngHolder = ActCreateJuridicalLicenseHolder(task);
            ActAccepting(task);
            var linkWrapper = ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.HolderRequisites, linkWrapper.AvailableRequisites[RequisitesOrigin.FromRegistr],
                            "Linkager dossierFile.LicenseDossier.LicenseHolder is existing assert");

            Assert.AreEqual(linkWrapper.AvailableRequisites[RequisitesOrigin.FromRegistr].LicenseHolder.Inn, linkWrapper.AvailableRequisites[RequisitesOrigin.FromTask].LicenseHolder.Inn,
                            "Linkager actualHolder inn equals taskHolder assert");

            Assert.AreEqual(linkWrapper.AvailableRequisites[RequisitesOrigin.FromRegistr].LicenseHolder.Ogrn, linkWrapper.AvailableRequisites[RequisitesOrigin.FromTask].LicenseHolder.Ogrn,
                            "Linkager actualHolder ogrn equals taskHolder assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.New, 
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.Id, exisitngHolder.Id,
                            "dossierFile dossier holder is existing assert");

            var currentRequisistes = dossierFile.LicenseDossier.LicenseHolder.RequisitesList.OrderByDescending(r => r.CreateDate).First();
            Assert.AreEqual(dossierFile.HolderRequisites, currentRequisistes,
                            "dossier requisites equals old holder actual requisistes");

            Assert.AreEqual(dossierFile.RegNumber, 1,
                            "dossierFile regnumber equals 1 for new dossier");

            string code = DictionaryManager.GetDictionaryItem<LicensedActivity>(dossierFile.LicenseDossier.LicensedActivityId).Code;

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                            string.Format("ЛО-24-{0}-{1}", code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Привязка заявки на предоставление лицензии, ОГРН и ИНН с существующим лицензиатом совпадают неполностью, заведение дела для существующего лицензиата по данными заявки
        /// </summary>
        [TestCase(1)]
        [TestCase(6)]
        [TestCase(11)]
        public void LinkageNewLicenseFileWithDoubtFullHolderDataTest(int serviceId)
        {
            // Arrange
            var task = ArrangeTask(serviceId);

            // Act
            var taskParser = MzLogicFactory.GetTaskParser();
            var licenseHolder = taskParser.ParseHolder(task);
            licenseHolder.Inn = "WRONG";
            var exisitngHolder = MzLogicFactory.ResolveDataMapper<LicenseHolder>().Save(licenseHolder);
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.HolderRequisites, Superviser.AvailableRequisites[RequisitesOrigin.FromRegistr],
                            "Linkager dossierFile.LicenseDossier.LicenseHolder is existing assert");

            Assert.AreNotEqual(Superviser.AvailableRequisites[RequisitesOrigin.FromRegistr].LicenseHolder.Inn, Superviser.AvailableRequisites[RequisitesOrigin.FromTask].LicenseHolder.Inn,
                              "Linkager actualHolder inn equals taskHolder assert");

            Assert.AreEqual(Superviser.AvailableRequisites[RequisitesOrigin.FromRegistr].LicenseHolder.Ogrn, Superviser.AvailableRequisites[RequisitesOrigin.FromTask].LicenseHolder.Ogrn,
                            "Linkager actualHolder ogrn equals taskHolder assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.New,
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.Id, exisitngHolder.Id,
                            "dossierFile dossier holder is existing assert");

            var currentRequisistes = dossierFile.LicenseDossier.LicenseHolder.RequisitesList.OrderByDescending(r => r.CreateDate).First();
            Assert.AreEqual(dossierFile.HolderRequisites, currentRequisistes,
                            "dossier requisites equals old holder actual requisistes");

            Assert.AreEqual(dossierFile.RegNumber, 1,
                            "dossierFile regnumber equals 1 for new dossier");

            string code = DictionaryManager.GetDictionaryItem<LicensedActivity>(dossierFile.LicenseDossier.LicensedActivityId).Code;

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                            string.Format("ЛО-24-{0}-{1}", code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Привязка заявки на переоформление лицензии к существующей лицензии, существующему лицензионному делу, существующему лицензиату
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void LinkageRenewalFileTest(int serviceId)
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
            Assert.AreEqual(dossierFile.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.Old,
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.Id, existingHolder.Id,
                            "dossierFile holder is existing holder assert");

            Assert.AreEqual(dossierFile.LicenseDossier.Id, existingDossier.Id,
                            "dossierFile dossier is existing dossier assert");

            Assert.AreEqual(dossierFile.License.Id, existinglicense.Id,
                            "dossierFile license is existing license assert");

            Assert.AreEqual(dossierFile.RegNumber, 2,
                            "dossierFile regnumber equals 1 for old dossier");

            string code = DictionaryManager.GetDictionaryItem<LicensedActivity>(dossierFile.LicenseDossier.LicensedActivityId).Code;

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                            string.Format("ЛО-24-{0}-{1}", code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }

        /// <summary>
        /// Привязка заявки на переоформление лицензии(лицензия в деле отсутствует) к существующему лицензионному делу, существующему лицензиату
        /// </summary>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(12)]
        public void LinkageRenewalFileNoLicenseTest(int serviceId)
        {
            // Arrange
            var inn = RandomProvider.RandomNumberString(11);
            var ogrn = RandomProvider.RandomNumberString(13);

            var existingtask = ArrangeTask(GetNewLicenseServiceId(serviceId), inn, ogrn);
            var existingHolder = ActCreateJuridicalLicenseHolder(existingtask);
            var existingDossier = ActCreateExistingLicenseDossier(existingHolder, existingtask);

            var task = ArrangeTask(serviceId, inn, ogrn);

            // Act
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromRegistr);
            var dossierFile = Superviser.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.PersistentState, PersistentState.Old,
                            "Saving dossierFile assert");

            Assert.AreEqual(dossierFile.LicenseDossier.PersistentState, PersistentState.Old,
                            "dossierFile new dossier assert");

            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder.Id, existingHolder.Id,
                            "dossierFile holder is existing holder assert");

            Assert.AreEqual(dossierFile.LicenseDossier.Id, existingDossier.Id,
                            "dossierFile dossier is existing dossier assert");

            Assert.Null(dossierFile.LicenseId,
                            "dossierFile license is null assert");

            Assert.AreEqual(dossierFile.RegNumber, 2,
                            "dossierFile regnumber equals 1 for old dossier");

            string code = DictionaryManager.GetDictionaryItem<LicensedActivity>(dossierFile.LicenseDossier.LicensedActivityId).Code;

            Assert.AreEqual(dossierFile.LicenseDossier.RegNumber,
                            string.Format("ЛО-24-{0}-{1}", code, dossierFile.LicenseDossier.LicenseHolder.Inn),
                            "dossier regnumber is in correct format");

            Assert.NotNull(MzLogicFactory.ResolveDataMapper<DossierFile>().Retrieve(dossierFile.Id),
                "Assert that new dossierFile succesfully retrieve");
        }
    }
}
