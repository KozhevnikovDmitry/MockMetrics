using System;
using System.Linq;
using Autofac;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.Tests.AcceptanceTest.Supervision;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Licensing
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ImportLicenseTests : SupervisionFixture
    {
        [Test]
        public void ImportDrugNewLicenseTest()
        {
            // Arrange
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            var task = contentParser.ParseTask(1);
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var order = Superviser.PrepareGrantLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "100500";
            order.Stamp = DateTime.Today;
            var licenseProvider = MzLogicFactory.GetLicenseProvider();

            // Act
            var license = licenseProvider.GetNewLicense(Superviser.DossierFile);

            // Assert
            Assert.AreEqual(license.LicenseObjectList.Count, 2);
            var firstObj = license.LicenseObjectList.First();
            var lastObj = license.LicenseObjectList.Last();

            #region First LicenseObject Assert

            Assert.IsEmpty(firstObj.ObjectSubactivityList);
            Assert.IsEmpty(firstObj.Name);
            Assert.AreEqual(firstObj.Note, "Наркотическая деятельность номер 3, Наркотическая деятельность номер 28, Наркотическая деятельность номер 37");

            Assert.AreEqual(firstObj.Address.Zip, "100500");
            Assert.AreEqual(firstObj.Address.CountryRegion, "Красноярский край");
            Assert.AreEqual(firstObj.Address.Area, "Джигурдинский Улус");
            Assert.AreEqual(firstObj.Address.City, "Джигурдоевск");
            Assert.AreEqual(firstObj.Address.Street, "Джигурды");
            Assert.AreEqual(firstObj.Address.House, "1");
            Assert.AreEqual(firstObj.Address.Build, "б");
            Assert.AreEqual(firstObj.Address.Flat, "2");
            Assert.AreEqual(firstObj.Address.Note, "Комната приёма пищи");

            #endregion

            #region First LicenseObject Assert

            Assert.IsEmpty(lastObj.ObjectSubactivityList);
            Assert.IsEmpty(lastObj.Name);
            Assert.AreEqual(lastObj.Note, "Наркотическая деятельность номер 4, Наркотическая деятельность номер 29, Наркотическая деятельность номер 38");

            Assert.AreEqual(lastObj.Address.Zip, "500100");
            Assert.AreEqual(lastObj.Address.CountryRegion, "Краевой красноярск");
            Assert.AreEqual(lastObj.Address.Area, "Ждигурдинский Улус");
            Assert.AreEqual(lastObj.Address.City, "Ждигурдоевск");
            Assert.AreEqual(lastObj.Address.Street, "Ждигурды");
            Assert.AreEqual(lastObj.Address.House, "2");
            Assert.AreEqual(lastObj.Address.Build, "а");
            Assert.AreEqual(lastObj.Address.Flat, "3");
            Assert.AreEqual(lastObj.Address.Note, "Серверная");

            #endregion
        }
        
        [Test]
        public void ImportMedNewLicenseTest()
        {
            // Arrange
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            var task = contentParser.ParseTask(6);
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var order = Superviser.PrepareGrantLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "100500";
            order.Stamp = DateTime.Today;
            var licenseProvider = MzLogicFactory.GetLicenseProvider();

            // Act
            var license = licenseProvider.GetNewLicense(Superviser.DossierFile);

            // Assert
            Assert.AreEqual(license.LicenseObjectList.Count, 2);
            var firstObj = license.LicenseObjectList.First();
            var lastObj = license.LicenseObjectList.Last();

            #region First LicenseObject Assert

            Assert.IsEmpty(firstObj.ObjectSubactivityList);
            Assert.IsEmpty(firstObj.Name);
            Assert.AreEqual(firstObj.Note, "судебно-медицинская экспертиза (лицензирование осуществляет Министерство здравоохранения Российской Федерации)");

            Assert.AreEqual(firstObj.Address.Zip, "100500");
            Assert.AreEqual(firstObj.Address.CountryRegion, "Красноярский край");
            Assert.AreEqual(firstObj.Address.Area, "Джигурдинский Улус");
            Assert.AreEqual(firstObj.Address.City, "Джигурдоевск");
            Assert.AreEqual(firstObj.Address.Street, "Джигурды");
            Assert.AreEqual(firstObj.Address.House, "1");
            Assert.AreEqual(firstObj.Address.Build, "б");
            Assert.AreEqual(firstObj.Address.Flat, "2");
            Assert.IsEmpty(firstObj.Address.Note);

            #endregion

            #region First LicenseObject Assert

            Assert.IsEmpty(lastObj.ObjectSubactivityList);
            Assert.IsEmpty(lastObj.Name);
            Assert.AreEqual(lastObj.Note, "скорая медицинская помощь");

            Assert.AreEqual(lastObj.Address.Zip, "500100");
            Assert.AreEqual(lastObj.Address.CountryRegion, "Краевой красноярск");
            Assert.AreEqual(lastObj.Address.Area, "Ждигурдинский Улус");
            Assert.AreEqual(lastObj.Address.City, "Ждигурдоевск");
            Assert.AreEqual(lastObj.Address.Street, "Ждигурды");
            Assert.AreEqual(lastObj.Address.House, "2");
            Assert.AreEqual(lastObj.Address.Build, "а");
            Assert.AreEqual(lastObj.Address.Flat, "3");
            Assert.IsEmpty(lastObj.Address.Note);

            #endregion
        }

        [Test]
        public void ImportFarmNewLicenseTest()
        {
            // Arrange
            var contentParser = MzLogicFactory.IocContainer.Resolve<ContentParser>();
            var task = contentParser.ParseTask(11);
            ActAccepting(task);
            ActLinkaging(RequisitesOrigin.FromTask);
            ActSupervisionStepNext();
            ActSupervisionStepNextWithStatus(TaskStatusType.Working);
            ActSupervisionStepNextWithStatus(TaskStatusType.Ready);
            var order = Superviser.PrepareGrantLicenseOrder(Superviser.DossierFile.CurrentScenarioStep);
            order.RegNumber = "100500";
            order.Stamp = DateTime.Today;
            var licenseProvider = MzLogicFactory.GetLicenseProvider();

            // Act
            var license = licenseProvider.GetNewLicense(Superviser.DossierFile);

            // Assert
            Assert.AreEqual(license.LicenseObjectList.Count, 2);
            var firstObj = license.LicenseObjectList.First();
            var lastObj = license.LicenseObjectList.Last();

            #region First LicenseObject Assert

            Assert.IsEmpty(firstObj.ObjectSubactivityList);
            Assert.AreEqual(firstObj.Name, "Аптека готовых лекарственных форм");
            Assert.AreEqual(firstObj.Note, "Хранение лекарственных средств для медицинского применения, Хранение лекарственных препаратов для медицинского применения, Перевозка лекарственных средств для медицинского применения");

            Assert.AreEqual(firstObj.Address.Zip, "100500");
            Assert.AreEqual(firstObj.Address.CountryRegion, "Красноярский край");
            Assert.AreEqual(firstObj.Address.Area, "Джигурдинский Улус");
            Assert.AreEqual(firstObj.Address.City, "Джигурдоевск");
            Assert.AreEqual(firstObj.Address.Street, "Джигурды");
            Assert.AreEqual(firstObj.Address.House, "1");
            Assert.AreEqual(firstObj.Address.Build, "б");
            Assert.AreEqual(firstObj.Address.Flat, "2");
            Assert.IsEmpty(firstObj.Address.Note);

            #endregion

            #region First LicenseObject Assert

            Assert.IsEmpty(lastObj.ObjectSubactivityList);
            Assert.AreEqual(lastObj.Name, "Производственная аптека");
            Assert.AreEqual(lastObj.Note, "Перевозка лекарственных препаратов для медицинского применения, Розничная торговля лекарственными препаратами для медицинского применения, Отпуск лекарственных препаратов для медицинского применения, Изготовление лекарственных препаратов для медицинского применения");

            Assert.AreEqual(lastObj.Address.Zip, "500100");
            Assert.AreEqual(lastObj.Address.CountryRegion, "Краевой красноярск");
            Assert.AreEqual(lastObj.Address.Area, "Ждигурдинский Улус");
            Assert.AreEqual(lastObj.Address.City, "Ждигурдоевск");
            Assert.AreEqual(lastObj.Address.Street, "Ждигурды");
            Assert.AreEqual(lastObj.Address.House, "2");
            Assert.AreEqual(lastObj.Address.Build, "а");
            Assert.AreEqual(lastObj.Address.Flat, "3");
            Assert.IsEmpty(lastObj.Address.Note);

            #endregion
        }
    }
}
