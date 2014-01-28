using System.Collections.Generic;
using BLToolkit.EditableObjects;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DomainLogic.GuParse
{
    [TestFixture]
    public class BasicParserTests
    {
        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetJuridicalHolderNodeTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var holderNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.HasChildContentNodeEndsWith(taskNode, "OrgInfo") == true
                                                       && t.GetChildContentNodeEndsWith(taskNode, "OrgInfo") == holderNode
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == false
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseUL") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) {CallBase = true}.Object;

            // Act
            var result = parser.GetHolderNode(task);

            // Assert
            Assert.AreEqual(result, holderNode);
        }

        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetIndividualHolderNodeTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var holderNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.HasChildContentNodeEndsWith(taskNode, "OrgInfo") == false
                                                       && t.GetChildContentNodeEndsWith(taskNode, "IPInfo") == holderNode
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == true
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) { CallBase = true }.Object;

            // Act
            var result = parser.GetHolderNode(task);

            // Assert
            Assert.AreEqual(result, holderNode);
        }

        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetJuridicalLicenseNodeTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var licenseNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(taskNode, "LicenseInfo") == licenseNode
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == false
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseUL") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) { CallBase = true }.Object;

            // Act
            var result = parser.GetLicenseNode(task);

            // Assert
            Assert.AreEqual(result, licenseNode);
        }

        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetIndividualLicenseNodeTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var licenseNode = Mock.Of<ContentNode>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNode(taskNode, "LicenseInfo") == licenseNode
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == true
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) { CallBase = true }.Object;

            // Act
            var result = parser.GetLicenseNode(task);

            // Assert
            Assert.AreEqual(result, licenseNode);
        }

        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetIndividulalLicenseObjectsNodesTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var licenseObjectNodes = Mock.Of<IList<ContentNode>>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNodes(taskNode, "WorksInfo") == licenseObjectNodes
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == true
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) { CallBase = true }.Object;

            // Act
            var result = parser.GetLincenseObjectNodes(task);

            // Assert
            Assert.AreEqual(result, licenseObjectNodes);
        }

        [TestCase(LicenseActionType.New)]
        [TestCase(LicenseActionType.Renewal)]
        [TestCase(LicenseActionType.Stop)]
        [TestCase(LicenseActionType.Duplicate)]
        [TestCase(LicenseActionType.Copy)]
        public void GetJuridicalLicenseObjectsNodesTest(LicenseActionType licenseActionType)
        {
            // Arrange
            var root = Mock.Of<ContentNode>();
            var task = Mock.Of<Task>(t => t.RootContentNode == root);
            var mzTask = Mock.Of<IMzTaskWrapper>(t => t.LicenseActionType == licenseActionType);
            MzTaskContext.Current = Mock.Of<MzTaskContext>(t => t.MzTask(task) == mzTask);
            var legalChoice = Mock.Of<ContentNode>();
            var taskNode = Mock.Of<ContentNode>();
            var licenseObjectNodes = Mock.Of<IList<ContentNode>>();
            var importer = Mock.Of<IContentImporter>(t => t.GetContentNodes(taskNode, "WorksInfo") == licenseObjectNodes
                                                       && t.GetContentNode(root, "LegalChoice") == legalChoice
                                                       && t.HasContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseIP") == false
                                                       && t.GetContentNode(legalChoice, licenseActionType.GetEniseyName() + "LicenseUL") == taskNode);
            var parser = new Mock<BasicParser>(importer, Mock.Of<IContentMapper>()) { CallBase = true }.Object;

            // Act
            var result = parser.GetLincenseObjectNodes(task);

            // Assert
            Assert.AreEqual(result, licenseObjectNodes);
        }
    }
}