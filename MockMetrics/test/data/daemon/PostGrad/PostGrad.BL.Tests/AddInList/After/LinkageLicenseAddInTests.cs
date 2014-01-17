using System;
using BLToolkit.EditableObjects;
using Moq;
using NUnit.Framework;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.Licensing;
using LinkageLicenseAddIn = PostGrad.BL.AddInList.After.LinkageLicenseAddIn;

namespace PostGrad.BL.Tests.AddInList.After
{
    [TestFixture]
    public class LinkageLicenseAddInTests
    {
        [Test]
        public void SortOrderTest()
        {
            // Arrange
            var addin = new LinkageLicenseAddIn(Mock.Of<ITaskParser>());

            // Assert
            Assert.AreEqual(addin.SortOrder, 3);
        }

        [Test]
        public void NoLinkageForNewLicenseTaskTest()
        {
            // Arrange
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file);
            var addin = new LinkageLicenseAddIn(Mock.Of<ITaskParser>());

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
            var addin = new LinkageLicenseAddIn(Mock.Of<ITaskParser>());

            // Assert
            Assert.Throws<NoDossierLinkagedException>(() => addin.Linkage(fileLink, Mock.Of<IDomainDbManager>()));
        }

        [Test]
        public void LinkageLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.RegNumber == "цщ-100500" && t.GrantDate == DateTime.Today);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseList == new EditableList<License>
            {
                Mock.Of<License>(l => l.RegNumber == "100501" && l.GrantDate == DateTime.Today),
                Mock.Of<License>(l => l.RegNumber == "100500" && l.GrantDate == DateTime.Today.AddDays(1)),
                license
            });
            var task = Mock.Of<Task>();
            var file = Mock.Of<DossierFile>(t => t.IsNewLicense == false && t.Task == task);
            var fileLink = Mock.Of<IDossierFileLinkWrapper>(t => t.DossierFile == file && t.LicenseDossier == dossier);
            var licenseInfo = Mock.Of<LicenseInfo>(t => t.RegNumber == "ЦЩ-100500" && t.GrantDate == DateTime.Today);
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var addin = new LinkageLicenseAddIn(taskParser);

            // Act
            addin.Linkage(fileLink, Mock.Of<IDomainDbManager>());

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
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var addin = new LinkageLicenseAddIn(taskParser);

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
            var taskParser = Mock.Of<ITaskParser>(t => t.ParseLicenseInfo(task) == licenseInfo);
            var addin = new LinkageLicenseAddIn(taskParser);

            // Assert
            Assert.Throws<TooMoreLinkagingLicesensesException>(() => addin.Linkage(fileLink, Mock.Of<IDomainDbManager>()));
        }
    }
}