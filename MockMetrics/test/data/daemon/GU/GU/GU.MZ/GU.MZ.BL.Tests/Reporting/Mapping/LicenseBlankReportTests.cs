using System;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.BL.Reporting.Mapping.MappingException;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.Reporting.Mapping
{
    /// <summary>
    /// Тесты на методы класса LicenseBlankReport
    /// </summary>
    [TestFixture]
    public class LicenseBlankReportTests : BaseTestFixture
    {
        #region TestData

        /// <summary>
        /// Мок менеджера базы данных
        /// </summary>
        private IDomainDbManager _mockDb;

        /// <summary>
        /// Мок менеджера справочников
        /// </summary>
        private IDictionaryManager _mockDictionaryManager;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // мок менеджера БД
            _mockDb = Mock.Of<IDomainDbManager>();

            // мок менеджера справочников
            _mockDictionaryManager =
                Mock.Of<IDictionaryManager>(
                    t =>
                    t.GetDictionaryItem<LicensedActivity>(1)
                    == Mock.Of<LicensedActivity>(a => a.BlankName == "Деятельностью" && a.AdditionalInfo == "Дополнительная информация")
                    && t.GetDictionaryItem<LicensedSubactivity>(1)
                       == Mock.Of<LicensedSubactivity>(
                           a =>
                           a.BlankName == "Поддеятельность"
                           && a.SubactivityGroup == Mock.Of<SubactivityGroup>(g => g.BlankName == "Группа поддеятельностей"))
                    && t.GetDictionaryItem<LicensedSubactivity>(2)
                    == Mock.Of<LicensedSubactivity>(
                        a =>
                        a.BlankName == "Очень длинное и страшное название лицензируемой поддеятельности"
                        && a.SubactivityGroup == Mock.Of<SubactivityGroup>(g => g.BlankName == "Группа поддеятельностей")));


        }

        /// <summary>
        /// Возвращает тестируемый отчёт бланка лицензии
        /// </summary>
        /// <param name="licence">Лицензия</param>
        /// <returns>Отчёт бланка лицензии</returns>
        private LicenseBlankReport BuildReport(License licence)
        {
            var licenseBlankReport = new LicenseBlankReport(() => _mockDb, _mockDictionaryManager);
            licenseBlankReport.SetLicense(licence);
            return licenseBlankReport;
        }

        /// <summary>
        /// Возвращает короткую лицензию для печати без бланков приложений
        /// </summary>
        /// <returns>Лицензия на бланк без приложений</returns>
        private License CreateShortLicense()
        {
            var license = License.CreateInstance();
            license.RegNumber = "100500";
            license.GrantDate = DateTime.Today;
            license.GrantOrderRegNumber = "100500";
            license.GrantOrderStamp = DateTime.Today;
            license.DueDate = DateTime.Today.AddYears(10);
            license.BlankNumber = "100500";
            license.LicensedActivityId = 1;

            var licenseRequisites = LicenseRequisites.CreateInstance();
            licenseRequisites.JurRequisites = JurRequisites.CreateInstance();
            licenseRequisites.JurRequisites.HeadName = "ФИО";
            licenseRequisites.JurRequisites.HeadPositionName = "Глава";
            licenseRequisites.AddressId = 1;
            licenseRequisites.JurRequisites.FullName = "ПОЛНОЕ ИМЯ";
            licenseRequisites.JurRequisites.ShortName = "КРАТКОЕ ИМЯ";
            licenseRequisites.JurRequisites.FirmName = "ФИРМЕННОЕ ИМЯ";
            licenseRequisites.CreateDate = DateTime.Now;
            licenseRequisites.Address = Mock.Of<Address>(t => t.ToLongString() == "Адрес");
            
            var licenseObject = LicenseObject.CreateInstance();
            licenseObject.GrantOrderRegNumber = "100500";
            licenseObject.GrantOrderStamp = DateTime.Today;
            licenseObject.LicenseObjectStatusId = 2;
            licenseObject.Address = Address.CreateInstance();
            licenseObject.Address.City = "Адрес";

            var objectSubactivity = ObjectSubactivity.CreateInstance();
            objectSubactivity.LicensedSubactivityId = 1;
            licenseObject.ObjectSubactivityList.Add(objectSubactivity);

            var licenseDossier = LicenseDossier.CreateInstance();
            var licenseHolder = LicenseHolder.CreateInstance();
            licenseHolder.Inn = "100500";
            licenseHolder.Ogrn = "500100";
            licenseDossier.LicenseHolder = licenseHolder;

            license.LicenseDossier = licenseDossier;
            license.LicenseObjectList.Add(licenseObject);
            license.LicenseRequisitesList.Add(licenseRequisites);
            license.LicenseRequisitesList.Add(Mock.Of<LicenseRequisites>(t => t.CreateDate == DateTime.Now.AddDays(-1)));

            return license;
        }

        /// <summary>
        /// Возвращает длинную лицензию для печати с бланками приложений
        /// </summary>
        /// <returns>Лицензия на бланк без приложений</returns>
        private License CreateLongLicense()
        {
            var license = CreateShortLicense();
            license.LicenseObjectList.Clear();
            for (int i = 0; i < 10; i++)
            {
                var licenseObject = LicenseObject.CreateInstance();
                licenseObject.GrantOrderRegNumber = "100500";
                licenseObject.GrantOrderStamp = DateTime.Today;
                licenseObject.LicenseObjectStatusId = 2;
                licenseObject.Address = Address.CreateInstance();
                licenseObject.Address.City = "ОЧЕНЬ ДЛИННЫЙ И ЗАПУТАННЫЙ АДРЕС ОСУЩЕСТВЛЕНИЯ ЛИЦЕНЗИРУЕМОЙ ДЕЯТЕЛЬНОСТИ";

                var objectSubactivity = ObjectSubactivity.CreateInstance();
                objectSubactivity.LicensedSubactivityId = 2;
                licenseObject.ObjectSubactivityList.Add(objectSubactivity);

                license.LicenseObjectList.Add(licenseObject);
            }
            return license;
        }

        ///// <summary>
        ///// Тест на получение не null данных для отчёта
        ///// </summary>
        //[Test]
        //public void RetireveNotNullReportDataTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.NotNull(data);
        //}

        ///// <summary>
        ///// Тест на попытку получения данных лицензии без единого активного объекта с номенклатурой
        ///// </summary>
        //[Test]
        //public void RetrieveLicenseDataWithoutActiveObjectsTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    license.LicenseObjectList.Clear();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));
            
        //    // Assert
        //    Assert.Throws<RetrieveLicenseDataWithoutActiveObjectsException>(() => report.RetrieveData());
        //}

        ///// <summary>
        ///// Тест на правильное заполнение полей данными самой лицензии
        ///// </summary>
        //[Test]
        //public void RetireveCorrectLicenseDataTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.RegNumber, license.RegNumber);
        //    Assert.AreEqual(data.GrantOrderRegNumber, license.GrantOrderRegNumber);
        //    Assert.AreEqual(data.BlankNumber, license.BlankNumber);
        //    Assert.AreEqual(data.LicenseHolderFullName, "ПОЛНОЕ ИМЯ");
        //    Assert.AreEqual(data.LicenseHolderShortName, "КРАТКОЕ ИМЯ");
        //    Assert.AreEqual(data.LicenseHolderFirmName, "ФИРМЕННОЕ ИМЯ");
        //    Assert.AreEqual(data.LicensiarHeadName, "ФИО");
        //    Assert.AreEqual(data.LicensiarHeadPosition, "Глава");
        //    Assert.AreEqual(data.Inn, "100500");
        //    Assert.AreEqual(data.Ogrn, "500100");
        //    Assert.AreEqual(data.LicensiarHeadPosition, "Глава");
        //    Assert.AreEqual(data.LicensedActivity, _mockDictionaryManager.GetDictionaryItem<LicensedActivity>(license.LicensedActivityId).BlankName);
        //    Assert.AreEqual(data.AdditionalActivityInfo, _mockDictionaryManager.GetDictionaryItem<LicensedActivity>(license.LicensedActivityId).AdditionalInfo);
        //}

        ///// <summary>
        ///// Тест на правильное заполнение дат лицензии в отчёте 
        ///// </summary>
        //[Test]
        //public void RetrieveCorrectDateFormatTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.GrantDateDay, license.GrantDate.Value.Day.ToString());
        //    Assert.AreEqual(
        //        data.GrantDateMonthYear,
        //        string.Format(
        //            "{0} {1}", MonthDataHelper.GetMonth(license.GrantDate.Value.Month), license.GrantDate.Value.Year));
            
        //    Assert.AreEqual(data.GrantOrderStampDay, license.GrantOrderStamp.Value.Day.ToString());
        //    Assert.AreEqual(
        //        data.GrantOrderStampMonthYear,
        //        string.Format(
        //            "{0} {1}",
        //            MonthDataHelper.GetMonth(license.GrantOrderStamp.Value.Month),
        //            license.GrantOrderStamp.Value.Year));


        //    Assert.AreEqual(data.DueDateDay, license.DueDate.Value.Day.ToString());
        //    Assert.AreEqual(
        //        data.DueDateMonthYear,
        //        string.Format(
        //            "{0} {1}",
        //            MonthDataHelper.GetMonth(license.DueDate.Value.Month),
        //            license.DueDate.Value.Year));
        //}

        ///// <summary>
        ///// Тест на ошибочное заполненеие дат лицензии в отчёте
        ///// </summary>
        //[Test]
        //public void RetrieveErrorDateFormatTest()
        //{
        //     // Arrange
        //    var license = CreateShortLicense();
        //    license.GrantDate = null;
        //    license.GrantOrderStamp = null;
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.GrantDateDay, "Ошибка");
        //    Assert.AreEqual(data.GrantOrderStampDay, "Ошибка");
        //    Assert.AreEqual(data.GrantDateMonthYear,"Ошибка");
        //    Assert.AreEqual(data.GrantOrderStampMonthYear,"Ошибка");
        //}

        ///// <summary>
        ///// Тест на получение данных бессрочной лицензии 
        ///// </summary>
        //[Test]
        //public void RetrieveLicenseWithoutDueDateTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    license.DueDate = null;
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.DueDateDay, string.Empty);
        //    Assert.AreEqual(data.DueDateMonthYear, string.Empty);
        //}

        ///// <summary>
        ///// Тест по получение пустого списка с данными по приложениям - короткая лицензия
        ///// </summary>
        //[Test]
        //public void RetrieveEmptyAnnexListForShortLicenseTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.LicenseAnnexBlankList.Count, 0);
        //    Assert.AreEqual(data.AnnexCount, "0");
        //    Assert.AreEqual(data.AnnexBlankCount, "0");
        //}

        ///// <summary>
        ///// Тест на заполнение текста лицевой стороны короткой лицензии
        ///// </summary>
        //[Test]
        //public void RetrieveLicenseTextForShortLicenseTest()
        //{
        //    // Arrange
        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.FrontText, "Группа поддеятельностей: Поддеятельность.");
        //    Assert.AreEqual(data.BackTopText, "Адреса осуществления лицензируемого вида деятельности:");
        //    Assert.AreEqual(data.BackBottomText, string.Format("1. {0}.", license.LicenseObjectList.Single().Address.ToLongString()));
        //}
        
        ///// <summary>
        ///// Тест на заполнение текста лицевой стороны короткой лицензии при null BlankName поддеятельностей
        ///// </summary>
        //[Test]
        //public void RetrieveLicenseTextForShortLicenseWithNullBlankNameSubactivityTest()
        //{
        //    // Arrange
        //    _mockDictionaryManager =
        //        Mock.Of<IDictionaryManager>(
        //            t =>
        //            t.GetDictionaryItem<LicensedActivity>(1)
        //            == Mock.Of<LicensedActivity>(a => a.BlankName == "Деятельностью")
        //            && t.GetDictionaryItem<LicensedSubactivity>(1)
        //            == Mock.Of<LicensedSubactivity>(
        //                a =>
        //                a.Name == "Поддеятельность" &&
        //                a.BlankName == string.Empty
        //                && a.SubactivityGroup == Mock.Of<SubactivityGroup>(g => g.Name == "Группа поддеятельностей" && g.BlankName == string.Empty)));

        //    var license = CreateShortLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.FrontText, "Группа поддеятельностей: Поддеятельность.");
        //    Assert.AreEqual(data.BackTopText, "Адреса осуществления лицензируемого вида деятельности:");
        //    Assert.AreEqual(data.BackBottomText, string.Format("1. {0}.", license.LicenseObjectList.Single().Address.ToLongString()));
        //}

        ///// <summary>
        ///// Тест на получение текста лицевой стороны длинной лицензии с приложениями
        ///// </summary>
        //[Test]
        //public void RetrieveLicenseTextForLongLicenseTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.FrontText, "Согласно приложению(ям)");
        //    Assert.AreEqual(data.BackTopText, "Адреса осуществления лицензируемого вида деятельности, согласно приложению(ям)");
        //    Assert.AreEqual(data.BackBottomText, string.Empty);
        //}

        ///// <summary>
        ///// Тест на получение списка данных для бланков приложений лицензии без разбиения приложений
        ///// </summary>
        //[Test]
        //public void RetrieveAnnexDataListForLongLicenseTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.LicenseAnnexBlankList.Count, 10);
        //    Assert.AreEqual(data.AnnexCount, "10");
        //    Assert.AreEqual(data.AnnexBlankCount, "10");
        //}

        /// <summary>
        /// Тест на получение списка данных для бланков приложение лицензии с разбиением приложений
        /// </summary>
        [Test]
        public void RetrieveAnnexDataListForLongLicenseWithAnnexBreakTest()
        {
            // Arrange
            var license = CreateLongLicense();
            var clone = license.LicenseObjectList.First().ObjectSubactivityList.First();
            for (int i = 0; i < 10; i++)
            {
                license.LicenseObjectList.First().ObjectSubactivityList.Add(clone);
            }
            var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

            // Act
            dynamic data = report.RetrieveData();



            // Assert
            Assert.AreEqual(data.LicenseAnnexBlankList.Count, 11);
            Assert.AreEqual(data.AnnexCount, "10");
            Assert.AreEqual(data.AnnexBlankCount, "11");
        }

        ///// <summary>
        ///// Тест на получение данных для отчёта только по активным объектам с номенклатурой
        ///// </summary>
        //[Test]
        //public void RetrieveDataOnlyForActiveObjectTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var licObj = LicenseObject.CreateInstance();
        //    licObj.LicenseObjectStatusId = 100500;
        //    license.LicenseObjectList.Add(licObj);
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();


        //    // Assert
        //    Assert.AreEqual(data.LicenseAnnexBlankList.Count, 10);
        //}

        ///// <summary>
        ///// Тест на проставление порядковых номеров приложениям без разбиения приложение
        ///// </summary>
        //[Test]
        //public void RetriveAnnexDataWithRangeAnnexNumbersTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    var annexIndex = 1;
        //    foreach (var licenseAnnexBlankReportData in data.LicenseAnnexBlankList)
        //    {
        //        Assert.AreEqual(
        //            licenseAnnexBlankReportData.AnnexNumber, annexIndex.ToString());
        //        annexIndex++;
        //    }
        //}

        ///// <summary>
        ///// Тест на проставление порядковых номеров приложениям c разбиением приложений
        ///// </summary>
        //[Test]
        //public void RetriveAnnexDataWithRangeAnnexNumbersWithAnnexBreakTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense(); 
        //    var clone = license.LicenseObjectList.First().ObjectSubactivityList.First();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        license.LicenseObjectList.First().ObjectSubactivityList.Add(clone);
        //    }
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    Assert.AreEqual(data.LicenseAnnexBlankList[1].AnnexNumber, "1 продолжение");
        //}

        ///// <summary>
        ///// Тест на правильное заполнение данных для бланка приложения лицензии при null BlankName поддеятельностей
        ///// </summary>
        //[Test]
        //public void RetrieveCorrectAnnexBlankDataWithNullBlankNameSubactivityTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var licenseObject = license.LicenseObjectList.First();

        //    _mockDictionaryManager =
        //        Mock.Of<IDictionaryManager>(
        //            t =>
        //            t.GetDictionaryItem<LicensedActivity>(1)
        //            == Mock.Of<LicensedActivity>(a => a.BlankName == "Деятельностью")
        //            && t.GetDictionaryItem<LicensedSubactivity>(2)
        //            == Mock.Of<LicensedSubactivity>(
        //                a =>
        //                a.Name == "Очень длинное и страшное название лицензируемой поддеятельности" &&
        //                a.BlankName == string.Empty
        //                && a.SubactivityGroup == Mock.Of<SubactivityGroup>(g => g.Name == "Группа поддеятельностей" && g.BlankName == string.Empty)));

        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    var annexData = data.LicenseAnnexBlankList[0];
        //    Assert.AreEqual(annexData.AddressList, licenseObject.Address.ToLongString());
        //    Assert.AreEqual(annexData.SubactivityList, "Группа поддеятельностей: Очень длинное и страшное название лицензируемой поддеятельности.");
        //}

        ///// <summary>
        ///// Тест на правильное заполнение данных для бланка приложения лицензии
        ///// </summary>
        //[Test]
        //public void RetrieveCorrectAnnexBlankDataTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var licenseObject = license.LicenseObjectList.First();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    var annexData = data.LicenseAnnexBlankList[0];
        //    Assert.AreEqual(annexData.AddressList, licenseObject.Address.ToLongString());
        //    Assert.AreEqual(annexData.SubactivityList, "Группа поддеятельностей: Очень длинное и страшное название лицензируемой поддеятельности.");
        //}


        ///// <summary>
        ///// Тест на правильное заполнение данных для бланка приложения лицензии с длинным список поддеятельностей 
        ///// </summary>
        //[Test]
        //public void RetrieveCorrectAnnexBlankDataWithLongSubactivityListTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();

        //    var licenseObject = license.LicenseObjectList.First();

        //    var subActName = string.Empty;
        //    for (int i = 0; i < 40; i++)
        //    {
        //        subActName += "Поддеятельность";
        //    }

        //    _mockDictionaryManager =
        //       Mock.Of<IDictionaryManager>(
        //           t =>
        //           t.GetDictionaryItem<LicensedActivity>(1)
        //           == Mock.Of<LicensedActivity>(a => a.BlankName == "Деятельностью")
        //           && t.GetDictionaryItem<LicensedSubactivity>(2)
        //           == Mock.Of<LicensedSubactivity>(
        //               a =>
        //               a.BlankName == subActName
        //               && a.SubactivityGroup == Mock.Of<SubactivityGroup>(g => g.BlankName == "Группа поддеятельностей")));

        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();

        //    // Assert
        //    var annexData = data.LicenseAnnexBlankList[0];
        //    Assert.AreEqual(annexData.AddressList, licenseObject.Address.ToLongString());
        //    Assert.AreEqual(annexData.SubactivityList, string.Format("Группа поддеятельностей: {0}.", subActName));
        //}

        
        ///// <summary>
        ///// Тест на правильное заполнение дат приложения лицензии в отчёте 
        ///// </summary>
        //[Test]
        //public void RetrieveCorrectAnnexDateFormatTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();
        //    var annexData = data.LicenseAnnexBlankList[0];

        //    // Assert
        //    Assert.AreEqual(annexData.AnnexGrantDateDay, license.GrantDate.Value.Day.ToString());
        //    Assert.AreEqual(
        //        annexData.AnnexGrantDateMonthYear,
        //        string.Format(
        //            "{0} {1}", MonthDataHelper.GetMonth(license.GrantDate.Value.Month), license.GrantDate.Value.Year));
        //}
        
        ///// <summary>
        ///// Тест на ошибочное заполненеие дат приложения лицензии в отчёте
        ///// </summary>
        //[Test]
        //public void RetrieveErrorAnnexDateFormatTest()
        //{
        //    // Arrange
        //    var license = CreateLongLicense();
        //    license.LicenseObjectList.First().GrantOrderStamp = null;
        //    var report = BuildReport(Mock.Of<License>(t => t.Clone() == license));

        //    // Act
        //    dynamic data = report.RetrieveData();
        //    var annexData = data.LicenseAnnexBlankList[0];

        //    // Assert
        //    Assert.AreEqual(annexData.AnnexGrantDateDay, "Ошибка");
        //    Assert.AreEqual(annexData.AnnexGrantDateMonthYear, "Ошибка");
        //}

        ///// <summary>
        ///// Тест на глубокое клонирование объекта лицензии в конструкторе маппера
        ///// </summary>
        //[Test]
        //public void DeepCloneLicenseInMapperCtorTest()
        //{
        //    // Arrange
        //    var license = new Mock<License>();
        //    var licenseObject = new Mock<LicenseObject>();
        //    var subActivity = new Mock<ObjectSubactivity>();
        //    licenseObject.Setup(t => t.Clone()).Returns(Mock.Of<LicenseObject>(t => t.ObjectSubactivityList == new EditableList<ObjectSubactivity> { subActivity.Object }));
        //    license.Setup(t => t.Clone()).Returns(Mock.Of<License>(t => t.LicenseObjectList == new EditableList<LicenseObject> { licenseObject.Object }));

        //    // Act
        //    BuildReport(license.Object);

        //    // Assert
        //    license.Verify(t => t.Clone(), Times.Once());
        //    licenseObject.Verify(t => t.Clone(), Times.Once());
        //    subActivity.Verify(t => t.Clone(), Times.Once());
        //}
    }
}
