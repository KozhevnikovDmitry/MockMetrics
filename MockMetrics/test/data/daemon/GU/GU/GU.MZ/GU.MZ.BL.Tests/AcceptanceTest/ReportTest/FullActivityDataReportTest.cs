using System;
using GU.MZ.BL.Tests.AcceptanceTest;
using GU.MZ.DataModel.Licensing;

using NUnit.Framework;

namespace GU.MZ.BL.Test.AcceptanceTest.ReportTest
{
    /// <summary>
    /// Приёмочные тесты на выгрузку данных для отчёта "Полный отчёт по лицензированию по виду деятельности"
    /// </summary>
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class FullActivityDataReportTest : MzAcceptanceTests
    {
        /// <summary>
        /// Тест на выгрузку данных для отчёта "Полный отчёт по лицензированию по виду деятельности"
        /// </summary>
        [Test]
        public void GetReportDataTest()
        {
            // Arrange
            var activity = DictionaryManager.GetDictionaryItem<LicensedActivity>(3);

            // Act
            var data = MzLogicFactory.GetFullActivityDataReport(activity, DateTime.MinValue, DateTime.MaxValue).RetrieveData();

            // Assert
            Assert.NotNull(data);
        }
    }
}
