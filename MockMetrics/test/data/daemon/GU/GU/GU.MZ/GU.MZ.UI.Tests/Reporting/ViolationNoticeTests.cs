using System;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Notifying;
using Moq;
using NUnit.Framework;

namespace GU.MZ.UI.Tests.Reporting
{
    /// <summary>
    /// Приёмочные тесты на создание отчёта "Уведомление о необходимости устранения нарушений"
    /// </summary>
#if Integration
    [TestFixture, RequiresSTA]
#else
    [TestFixture(Ignore = true), RequiresSTA]
#endif
    public class ViolationNoticeTests : IsolatedReportFixture
    {
        /// <summary>
        /// Тест на создание бланка короткой лицензии
        /// </summary>
        [Test]
        public void ShowViolationNoticeReportTest()
        {
            // Arrange
            var notice = ViolationNotice.CreateInstance();
            notice.LicensedActivity = "Оборот наркотических средств, психотропных веществ и их прекурсоров, культивирование наркосодержащих растений";
            notice.TaskRegNumber = 100500;
            notice.TaskStamp = DateTime.Today;
            notice.LicenseHolder = "Красноярская государственная свиноводческая артель Ждигурда Инкорпорэйтед Лимитед имени III Интернационала";
            notice.Address = "666333, респ. Джигурдостан, Пигурдиский р-н, г. Мигурдоевск, ул. Академика Ждигурды 33, д. 66/3 стр 3 корус 1";
            notice.EmployeeName = "Уйгурда Щ.Ъ.";
            notice.EmployeePosition = "Глава отдела обеспечения гугурдой";
            notice.Violations = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. \n Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. \n Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            var report = Mock.Of<ViolationNoticeReport>(t => t.RetrieveData() == notice
                    && t.ViewPath == "Reporting/View/GU.MZ/ViolationNoticeReport.mrt"
                    && t.DataAlias == "data");
            
            // Act
            ShowReport(report);
        }
    }
}
