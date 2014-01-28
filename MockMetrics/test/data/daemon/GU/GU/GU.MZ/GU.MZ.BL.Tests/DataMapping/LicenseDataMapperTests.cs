using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    /// <summary>
    /// Тесты на методы класса LicenseDataMapper
    /// </summary>
    [TestFixture]
    public class LicenseDataMapperTests : BaseTestFixture
    {

        #region Test Data

        private IDomainDataMapper<LicenseObject> _stubObjectMapper;

        private IDomainDataMapper<LicenseRequisites> _stubRequisitesMapper;

        private IDomainDataMapper<LicenseDossier> _stubDossierMapper;

        private IDomainDbManager _stubDb;

        #endregion

        /// <summary>
        /// Setup before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _stubObjectMapper = Mock.Of<IDomainDataMapper<LicenseObject>>();
            _stubRequisitesMapper = Mock.Of<IDomainDataMapper<LicenseRequisites>>();
            _stubDossierMapper = Mock.Of<IDomainDataMapper<LicenseDossier>>();
            _stubDb = Mock.Of<IDomainDbManager>();
            Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == _stubDb);
        }

        #region Retrieve Tests

        [Test]
        public void RetrieveLicense()
        {
            // Arrange
            var license = Mock.Of<License>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<License>(1) == license);
            var mapper = new LicenseDataMapper(_stubObjectMapper, _stubRequisitesMapper, new StubDbCtx(db)) { LiсenseDossierMapper = _stubDossierMapper };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result, license);
        }

        [Test]
        public void RetrieveLicenseDossierTest()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.LicenseDossierId == 2);
            var dossier = Mock.Of<LicenseDossier>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<License>(1) == license
                                                    && t.RetrieveDomainObject<LicenseDossier>(2) == dossier);
            var mapper = new LicenseDataMapper(_stubObjectMapper, _stubRequisitesMapper, new StubDbCtx(db)) { LiсenseDossierMapper = _stubDossierMapper };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseDossier, dossier);
        }

        [Test]
        public void FillDossierAssociationsPerRetrieve()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.LicenseDossierId == 2);
            var dossier = Mock.Of<LicenseDossier>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<License>(1) == license
                                                    && t.RetrieveDomainObject<LicenseDossier>(2) == dossier);
            var mockDossierMapper = new Mock<IDomainDataMapper<LicenseDossier>>();
            var mapper = new LicenseDataMapper(_stubObjectMapper, _stubRequisitesMapper, new StubDbCtx(db)) { LiсenseDossierMapper = mockDossierMapper.Object };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            mockDossierMapper.Verify(t => t.FillAssociations(dossier, db), Times.Once());
        }

        [Test]
        public void RetriveLicenseObjectListTest()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.Id == 1);
            var licenseObject = Mock.Of<LicenseObject>(t => t.Id == 3 && t.LicenseId == 1);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<License>(1) == license
                                                    && t.GetDomainTable<LicenseObject>() == new List<LicenseObject>
                                                        {
                                                            licenseObject,
                                                            Mock.Of<LicenseObject>(),
                                                        }.AsQueryable());

            var objectMapper = Mock.Of<IDomainDataMapper<LicenseObject>>(t => t.Retrieve(3, db) == licenseObject);

            var mapper = new LicenseDataMapper(objectMapper, _stubRequisitesMapper,
                                               new StubDbCtx(db)) { LiсenseDossierMapper = _stubDossierMapper };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseObjectList.Single(), licenseObject);
        }


        [Test]
        public void RetriveLicenseRequisitesListTest()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.Id == 1);
            var licenseRequisites = Mock.Of<LicenseRequisites>(t => t.Id == 3 && t.LicenseId == 1);
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<License>(1) == license
                                                    && t.GetDomainTable<LicenseRequisites>() == new List<LicenseRequisites>
                                                        {
                                                            licenseRequisites,
                                                            Mock.Of<LicenseRequisites>(),
                                                        }.AsQueryable());

            var requisitesMapper = Mock.Of<IDomainDataMapper<LicenseRequisites>>(t => t.Retrieve(3, db) == licenseRequisites);

            var mapper = new LicenseDataMapper(_stubObjectMapper, requisitesMapper,
                                               new StubDbCtx(db)) { LiсenseDossierMapper = _stubDossierMapper };

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.LicenseRequisitesList.Single(), licenseRequisites);
        }


        #endregion

        #region Save Tests

        [Test]
        public void SaveLicenseTest()
        {
            // Arrange
            var license = Mock.Of<License>(t => t.LicenseObjectList == new EditableList<LicenseObject>());
            var db = new Mock<IDomainDbManager>();
            var mapper = new LicenseDataMapper(_stubObjectMapper, _stubRequisitesMapper, new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<License>(t => t.Clone() == license));

            // Assert
            db.Verify(t => t.SaveDomainObject(license));
            Assert.AreEqual(license, result);
        }

        [Test]
        public void SaveLicenseObjectListTest()
        {
            // Arrange
            var licenseObject = Mock.Of<LicenseObject>();
            var savedObject = Mock.Of<LicenseObject>();
            var license = Mock.Of<License>(t => t.Id == 100500 && t.LicenseObjectList == new EditableList<LicenseObject> { licenseObject });

            var db = new Mock<IDomainDbManager>();

            var objectMapper =
                Mock.Of<IDomainDataMapper<LicenseObject>>(d => d.Save(It.Is<LicenseObject>(t => t.LicenseId == 100500 && t.Equals(licenseObject)), db.Object, false) == savedObject);

            var mapper = new LicenseDataMapper(objectMapper, _stubRequisitesMapper,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<License>(t => t.Clone() == license));

            // Assert
            Assert.AreEqual(result.LicenseObjectList.Single(), savedObject);
        }
        
        [Test]
        public void SaveRequisistesListTest()
        {
            // Arrange
            var licenseRequisites = Mock.Of<LicenseRequisites>();
            var savedRequisites = Mock.Of<LicenseRequisites>();
            var license = Mock.Of<License>(t => t.Id == 100500 && t.LicenseRequisitesList == new EditableList<LicenseRequisites> { licenseRequisites });

            var db = new Mock<IDomainDbManager>();

            var requisitesMapper =
                Mock.Of<IDomainDataMapper<LicenseRequisites>>(d => d.Save(It.Is<LicenseRequisites>(t => t.LicenseId == 100500 && t.Equals(licenseRequisites)), db.Object, false) == savedRequisites);

            var mapper = new LicenseDataMapper(_stubObjectMapper, requisitesMapper,
                                                   new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<License>(t => t.Clone() == license));

            // Assert
            Assert.AreEqual(result.LicenseRequisitesList.Single(), savedRequisites);
        }

        [Test]
        public void DeleteLicenseObjectsTest()
        {
            // Arrange
            var licesnseObject = Mock.Of<LicenseObject>();
            var objectList = new EditableList<LicenseObject> { licesnseObject };
            objectList.AcceptChanges();
            objectList.Remove(licesnseObject);

            var license = Mock.Of<License>(t => t.LicenseObjectList == objectList);
            var db = Mock.Of<IDomainDbManager>();

            var objectMapper = new Mock<IDomainDataMapper<LicenseObject>>();
            var mapper = new LicenseDataMapper(objectMapper.Object, _stubRequisitesMapper, new StubDbCtx(db));

            // Act
            mapper.Save(Mock.Of<License>(t => t.Clone() == license));

            // Assert
            objectMapper.Verify(t => t.Delete(licesnseObject, db), Times.Once());
        }
        
        [Test]
        public void DeleteREquisistesTest()
        {
            // Arrange
            var licenseRequisites = Mock.Of<LicenseRequisites>();
            var licenseRequisitesList = new EditableList<LicenseRequisites> { licenseRequisites };
            licenseRequisitesList.AcceptChanges();
            licenseRequisitesList.Remove(licenseRequisites);

            var license = Mock.Of<License>(t => t.LicenseRequisitesList == licenseRequisitesList);
            var db = Mock.Of<IDomainDbManager>();

            var requistesMapper = new Mock<IDomainDataMapper<LicenseRequisites>>();
            var mapper = new LicenseDataMapper(_stubObjectMapper, requistesMapper.Object, new StubDbCtx(db));

            // Act
            mapper.Save(Mock.Of<License>(t => t.Clone() == license));

            // Assert
            requistesMapper.Verify(t => t.Delete(licenseRequisites, db), Times.Once());
        }

        #endregion

        #region FillAssociations Tests

        [Test]
        public void LoadActualRequisitesTest()
        {
            // Arrange
            var license = License.CreateInstance();
            license.Id = 1;
            var requisites = Mock.Of<LicenseRequisites>(t => t.Id == 2 && t.LicenseId == 1 && t.CreateDate == DateTime.Now);
            var db = Mock.Of<IDomainDbManager>(t => t.GetDomainTable<LicenseRequisites>() == new List<LicenseRequisites>
                {
                    requisites,
                    Mock.Of<LicenseRequisites>(r => r.LicenseId == 1 && r.CreateDate == DateTime.Now.AddDays(-1)),
                    Mock.Of<LicenseRequisites>(r => r.LicenseId == 2 && r.CreateDate == DateTime.Now.AddDays(+1))
                }.AsQueryable());

            var requisitesMapper = Mock.Of<IDomainDataMapper<LicenseRequisites>>(t => t.Retrieve(2, db) == requisites);
            var mapper = new LicenseDataMapper(_stubObjectMapper, requisitesMapper, new StubDbCtx(db));

            // Act
            mapper.FillAssociations(license);

            // Assert
            Assert.AreEqual(license.ActualRequisites, requisites);
        }

        #endregion


    }
}
