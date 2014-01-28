using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    [TestFixture]
    public class HolderRequisitesDataMapperTests : BaseTestFixture
    {
        #region Retrieve Tests

        [Test]
        public void RetrieveRequisitesTest()
        {
            // Arrange
            var requisites = Mock.Of<HolderRequisites>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<HolderRequisites>(1) == requisites);
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result, requisites);
        }

        [Test]
        public void RetrieveAddressTest()
        {
            // Arrange
            var requisites = Mock.Of<HolderRequisites>(t => t.AddressId == 2);
            var address = Mock.Of<Address>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<HolderRequisites>(1) == requisites
                                                 && t.RetrieveDomainObject<Address>(2) == address);

            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.Address, address);
        }

        [Test]
        public void RetrieveJuridicalTest()
        {
            // Arrange
            var requisites = Mock.Of<HolderRequisites>(t => t.JurRequisitesId == 2);
            var jurReqs = Mock.Of<JurRequisites>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<HolderRequisites>(1) == requisites
                                                 && t.RetrieveDomainObject<JurRequisites>(2) == jurReqs);
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.JurRequisites, jurReqs);
        }

        [Test]
        public void RetrieveIndividulaTest()
        {
            // Arrange
            var requisites = Mock.Of<HolderRequisites>(t => t.IndRequisitesId == 2);
            var indReqs = Mock.Of<IndRequisites>();
            var db = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<HolderRequisites>(1) == requisites
                                                 && t.RetrieveDomainObject<IndRequisites>(2) == indReqs);
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.IndRequisites, indReqs);
        }

        #endregion

        #region Save Tests

        [Test]
        public void SaveRequisitesTest()
        {
            // Arrange  
            var address = Mock.Of<Address>();
            var jurReqs = Mock.Of<JurRequisites>();
            var requisites = Mock.Of<HolderRequisites>(t => t.Address == address && t.JurRequisites == jurReqs);
            var db = new Mock<IDomainDbManager>();
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<HolderRequisites>(t => t.Clone() == requisites));

            // Assert
            Assert.AreEqual(result, requisites);
            db.Verify(t => t.SaveDomainObject(requisites), Times.Once());
        }

        [Test]
        public void SaveAddressTest()
        {
            // Arrange  
            var address = Mock.Of<Address>(t => t.Id == 100500);
            var jurReqs = Mock.Of<JurRequisites>();
            var requisites = Mock.Of<HolderRequisites>(t => t.Address == address && t.JurRequisites == jurReqs);
            var db = new Mock<IDomainDbManager>();
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<HolderRequisites>(t => t.Clone() == requisites));

            Assert.AreEqual(result.Address, address);
            Assert.AreEqual(result.AddressId, 100500);
            db.Verify(t => t.SaveDomainObject(address), Times.Once());
        }

        [Test]
        public void SaveJuridicalTest()
        {
            // Arrange  
            var address = Mock.Of<Address>();
            var jurReqs = Mock.Of<JurRequisites>(t => t.Id == 100500);
            var requisites = Mock.Of<HolderRequisites>(t => t.Address == address && t.JurRequisites == jurReqs);
            var db = new Mock<IDomainDbManager>();
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<HolderRequisites>(t => t.Clone() == requisites));

            Assert.AreEqual(result.JurRequisites, jurReqs);
            Assert.AreEqual(result.JurRequisitesId, 100500);
            db.Verify(t => t.SaveDomainObject(jurReqs), Times.Once());
        }

        [Test]
        public void SaveIndividualTest()
        {
            // Arrange  
            var address = Mock.Of<Address>();
            var indReqs = Mock.Of<IndRequisites>(t => t.Id == 100500);
            var requisites = Mock.Of<HolderRequisites>(t => t.Address == address && t.IndRequisites == indReqs);
            var db = new Mock<IDomainDbManager>();
            var mapper = new HolderRequisitesDataMapper(new StubDbCtx(db.Object));

            // Act
            var result = mapper.Save(Mock.Of<HolderRequisites>(t => t.Clone() == requisites));

            Assert.AreEqual(result.IndRequisites, indReqs);
            Assert.AreEqual(result.IndRequisitesId, 100500);
            db.Verify(t => t.SaveDomainObject(indReqs), Times.Once());
        }

        #endregion
    }
}
