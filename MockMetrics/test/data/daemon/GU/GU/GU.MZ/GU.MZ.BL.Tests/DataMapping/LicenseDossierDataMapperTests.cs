using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    public class LicenseDossierDataMapperTests : BaseTestFixture
    {
        #region TestData

        private IDomainDataMapper<LicenseHolder> _holderMapper;

        #endregion

        [SetUp]
        public void Setup()
        {
            _holderMapper = Mock.Of<IDomainDataMapper<LicenseHolder>>();
            Mock.Of<IDomainDataMapper<DossierFile>>();
        }

        [Test]
        public void RetrieveLicenseDossierTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<LicenseDossier>(1) == dossier);

            var mapper = new LicenseDossierDataMapper(_holderMapper,
                                                      new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result, dossier);
        }

        [Test]
        public void RetrieveLicenseHolderTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseHolderId == 2);
            var holder = Mock.Of<LicenseHolder>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<LicenseDossier>(1) == dossier);

            var mapper = new LicenseDossierDataMapper(Mock.Of<IDomainDataMapper<LicenseHolder>>(t => t.Retrieve(2, db) == holder),
                                                      new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseHolder, holder);
        }

        [Test]
        public void RetriveLicenseListTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>(t => t.Id == 1  && t.LicensedActivityId == 2);
            var license = Mock.Of<License>(t => t.Id == 3 && t.LicensedActivityId == 2 && t.LicenseDossierId == 1);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<LicenseDossier>(1) == dossier
                                                 && t.GetDomainTable<License>() == new List<License>
                                                     {
                                                         Mock.Of<License>(l => l.Id == 3 && l.LicensedActivityId == 2 && l.LicenseDossierId == 1),
                                                         Mock.Of<License>(l => l.LicenseDossierId == 2 && l.LicensedActivityId == 2),
                                                         Mock.Of<License>(l => l.LicensedActivityId == 1 && l.LicenseDossierId == 1)
                                                     }.AsQueryable());
            var licenseMapper = Mock.Of<IDomainDataMapper<License>>(t => t.Retrieve(3, db) == license);

            var mapper = new LicenseDossierDataMapper(_holderMapper,
                new StubDbCtx(db))
            {
                LicenseMapper = licenseMapper
            };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseList.Single(), license);
        }

        [Test]
        public void RetriveDossierFileListTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>(t => t.Id == 1);
            var file = Mock.Of<DossierFile>(t => t.Id == 2 && t.LicenseDossierId == 1);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<LicenseDossier>(1) == dossier
                                                 && t.GetDomainTable<DossierFile>() == new List<DossierFile>
                                                     {
                                                         file,
                                                         Mock.Of<DossierFile>()
                                                     }.AsQueryable());


            var mapper = new LicenseDossierDataMapper(_holderMapper,
                                                      new StubDbCtx(db))
                {
                    DossierFileMapper = Mock.Of<IDomainDataMapper<DossierFile>>(t => t.Retrieve(2, db) == file)
                };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.DossierFileList.Single(), file);
        }

        [Test]
        public void SaveLicenseDossierTest()
        {
            // Arrange
            var dossier = Mock.Of<LicenseDossier>();
            var db = new Mock<IDomainDbManager>();

            var mapper = new LicenseDossierDataMapper(Mock.Of<IDomainDataMapper<LicenseHolder>>(d => d.Save(It.IsAny<LicenseHolder>(), db.Object, false) == Mock.Of<LicenseHolder>()),
                                                      Mock.Of<IDomainContext>(
                                                          t => t.GetDbManager(It.IsAny<string>()) == db.Object));

            // Act
            var result = mapper.Save(Mock.Of<LicenseDossier>(t => t.Clone() == dossier));

            // Assert
            db.Verify(t => t.SaveDomainObject(dossier));
            Assert.AreEqual(dossier, result);
        }

        [Test]
        public void SaveLicenseHolderTest()
        {
            // Arrange
            var holder = Mock.Of<LicenseHolder>();
            var savedHolder = Mock.Of<LicenseHolder>(t => t.Id == 100500);
            var dossier = Mock.Of<LicenseDossier>(t => t.LicenseHolder == holder);

            var db = new Mock<IDomainDbManager>();
            var holderMapper =
                Mock.Of<IDomainDataMapper<LicenseHolder>>(d => d.Save(holder, db.Object, false) == savedHolder);

            var mapper = new LicenseDossierDataMapper(holderMapper,
                                                      Mock.Of<IDomainContext>(
                                                          t => t.GetDbManager(It.IsAny<string>()) == db.Object));

            // Act
            var result = mapper.Save(Mock.Of<LicenseDossier>(t => t.Clone() == dossier));

            // Assert
            Assert.AreEqual(result.LicenseHolderId, 100500);
            Assert.AreEqual(result.LicenseHolder, savedHolder);
        }
    }
}
