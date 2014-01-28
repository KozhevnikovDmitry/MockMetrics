using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;
using GU.MZ.BL.DataMapping;
using GU.MZ.DataModel.Dossier;
using Moq;
using NUnit.Framework;

namespace GU.MZ.BL.Tests.DataMapping
{
    [TestFixture]
    public class InventoryDataMapperTests
    {
        [Test]
        public void RetrieveDocumentInventoryTest()
        {
            // Arrange
            var inventory = Mock.Of<DocumentInventory>();
            var stubDb = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DocumentInventory>(1) == inventory);
            var mapper =
                new InventoryDataMapper(Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == stubDb));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result, inventory);
        }

        [Test]
        public void RetrieveProvidedDocumentListTest()
        {
            // Arrange
            var document = Mock.Of<ProvidedDocument>();
            var inventory = Mock.Of<DocumentInventory>(t => t.Id == 1);
            var stubDb = Mock.Of<IDomainDbManager>(t => t.RetrieveDomainObject<DocumentInventory>(1) == inventory
                                                     && t.GetDomainTable<ProvidedDocument>() == new List<ProvidedDocument>
                                                                                                    {
                                                                                                        Mock.Of<ProvidedDocument>(p => p.Id == 100500 && p.DocumentInventoryId == 1),
                                                                                                        Mock.Of<ProvidedDocument>(p => p.DocumentInventoryId == 2)
                                                                                                    }.AsQueryable()
                                                     && t.RetrieveDomainObject<ProvidedDocument>(100500) == document);
            var mapper =
                new InventoryDataMapper(Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == stubDb));

            // Act
            var result = mapper.Retrieve(1);

            // Assert
            Assert.AreEqual(result.ProvidedDocumentList.Single(), document);
        }

        [Test]
        public void SaveDocumentInventoryTest()
        {
            // Arrange
            var inventory = Mock.Of<DocumentInventory>();
            var mockDb = new Mock<IDomainDbManager>(); 
            var mapper =
                new InventoryDataMapper(Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == mockDb.Object));

            // Act
            var result = mapper.Save(Mock.Of<DocumentInventory>(t => t.Clone() == inventory));

            // Assert
            mockDb.Verify(t => t.SaveDomainObject(inventory), Times.Once());
            Assert.AreEqual(result, inventory);
        }

        [Test]
        public void SaveProvidedDocumentListTest()
        {
            // Arrange
            var document1 = Mock.Of<ProvidedDocument>();
            var document2 = Mock.Of<ProvidedDocument>();
            var inventory = Mock.Of<DocumentInventory>(t => t.Id == 1 && t.ProvidedDocumentList == new EditableList<ProvidedDocument> { document1, document2 });
            var mockDb = new Mock<IDomainDbManager>(); 
            var mapper =
                new InventoryDataMapper(Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == mockDb.Object));
            
            // Act
            var result = mapper.Save(Mock.Of<DocumentInventory>(t => t.Clone() == inventory && t.Id == 1));

            // Assert
            mockDb.Verify(t => t.SaveDomainObject(document1), Times.Once());
            mockDb.Verify(t => t.SaveDomainObject(document2), Times.Once());
            Assert.AreEqual(result.ProvidedDocumentList.First().DocumentInventoryId, 1);
            Assert.AreEqual(result.ProvidedDocumentList.Last().DocumentInventoryId, 1);
        }

        [Test]
        public void DeleteExcludedProvidedDocuemtnPerSaveTest()
        {
            // Arrange
            var stubDoc = Mock.Of<ProvidedDocument>();
            var doc = ProvidedDocument.CreateInstance();
            var docList = new EditableList<ProvidedDocument> { stubDoc, doc };
            docList.AcceptChanges();
            docList.Remove(doc);

            var inventory = Mock.Of<DocumentInventory>(t => t.Id == 1 && t.ProvidedDocumentList == docList);
            var mockDb = new Mock<IDomainDbManager>();
            var mapper =
                new InventoryDataMapper(Mock.Of<IDomainContext>(t => t.GetDbManager(It.IsAny<string>()) == mockDb.Object));

            // Act
            mapper.Save(Mock.Of<DocumentInventory>(t => t.Clone() == inventory));

            // Assert
            mockDb.Verify(t => t.SaveDomainObject(doc), Times.Once());
            Assert.AreEqual(doc.PersistentState, PersistentState.NewDeleted);
        }
    }
}