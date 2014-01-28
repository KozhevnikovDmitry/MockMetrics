using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using BLToolkit.EditableObjects;
using GU.MZ.BL.Tests;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using NUnit.Framework;

namespace GU.MZ.UI.Tests.Reporting
{
    /// <summary>
    /// Приёмочные тесты на создание отчёта "Бланк лицензии"
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true), RequiresSTA]
#endif
    public class ReportLicenseTests : MzUiAcceptanceTests
    {
        #region Arrange

        /// <summary>
        /// Подготавливает и возвращает короткую лицензию для печати без бланков приложений 
        /// </summary>
        /// <param name="licensedActivity">Лицензируемая деятельность</param>
        /// <returns>Короткая лицензия</returns>
        private DataModel.Licensing.License ArrangeShortLicense(LicensedActivity licensedActivity)
        {
            var requisites = LicenseRequisites.CreateInstance();
            requisites.JurRequisites = JurRequisites.CreateInstance();
            requisites.JurRequisites.HeadName = "Сигурда Д.Д.";
            requisites.JurRequisites.HeadPositionName = "Главный Уйгурда и Великий Утёс Одной Ногой на Небе";
            requisites.CreateDate = DateTime.Today;
            requisites.JurRequisites.FullName = "Красноярская государственная свиноводческая артель Ждигурда Инкорпорэйтед Лимитед имени III Интернационала";
            requisites.JurRequisites.ShortName = "КГСА Ждигурда";
            requisites.JurRequisites.FirmName = "КГСА Ждигурда Миллениум Инкорпорэйтед";
            requisites.JurRequisites.LegalFormId = 1;
            requisites.Address = this.ArrangeAddress();

            var license = DataModel.Licensing.License.CreateInstance();
            license.LicenseDossier = this.ArrangeLicenseDossier();
            license.RegNumber = string.Format("ЛО 24 {0} {1}", licensedActivity.Code,  RandomProvider.RandomNumberString(6));
            license.GrantDate = DateTime.Today;
            license.BlankNumber = "100500";
            license.CurrentStatus = LicenseStatusType.Active;
            license.LicensedActivityId = licensedActivity.Id;
            license.GrantOrderStamp = DateTime.Today;
            license.GrantOrderRegNumber = string.Format("лиц № {0}", RandomProvider.RandomNumberString(6));
            license.DueDate = null;

            var licenseObject = LicenseObject.CreateInstance();
            licenseObject.Address = this.ArrangeAddress();
            licenseObject.GrantOrderStamp = DateTime.Today;
            licenseObject.GrantOrderRegNumber = license.GrantOrderRegNumber;
            licenseObject.LicenseObjectStatusId = 2;

            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 1));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 2));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 3));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 4));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 5));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 6));
            licenseObject.ObjectSubactivityList.Add(ArrangeObjectSubactivity(licensedActivity, 7));

            license.LicenseObjectList = new EditableList<LicenseObject> { licenseObject };
            license.LicenseRequisitesList = new EditableList<LicenseRequisites> { requisites };
            return license;
        }

        /// <summary>
        /// Подготавливает и возвращает длинную лицензию для печати с бланками приложений 
        /// </summary>
        /// <param name="licensedActivity">Лицензируемая деятельность</param>
        /// <returns>Длинная лицензия</returns>
        private DataModel.Licensing.License ArrangeLongLicense(LicensedActivity licensedActivity)
        {
            var license = this.ArrangeShortLicense(licensedActivity);

            for (int i = 0; i < 10; i++)
            {
                var newObj = license.LicenseObjectList.First().Clone();
                license.LicenseObjectList.Add(newObj);
            }

            return license;
        }

        /// <summary>
        /// Подготавливает и возвращает поддеятельность осуществляемую на объекте
        /// </summary>
        /// <param name="licensedActivity">Лицензируемая деятельность</param>
        /// <returns>Поддеятельность на объекте</returns>
        private ObjectSubactivity ArrangeObjectSubactivity(LicensedActivity licensedActivity, int index)
        {
            var objectActivity = ObjectSubactivity.CreateInstance();
            objectActivity.LicensedSubactivity =
                DictionaryManager.GetDictionary<LicensedSubactivity>()
                                      .Where(t => t.LicensedActivityId == licensedActivity.Id)
                                      .ElementAt(index);

            objectActivity.LicensedSubactivityId = objectActivity.LicensedSubactivity.Id;

            return objectActivity;
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

            return dossier;
        }

        /// <summary>
        /// Подготавливает и возвращает адрес 
        /// </summary>
        /// <returns>Адрес</returns>
        private Address ArrangeAddress()
        {
            var address = Address.CreateInstance();
            address.Zip = "100500";
            address.City = "п. Верхняя Ждигурда";
            address.Street = "ул. Святой Ждигурды";
            address.House = "1a";
            address.CountryRegion = "Красноярский край";
            address.Build = @"корп.12\67";
            return address;
        }

        #endregion


        #region Act

        /// <summary>
        /// Сохраняет объект лицнезию и лицензиата
        /// </summary>
        /// <param name="license">Лицензия на сохранение</param>
        /// <returns>Сохранённая лицензия</returns>
        private DataModel.Licensing.License SaveLicense(DataModel.Licensing.License license)
        {
            license.LicenseDossier = this.MzLogicFactory.ResolveDataMapper<LicenseDossier>().Save(license.LicenseDossier);
            license.LicenseDossierId = license.LicenseDossier.Id;
            return this.MzLogicFactory.ResolveDataMapper<DataModel.Licensing.License>().Save(license);
        }

        #endregion

        /// <summary>
        /// Обёртка для нормального открытия диалогов
        /// </summary>
        /// <param name="showDialogAction">Делегат открытия диалога</param>
        /// <remarks>
        /// Глушит какой-то страшный exception, возникающий после закрытия диалога.
        /// Видимо связано с тем, что окошко отображается вне поднятого приложения 
        /// </remarks>
        private void Show(Action showDialogAction)
        {
            try
            {
                showDialogAction();
            }
            catch (InvalidComObjectException)
            {
                throw;
            }
        }

        /// <summary>
        /// Тест на сохранение данных короткой лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void SaveShortLicenseTest(int licenseActivityId)
        {
            // Arrange
            var license =
                this.ArrangeShortLicense(this.DictionaryManager.GetDictionaryItem<LicensedActivity>(licenseActivityId));

            // Act
            var savedLicense = this.SaveLicense(license);

            // Assert
            Assert.NotNull(this.MzLogicFactory.ResolveDataMapper<DataModel.Licensing.License>().Retrieve(savedLicense.Id));
        }

        /// <summary>
        /// Тест на сохранение данных длинной лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void SaveLongLicenseTest(int licenseActivityId)
        {
            // Arrange
            var license =
                this.ArrangeLongLicense(this.DictionaryManager.GetDictionaryItem<LicensedActivity>(licenseActivityId));

            // Act
            var savedLicense = this.SaveLicense(license);

            // Assert
            Assert.NotNull(this.MzLogicFactory.ResolveDataMapper<DataModel.Licensing.License>().Retrieve(savedLicense.Id));
        }

        /// <summary>
        /// Тест на создание бланка короткой лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShowShortLicenseBlankTest(int licenseActivityId)
        {
            // Arrange
            var license =
                this.ArrangeShortLicense(this.DictionaryManager.GetDictionaryItem<LicensedActivity>(licenseActivityId));
            var savedLicense = this.SaveLicense(license);
            var report = this.MzLogicFactory.GetLicenseBlankReport(savedLicense);
            var view = MzUiFactory.GetReportPresenter(report);

            // Act
            Show(
                () =>
                MzUiFactory.ShowDialogView(
                    view,
                    view.DataContext as INotifyPropertyChanged,
                    "Тест отчёт",
                    ResizeMode.CanResize,
                    SizeToContent.WidthAndHeight,
                    true));

            // Assert
            Assert.True(true);
        }

        /// <summary>
        /// Тест на создание бланка длинной лицензии
        /// </summary>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShowLongLicenseBlankTest(int licenseActivityId)
        {
            // Arrange
            var license =
                this.ArrangeLongLicense(this.DictionaryManager.GetDictionaryItem<LicensedActivity>(licenseActivityId));

            var savedLicense = this.SaveLicense(license);
            var report = this.MzLogicFactory.GetLicenseBlankReport(savedLicense);
            var view = MzUiFactory.GetReportPresenter(report);

            // Act
            Show(
                () =>
                MzUiFactory.ShowDialogView(
                    view,
                    view.DataContext as INotifyPropertyChanged,
                    "Тест отчёт",
                    ResizeMode.CanResize,
                    SizeToContent.WidthAndHeight,
                    true));

            // Assert
            Assert.True(true);
        }
    }
}
