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
    public class StandartOrderTests : IsolatedReportFixture
    {

        [Test]
        public void StandartOrderTest()
        {
            // Arrange
            var orderOption = StandartOrderOption.CreateInstance();
            orderOption.Name = "Приказ о присвоении звания народного джигурды";
            orderOption.Preamble = "Приказ о присвоении звания народного джигурды', 'В соответствии с частью 345 статьи 10094 Федерального закона от 32.05.2010,5 № 10094-ФЗ «О порядке присвоение званий народного джигурды», Федеральным законом от 33.06.2011б5 № 100500-ФЗ «О правах народа на джигурду»  пунктами 3423.42348 и 234.65762 Положения о министерстве зернохранения Красноярского края, утверждённого постановлением Правительства Красноярского края от 32.01.2006 № 4324-п, присвоить звание народного джигурды за успешное и регулярное народно осуществление джигурдинской деятельности (за исключением указанной деятельности, осуществляемой пигурдинскими организациями и другими организациями, входящими в частную систему джигурдоохранения, на территории инновационного центра «Сколково»), а также вручить жёлтый трёхостный удлинённый трактор-тундроход Машенька-21 на паровом ходу в качестве поощрения.";
            orderOption.AnnexPreamble = "Перечень юридических морд и индивидуальных пигурдей, в отношении которых принято решение о присвоении звания народного джигурды и вручении трактора";
            orderOption.SubjectIdName = "Номер наградного листа";
            orderOption.SubjectStampName = "Дата сборки трактора";
            orderOption.OrderDetailHeader = "Номер наградного листа и дата сборки трактора";
            orderOption.DetailCommentPattern = "И это ещё не всё! Позвоните прямо сейчас и получите:";

            var data = StandartOrder.CreateInstance();
            data.RegNumber = "1234654-лиц.";
            data.Stamp = DateTime.Today;
            data.EmployeeName = "Джигурда-Кутузов-Голенищев Мигурда Пигурдоевич";
            data.EmployeePosition = "Великий и могучий Утёс одной ногой на небе. Доктор наук.";
            data.EmployeeContacts = "8(391)2-100-500, gigurda@pigurda.com";
            data.LicensiarHeadName = "Ждигруда-Хигурда-Оглы Семён Семёнович";
            data.LicensiarHeadPosition = "Император Человечества и тёмный владыка Ситх";
            data.OrderOption = orderOption;

            var detail = StandartOrderDetail.CreateInstance();
            detail.Inn = "10050010050";
            detail.Ogrn = "1005001005001";
            detail.SubjectId = "100500";
            detail.SubjectStamp = DateTime.Today;
            detail.FullName = "Красноярская государственная свиноводческая артель Ждигурда Инкорпорэйтед Лимитед имени III Интернационала";
            detail.ShortName = "КГСА Ждигурда";
            detail.FirmName = "КГСА Ждигурда Миллениум Инкорпорэйтед";
            detail.Address = "Джигурдинский АО, улус Мигурда, пгрт им. Третьего Пигурды, проулок Кривой д.333/444 корп.5 стр.1 кв. 9, спросить Никиту";
            detail.Comment = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. \n Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. \n Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";


            data.DetailList = new EditableList<StandartOrderDetail>
            {
                detail,detail
            };

            var report =
                Mock.Of<IReport>(
                    t =>
                    t.RetrieveData() == data && t.ViewPath == "Reporting/View/GU.MZ/StandartOrderReport.mrt"
                    && t.DataAlias == "data");

            ShowReport(report);
        }
    }
}