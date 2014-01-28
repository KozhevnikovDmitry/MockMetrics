using System;
using System.Collections.Generic;
using Common.BL.ReportMapping;
using GU.MZ.BL.Reporting.Data;
using Moq;
using NUnit.Framework;

namespace GU.MZ.UI.Tests.Reporting
{
#if Integration
    [TestFixture, RequiresSTA]
#else
    [TestFixture(Ignore = true), RequiresSTA]
#endif
    public class InventoryReportTests : IsolatedReportFixture
    {
        [Test]
        public void InventoryReportTest()
        {
            // Arrange
            var data = new AcceptTaskInventory();
            data.AuthorName = "Джигурда-Кутузов-Голенищев Мигурда Пигурдоевич";
            data.AuthorPosition = "Великий и могучий Утёс одной ногой на небе. Доктор наук.";
            data.HolderName = "Краевая отраслевая многоцелевая полевая артель Краслеспромторгморгстрой";
            data.InventoryRegNumber = "100500";
            data.InventoryStamp = DateTime.Today;
            data.LicensedActivityName =
                "Оборот наркотических средств, психотропных веществ и их прекурсоров, культивирование наркосодержащих растений";
            data.InventDocumentList = new List<AcceptTaskInventory.InventDocument>
            {
                new AcceptTaskInventory.InventDocument {Count = 1, DocumentName = "Заявление о предоставлении лицензии" },
                new AcceptTaskInventory.InventDocument {Count = 2, DocumentName = "Документ, удостоверяющий личность" },
                new AcceptTaskInventory.InventDocument {Count = 3, DocumentName = "Документ, входящий в перечень, определяемый положением о лицензировании конкретного вида деятельности и свидетельствующий о соответствии соискателя лицензии лицензионным требованиям" },
                new AcceptTaskInventory.InventDocument {Count = 4, DocumentName = "Cвидетельство о постановке на учет индивидуального предпринимателя в налоговом органе на территории РФ" },
                new AcceptTaskInventory.InventDocument {Count = 5, DocumentName = "Cвидетельство о постановке на учет российской организации в налоговом органе на территории РФ" },
                new AcceptTaskInventory.InventDocument {Count = 100, DocumentName = "Лицензия на осуществление деятельности по обороту наркотических средств, психотропных веществ и их прекурсоров, культивированию наркосодержащих растений" }
            };

            var report =
                Mock.Of<IReport>(
                    t =>
                    t.RetrieveData() == data && t.ViewPath == "Reporting/View/GU.MZ/AcceptTaskInventoryReport.mrt"
                    && t.DataAlias == "data");

            ShowReport(report);
        }
    }
}
