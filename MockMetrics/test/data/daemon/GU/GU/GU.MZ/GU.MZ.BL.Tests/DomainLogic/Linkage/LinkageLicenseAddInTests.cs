using System;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.DA.Interface;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class LinkageLicenseAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageLicenseAddIn(Mock.Of<IParser>(), Mock.Of<IDomainDataMapper<License>>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 3);
        }

        [Test]
        public void NoLinkageForNewLicenseTaskTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var addin = new LinkageLicenseAddIn(Mock.Of<IParser>(), Mock.Of<IDomainDataMapper<License>>());

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.IsNull(fileLink.License);
        }

        [Test]
        public void NoDossierLinkagedTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var addin = new LinkageLicenseAddIn(Mock.Of<IParser>(), Mock.Of<IDomainDataMapper<License>>());

            // Assert
            Assert.Throws<NoDossierLinkagedException>(() => addin.Linkage(fileLink, Mock.Of<IDomainDbManager>()));
        }

        [Test]
        public void LinkageLicenseTest()
        {
            // Arrange
            var db = Mock.Of<IDomainDbManager>();
            var license = Mock.Of<License>(t => t.RegNumber == "цщ-100500" && t.GrantDate == DateTime.Today);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "100501" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "100500" && l.GrantDate == DateTime.Today.AddDays(1)),
                Mock.Of<License>(l => l.Id == 1 && l.RegNumber == "цщ-100500" && l.GrantDate == DateTime.Today)
            });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == dossier);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<IParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var licenseMapper = Mock.Of<IDomainDataMapper<License>>(t => t.Retrieve(1, db) == license);
            var addin = new LinkageLicenseAddIn(taskParser, licenseMapper);

            // Act
            addin.Linkage(fileLink, db);

            // Assert
            Assert.AreEqual(fileLink.License, license);
        }

        [Test]
        public void LinkageNoLicenseTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "100501" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "100500" && l.GrantDate == DateTime.Today.AddDays(1))
            });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == dossier);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<IParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var addin = new LinkageLicenseAddIn(taskParser, Mock.Of<IDomainDataMapper<License>>());

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

            // Assert
            Assert.IsNull(fileLink.License);
        }

        [Test]
        public void LinkageLicenseToMoreLicensesTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "ЦЩ-100500" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "ЦЩ-100500" && l.GrantDate == DateTime.Today)
            });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == dossier);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<IParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var addin = new LinkageLicenseAddIn(taskParser, Mock.Of<IDomainDataMapper<License>>());

            // Assert
            Assert.Throws<TooMoreLinkagingLicesensesException>(() => addin.Linkage(fileLink, Mock.Of<IDomainDbManager>()));
        }
    }
}