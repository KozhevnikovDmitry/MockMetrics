using System;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Linkage.LinkageException;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.Linkage
{
    [TestFixture]
    public class DossierFileLinkWrapperTests
    {
        [Test]
        public void NullDossierFileTest()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new DossierFileLinkWrapper(null));
        }

        [Test]
        public void LinkageNullHolderTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>());

            // Assert
            Assert.Throws<NoHolderLinkagedException>(() => wrapper.Linkage());
        }

        [Test]
        public void LinkageNullDossierTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>()
            };

            // Assert
            Assert.Throws<NoDossierLinkagedException>(() => wrapper.Linkage());
        }
        
        [Test]
        public void LinkageUnavailableRequisitesTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(),
                LicenseDossier = Mock.Of<LicenseDossier>()
            };

            // Assert
            Assert.Throws<UnavailableRequisitesException>(() => wrapper.Linkage());
        }

        [Test]
        public void LinkageDossierTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(),
                LicenseDossier = Mock.Of<LicenseDossier>(t => t.Id == 1),
                SelectedRequisites = Mock.Of<HolderRequisites>()
            };
            
            // Act
            wrapper.Linkage();
            var dossierFile = wrapper.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.LicenseDossier, wrapper.LicenseDossier);
            Assert.AreEqual(dossierFile.LicenseDossierId, 1);
        }

        [Test]
        public void LinkageRequisitesTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(),
                LicenseDossier = Mock.Of<LicenseDossier>(),
                SelectedRequisites =  Mock.Of<HolderRequisites>(t => t.Id == 1)
            };

            // Act
            wrapper.Linkage();
            var dossierFile = wrapper.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.HolderRequisites, wrapper.SelectedRequisites);
            Assert.AreEqual(dossierFile.HolderRequisitesId, 1);
        }

        [Test]
        public void LinkageHolderTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(t => t.Id == 1),
                LicenseDossier = Mock.Of<LicenseDossier>(),
                SelectedRequisites = Mock.Of<HolderRequisites>()
            };

            // Act
            wrapper.Linkage();
            var dossierFile = wrapper.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolder, wrapper.LicenseHolder);
            Assert.AreEqual(dossierFile.LicenseDossier.LicenseHolderId, 1);

            Assert.AreEqual(dossierFile.HolderRequisites.LicenseHolder, wrapper.LicenseHolder);
            Assert.AreEqual(dossierFile.HolderRequisites.LicenseHolderId, 1);
        }

        [Test]
        public void LinkageNoLicenseTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(),
                LicenseDossier = Mock.Of<LicenseDossier>(),
                SelectedRequisites = Mock.Of<HolderRequisites>()
            };

            // Act
            wrapper.Linkage();
            var dossierFile = wrapper.DossierFile;

            // Assert
            Assert.IsNull(dossierFile.License);
            Assert.IsNull(dossierFile.LicenseId);
        }

        [Test]
        public void LinkageLicenseTest()
        {
            // Arrange
            var wrapper = new DossierFileLinkWrapper(Mock.Of<DossierFile>())
            {
                LicenseHolder = Mock.Of<LicenseHolder>(),
                LicenseDossier = Mock.Of<LicenseDossier>(),
                License = Mock.Of<License>(t => t.Id == 1),
                SelectedRequisites = Mock.Of<HolderRequisites>()
            };

            // Act
            wrapper.Linkage();
            var dossierFile = wrapper.DossierFile;

            // Assert
            Assert.AreEqual(dossierFile.License, wrapper.License);
            Assert.AreEqual(dossierFile.LicenseId, 1);
        }

    }
}