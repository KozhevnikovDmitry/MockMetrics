using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Holder;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class ContentMapperTests
    {
        [TestCase("LAWform", 1)]
        [TestCase("WRONG", 0)]
        public void MapJuridicalRequisitesTest(string legalFormName, int legalFormIdResult)
        {

            // Arrange
            var holderNode = Mock.Of<ContentNode>(t => t.SpecNode.Tag == "OrgInfo");
            var importer = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(It.IsAny<ContentNode>(), It.IsAny<string>()) == "O_o"
                                                       && t.GetNodeStrValue(holderNode, "FullName") == "FullName"
                                                       && t.GetNodeStrValue(holderNode, "ShortName") == "ShortName"
                                                       && t.GetNodeStrValue(holderNode, "FirmName") == "FirmName"
                                                       && t.GetNodeStrValue(holderNode, "LawForm") == "LawForm");

            var dictManager = Mock.Of<IDictionaryManager>(t => t.GetDictionary<LegalForm>() ==
                                                               new List<LegalForm>
                                                               {
                                                                   Mock.Of<LegalForm>(l => l.Name == "" && l.Id == 0),
                                                                   Mock.Of<LegalForm>(l => l.Name == legalFormName && l.Id == 1)
                                                               });

            var loader = new ContentMapper(importer, dictManager);

            // Act
            var holderRequisites = loader.MapRequisites(holderNode);

            // Assert
            Assert.AreEqual(holderRequisites.JurRequisites.FullName, "FullName");
            Assert.AreEqual(holderRequisites.JurRequisites.ShortName, "ShortName");
            Assert.AreEqual(holderRequisites.JurRequisites.FirmName, "FirmName");
            Assert.AreEqual(holderRequisites.JurRequisites.LegalFormId, legalFormIdResult);
            Assert.IsNull(holderRequisites.IndRequisites);
        }

        [Test]
        public void MapJuridicalRequisitesMapChiefTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>(t => t.SpecNode.Tag == "OrgInfo");
            var chiefNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(It.IsAny<ContentNode>(), It.IsAny<string>()) == "O_o"
                                                       && t.GetContentNode(holderNode, "Director") == chiefNode
                                                       && t.GetNodeStrValue(chiefNode, "Position") == "Position"
                                                       && t.GetNodeStrValue(chiefNode, "ChiefLastName") == "ChiefLastName"
                                                       && t.GetNodeStrValue(chiefNode, "ChiefFirstName") == "ABC"
                                                       && t.GetNodeStrValue(chiefNode, "ChiefMiddleName") == "XYZ");


            var dictManager = Mock.Of<IDictionaryManager>(t => t.GetDictionary<LegalForm>() ==
                                                               new List<LegalForm>
                                                               {
                                                                   Mock.Of<LegalForm>(l => l.Name == "O_o" && l.Id == 1)
                                                               });
            var loader = new ContentMapper(importer, dictManager);

            // Act
            var holderRequisites = loader.MapRequisites(holderNode);

            // Assert
            Assert.AreEqual(holderRequisites.JurRequisites.HeadPositionName, "Position");
            Assert.AreEqual(holderRequisites.JurRequisites.HeadName, "ChiefLastName A.X.");
        }
        
        [Test]
        public void MapIndividualRequisitesTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>(t => t.SpecNode.Tag == "IPInfo");
            var documentNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(holderNode, "Document") == documentNode
                                                       && t.GetNodeStrValue(It.IsAny<ContentNode>(), It.IsAny<string>()) == "O_o"
                                                       && t.GetNodeStrValue(holderNode, "FamilyName") == "FamilyName"
                                                       && t.GetNodeStrValue(holderNode, "FirstName") == "FirstName"
                                                       && t.GetNodeStrValue(holderNode, "Patronymic") == "Patronymic"
                                                       && t.GetNodeStrValue(documentNode, "Series") == "Series"
                                                       && t.GetNodeStrValue(documentNode, "Number") == "Number"
                                                       && t.GetNodeDateValue(documentNode, "IssueDate") == DateTime.Today
                                                       && t.GetNodeStrValue(documentNode, "Issuer") == "Issuer");

            var loader = new ContentMapper(importer, Mock.Of<IDictionaryManager>());

            // Act
            var holderRequisites = loader.MapRequisites(holderNode);

            // Assert
            Assert.AreEqual(holderRequisites.IndRequisites.Surname, "FamilyName");
            Assert.AreEqual(holderRequisites.IndRequisites.Name, "FirstName");
            Assert.AreEqual(holderRequisites.IndRequisites.Patronymic, "Patronymic");
            Assert.AreEqual(holderRequisites.IndRequisites.Serial, "Series");
            Assert.AreEqual(holderRequisites.IndRequisites.Number, "Number");
            Assert.AreEqual(holderRequisites.IndRequisites.Stamp, DateTime.Today);
            Assert.AreEqual(holderRequisites.IndRequisites.Organization, "Issuer");
            Assert.IsNull(holderRequisites.JurRequisites);
        }

        [Test]
        public void MapAddressTest()
        {
            // Arrange
            var addressNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(addressNode, "PostalCode") == "PostalCode"
                                                       && t.GetNodeStrValue(addressNode, "Region") == "Region"
                                                       && t.GetNodeStrValue(addressNode, "District") == "District"
                                                       && t.GetNodeStrValue(addressNode, "CityLocality") == "CityLocality"
                                                       && t.GetNodeStrValue(addressNode, "StreetText") == "StreetText"
                                                       && t.GetNodeStrValue(addressNode, "ApartmentBlock") == "ApartmentBlock"
                                                       && t.GetNodeStrValue(addressNode, "Apartment") == "Apartment"
                                                       && t.GetNodeStrValue(addressNode, "Building") == "Building");
            var mapper = new ContentMapper(importer, Mock.Of<IDictionaryManager>());

            // Act
            var address = mapper.MapAddress(addressNode);

            // Assert
            Assert.AreEqual(address.Zip, "PostalCode");
            Assert.AreEqual(address.CountryRegion, "Region");
            Assert.AreEqual(address.Area, "District");
            Assert.AreEqual(address.City, "CityLocality");
            Assert.AreEqual(address.Street, "StreetText");
            Assert.AreEqual(address.House, "ApartmentBlock");
            Assert.AreEqual(address.Flat, "Apartment");
            Assert.AreEqual(address.Build, "Building");
        }

        [Test]
        public void MapHolderInfoFromSimpleInnOgrnNodesTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>();
            var innNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.Simple && t.StrValue == "INN");
            var ogrnNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.Simple && t.StrValue == "OGRN");
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNode(holderNode, "INN") == innNode
                                                              && t.GetContentNode(holderNode, "OGRN") == ogrnNode
                                                              && t.GetNodeStrValue(holderNode, "FullName") == "FullName"
                                                              && t.HasContentNode(holderNode, "FullName") == true);

            var contentMapper = new ContentMapper(contentImporter, Mock.Of<IDictionaryManager>());

            // Act
            var holderInfo = contentMapper.MapHolderInfo(holderNode);

            // Assert
            Assert.AreEqual(holderInfo.Inn, "INN");
            Assert.AreEqual(holderInfo.Ogrn, "OGRN");
            Assert.AreEqual(holderInfo.FullName, "FullName");
        }
        [Test]
        public void MapHolderInfoFromRefInnOgrnNodesTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>();
            var innRefNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.RefSpec);
            var ogrnRefNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.RefSpec);
            var innNode = Mock.Of<ContentNode>(t => t.StrValue == "INN");
            var ogrnNode = Mock.Of<ContentNode>(t => t.StrValue == "OGRN");
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNode(holderNode, "INN") == innRefNode
                                                              && t.GetContentNode(holderNode, "OGRN") == ogrnRefNode
                                                              && t.GetChildContentNodeStartsWith(innRefNode, "INN") == innNode
                                                              && t.GetChildContentNodeStartsWith(ogrnRefNode, "OGRN") == ogrnNode
                                                              && t.GetNodeStrValue(holderNode, "FullName") == "FullName"
                                                              && t.HasContentNode(holderNode, "FullName") == true);

            var contentMapper = new ContentMapper(contentImporter, Mock.Of<IDictionaryManager>());

            // Act
            var holderInfo = contentMapper.MapHolderInfo(holderNode);

            // Assert
            Assert.AreEqual(holderInfo.Inn, "INN");
            Assert.AreEqual(holderInfo.Ogrn, "OGRN");
            Assert.AreEqual(holderInfo.FullName, "FullName");
        }



        [Test]
        public void MapHolderFromSimpleInnOgrnNodesTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>();
            var innNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.Simple && t.StrValue == "INN");
            var ogrnNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.Simple && t.StrValue == "OGRN");
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNode(holderNode, "INN") == innNode
                                                              && t.GetContentNode(holderNode, "OGRN") == ogrnNode
                                                              && t.GetNodeStrValue(holderNode, "FullName") == "FullName");

            var contentMapper = new ContentMapper(contentImporter, Mock.Of<IDictionaryManager>());

            // Act
            var holderInfo = contentMapper.MapHolder(holderNode);

            // Assert
            Assert.AreEqual(holderInfo.Inn, "INN");
            Assert.AreEqual(holderInfo.Ogrn, "OGRN");
        }
        [Test]
        public void MapHolderFromRefInnOgrnNodesTest()
        {
            // Arrange
            var holderNode = Mock.Of<ContentNode>();
            var innRefNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.RefSpec);
            var ogrnRefNode = Mock.Of<ContentNode>(t => t.SpecNode.SpecNodeType == SpecNodeType.RefSpec);
            var innNode = Mock.Of<ContentNode>(t => t.StrValue == "INN");
            var ogrnNode = Mock.Of<ContentNode>(t => t.StrValue == "OGRN");
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetContentNode(holderNode, "INN") == innRefNode
                                                              && t.GetContentNode(holderNode, "OGRN") == ogrnRefNode
                                                              && t.GetChildContentNodeStartsWith(innRefNode, "INN") == innNode
                                                              && t.GetChildContentNodeStartsWith(ogrnRefNode, "OGRN") == ogrnNode
                                                              && t.GetNodeStrValue(holderNode, "FullName") == "FullName");

            var contentMapper = new ContentMapper(contentImporter, Mock.Of<IDictionaryManager>());

            // Act
            var holderInfo = contentMapper.MapHolder(holderNode);

            // Assert
            Assert.AreEqual(holderInfo.Inn, "INN");
            Assert.AreEqual(holderInfo.Ogrn, "OGRN");
        }

        [Test]
        public void MapLicenseInfoTest()
        {
            // Arrange
            var licenseNode = Mock.Of<ContentNode>();
            var contentImporter = Mock.Of<IContentImporter>(t => t.GetNodeStrValue(licenseNode, "LicenseNumber") == "RegNumber" &&
                                                          t.GetNodeDateValue(licenseNode, "LicenseDate") == DateTime.Today &&
                                                          t.GetNodeStrValue(licenseNode, "LicenseOrg") == "Licensiar");

            var contentMapper = new ContentMapper(contentImporter, Mock.Of<IDictionaryManager>());

            // Act
            var licenseInfo = contentMapper.MapLicenseInfo(licenseNode);

            // Assert
            Assert.AreEqual(licenseInfo.RegNumber, "RegNumber");
            Assert.AreEqual(licenseInfo.GrantDate, DateTime.Today);
            Assert.AreEqual(licenseInfo.Licensiar, "Licensiar");
        }
    }
}