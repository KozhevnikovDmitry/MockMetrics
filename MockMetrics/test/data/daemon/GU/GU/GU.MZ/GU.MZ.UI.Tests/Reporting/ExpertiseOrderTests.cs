using System;
using BLToolkit.EditableObjects;
using Common.BL.ReportMapping;
using GU.MZ.DataModel.MzOrder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.UI.Tests.Reporting
{
#if Integration
    [TestFixture, RequiresSTA]
#else
    [TestFixture(Ignore = true), RequiresSTA]
#endif
    public class ExpertiseOrderTests : IsolatedReportFixture
    {
        [Test]
        public void ExpertiseOrderTest()
        {
            // Arrange
            var data = ExpertiseOrder.CreateInstance();
            data.RegNumber = "1234654-лиц.";
            data.Stamp = DateTime.Today;
            data.EmployeeName = "Джигурда-Кутузов-Голенищев Мигурда Пигурдоевич";
            data.EmployeePosition = "Великий и могучий Утёс одной ногой на небе. Доктор наук.";
            data.EmployeeContacts = "8(391)2-100-500, gigurda@pigurda.com"; 
            data.TaskId = 100500;
            data.TaskStamp = DateTime.Today.AddDays(-7);
            data.Inn = "50010050010";
            data.FullName = "Красноярская государственная свиноводческая артель Ждигурда Инкорпорэйтед Лимитед имени III Интернационала";
            data.DueDays = 10;
            data.StartDate = DateTime.Today.AddDays(1);
            data.DueDate = DateTime.Today.AddDays(11);
            data.Address = "Джигурдинский АО, улус Мигурда, пгрт им. Третьего Пигурда, проулок Кривой д.333/444 корп.5 стр.1 кв. 9, спросить Никиту";
            data.LicensedActivity = "медицинской деятельности";

            var agree = ExpertiseOrderAgree.CreateInstance();
            agree.EmployeeName = "Джигурда-Кутузов-Голенищев Мигурда Пигурдоевич";
            agree.EmployeePosition = "Великий и могучий Утёс одной ногой на небе. Доктор наук.";

            data.ExpertiseOrderAgreeList = new EditableList<ExpertiseOrderAgree>
            {
                agree, agree
            };

            var address = ExpertiseHolderAddress.CreateInstance();
            address.Address = "Джигурдинский АО, улус Мигурда, пгрт им. Третьего Пигурда, проулок Кривой д.333/444 корп.5 стр.1 кв. 9, спросить Никиту";

            data.ExpertiseHolderAddressList = new EditableList<ExpertiseHolderAddress>
            {
                address, address
            };

            var report =
                Mock.Of<IReport>(
                    t =>
                    t.RetrieveData() == data && t.ViewPath == "Reporting/View/GU.MZ/ExpertiseOrderReport.mrt"
                    && t.DataAlias == "data");

            ShowReport(report);
        }
    }
}

