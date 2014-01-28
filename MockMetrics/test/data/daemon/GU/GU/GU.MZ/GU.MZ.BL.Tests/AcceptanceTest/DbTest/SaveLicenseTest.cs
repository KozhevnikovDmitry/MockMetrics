using System;
using System.Linq;
using Autofac;
using Common.DA.Interface;
using GU.MZ.BL.Tests;
using GU.MZ.BL.Tests.AcceptanceTest;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using NUnit.Framework;

namespace GU.MZ.BL.Test.AcceptanceTest.DbTest
{
    /// <summary>
    /// Приёмочные теста на сохранение данных лицензии
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class SaveLicenseTest : MzAcceptanceTests
    {
        #region Arrange

        private License ArrangeLicense()
        {
            var requisites = LicenseRequisites.CreateInstance();
            requisites.JurRequisites = JurRequisites.CreateInstance();
            requisites.JurRequisites.HeadName = "Пигурда Сигурда Уйгурдович";
            requisites.JurRequisites.HeadPositionName = "Верховный Джигурда и Великий Утёс Одной Ногой на Небе";
            requisites.CreateDate = DateTime.Today;
            requisites.JurRequisites.FullName = "ОАО МосКрасДнепрНефтеГазЛесПромТоргМоргСтрой Интерпрайз Инкорпорэйтед Лимитед и Ко";
            requisites.JurRequisites.ShortName = "ОАО Лесоповал";
            requisites.JurRequisites.FirmName = "Джигурда и Партнёры";
            requisites.JurRequisites.LegalFormId = 1;
            requisites.Address = ArrangeAddress();

            var license = License.CreateInstance();
            license.LicenseDossier = ArrangeLicenseDossier();
            license.RegNumber = "ЛО-24-" + license.LicenseHolder.Inn;
            license.BlankNumber = RandomProvider.RandomNumberString(6);
            license.GrantDate = DateTime.Today;
            license.DueDate = DateTime.Today.AddYears(1);
            license.LicensedActivityId = 1;
            license.CurrentStatus = LicenseStatusType.Active;
            license.GrantOrderRegNumber = "№" + RandomProvider.RandomNumberString(6);
            license.GrantOrderStamp = DateTime.Today;
            license.LicenseObjectList.Add(ArrangeLicenseObject());
            license.LicenseRequisitesList.Add(requisites);
            return license;
        }
        /// <summary>
        /// Подготавливает и возвращает лицензионное дело для прикрепления к лицензии
        /// </summary>
        /// <returns>Лицензиат</returns>
        private LicenseDossier ArrangeLicenseDossier()
        {
            var holder = LicenseHolder.CreateInstance();

            holder.Inn = RandomProvider.RandomNumberString(11);
            holder.Ogrn = RandomProvider.RandomNumberString(13);

            var dossier = LicenseDossier.CreateInstance();
            dossier.LicenseHolder = holder;
            dossier.LicensedActivityId = 1;

            return dossier;
        }

        private LicenseObject ArrangeLicenseObject()
        {
            var licenseObject = LicenseObject.CreateInstance();
            licenseObject.Name = "Наркодиспансер №66 имени св. Бигурды";
            licenseObject.Address = ArrangeAddress();
            licenseObject.LicenseObjectStatusId = 2;
            licenseObject.GrantOrderRegNumber = "№" + RandomProvider.RandomNumberString(6);
            licenseObject.GrantOrderStamp = DateTime.Today;
            var objectSubactivity = ObjectSubactivity.CreateInstance();
            objectSubactivity.LicensedSubactivityId =
                DictionaryManager.GetDictionary<LicensedSubactivity>().First(t => t.LicensedActivityId == 1).Id;
            licenseObject.ObjectSubactivityList.Add(objectSubactivity);
            return licenseObject;
        }

        private Address ArrangeAddress()
        {
            var address = Address.CreateInstance();
            address.Zip = "100500";
            address.City = "Джигурда-Сити";
            address.Street = "Мигурда-Штрассе";
            address.House = "1";
            return address;
        }

        #endregion

        /// <summary>
        /// Тест на сохранение данных лицензии
        /// </summary>
        [Test]
        public void SaveSimpleLicenseTest()
        {
            // Arrange
            var license = ArrangeLicense();

            // Act
            license.LicenseDossier = MzLogicFactory.ResolveDataMapper<LicenseDossier>().Save(license.LicenseDossier);
            license.LicenseDossierId = license.LicenseDossier.Id;
            var savedLicense = MzLogicFactory.ResolveDataMapper<License>().Save(license);

            // Assert
            Assert.NotNull(MzLogicFactory.ResolveDataMapper<License>().Retrieve(savedLicense.Id));
        }
    }
}
