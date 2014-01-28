using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using SpecManager.BL.Exceptions;
using SpecManager.BL.Model;

namespace SpecManager.BL.Test.Unit
{
    [TestFixture]
    public class SpecNodeTest
    {
        private SpecNode ArrangeSpecNode()
        {
            var spec = ArrangeSpec();
            var parentNode = new SpecNode { Id = 1, Name = "Нода родитель" };
            var dict = new Dict { Id = 1 };
            var specNode = new SpecNode
                               {
                                   Id = 100500,
                                   SpecId = spec.Id,
                                   Spec = spec,
                                   ParentSpecNodeId = parentNode.Id,
                                   ParentSpecNode = parentNode,
                                   Name = "Нода",
                                   Tag = "Node",
                                   RefSpecUri = "ref/spec/uri",
                                   SpecNodeType = SpecNodeType.Simple,
                                   AttrDataType = AttrDataType.String,
                                   MinOccurs = 0,
                                   MaxOccurs = 10,
                                   RefSpecId = spec.Id,
                                   RefSpec = spec,
                                   FormatRegexp = "regex",
                                   FormatDescription = "описание",
                                   DictId = dict.Id,
                                   Dict = dict,
                                   IsEditableDict = true,
                                   Order = 1,
                                   Note = "примечание"
                               };
            return specNode;
        }

        private Spec ArrangeSpec()
        {
            return new Spec { Id = 1, Name = "Спека", Uri = "Ури спеки" };
        }

        #region Clone

        [Test]
        public void ClonePropertiesTest()
        {
            // Arrange
            var specNode = this.ArrangeSpecNode();

            // Act
            var clone = specNode.Clone();

            // Assert
            Assert.AreEqual(clone.Id, 0);
            Assert.AreEqual(clone.Spec, specNode.Spec);
            Assert.AreEqual(clone.SpecId, specNode.SpecId);
            Assert.AreEqual(clone.ParentSpecNodeId, specNode.ParentSpecNodeId);
            Assert.AreEqual(clone.ParentSpecNode, specNode.ParentSpecNode);
            Assert.AreEqual(clone.Name, specNode.Name);
            Assert.AreEqual(clone.Tag, specNode.Tag);
            Assert.AreEqual(clone.RefSpecUri, specNode.RefSpecUri);
            Assert.AreEqual(clone.SpecNodeType, specNode.SpecNodeType);
            Assert.AreEqual(clone.AttrDataType, specNode.AttrDataType);
            Assert.AreEqual(clone.MinOccurs, specNode.MinOccurs);
            Assert.AreEqual(clone.MaxOccurs, specNode.MaxOccurs);
            Assert.AreEqual(clone.RefSpecId, specNode.RefSpecId);
            Assert.AreEqual(clone.RefSpec, specNode.RefSpec);
            Assert.AreEqual(clone.FormatRegexp, specNode.FormatRegexp);
            Assert.AreEqual(clone.FormatDescription, specNode.FormatDescription);
            Assert.AreEqual(clone.DictId, specNode.DictId);
            Assert.AreEqual(clone.Dict, specNode.Dict);
            Assert.AreEqual(clone.IsEditableDict, specNode.IsEditableDict);
            Assert.AreEqual(clone.Order, specNode.Order);
            Assert.AreEqual(clone.Note, specNode.Note);
        }

        [Test]
        public void CloneChildrenTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var mockChild = new Mock<SpecNode>();
            mockChild.Setup(t => t.Clone()).Returns(Mock.Of<SpecNode>());
            specNode.ChildSpecNodes = new List<SpecNode>
            {
                mockChild.Object
            };

            // Act
            var clone = specNode.Clone();

            // Assert
            mockChild.Verify(t => t.Clone(), Times.Once());
        }

        [Test]
        public void CloneChildrenWithSetParenNodeTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var mockChild = new Mock<SpecNode>();
            var stubChild = Mock.Of<SpecNode>(t => t.Clone() == mockChild.Object);
            specNode.ChildSpecNodes = new List<SpecNode>
            {
                stubChild
            };

            // Act
            var clone = specNode.Clone();

            // Assert
            mockChild.VerifySet(t => t.ParentSpecNodeId = 0);
            mockChild.VerifySet(t => t.ParentSpecNode = clone);
        }

        #endregion

        #region Remove

        [Test]
        public void RemoveFromSpecTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var spec = new Spec { ChildSpecNodes = new List<SpecNode> { specNode } };
            specNode.Spec = spec;

            // Act
            specNode.Remove();

            // Assert
            Assert.False(spec.ChildSpecNodes.Contains(specNode));
        }

        [Test]
        public void RemoveFromSpecNodeTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var parentNode = new SpecNode { ChildSpecNodes = new List<SpecNode> { specNode } };
            specNode.ParentSpecNode = parentNode;

            // Act
            specNode.Remove();

            // Assert
            Assert.False(parentNode.ChildSpecNodes.Contains(specNode));
        }

        [Test]
        public void RemoveWithoutParentSpecTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var spec = new Spec { ChildSpecNodes = new List<SpecNode> { specNode } };

            // Assert
            Assert.Throws<BLLException>(() => specNode.Remove());
        }

        [Test]
        public void RemoveWithoutParentSpecNodeTest()
        {
            // Arrange
            var specNode = new SpecNode();
            var parentNode = new SpecNode { ChildSpecNodes = new List<SpecNode> { specNode } };

            // Assert
            Assert.Throws<BLLException>(() => specNode.Remove());
        }

        [Test]
        public void RemoveWithoutContainingInChildsOfSpecTest()
        {
            // Arrange
            var spec = new Spec();
            var specNode = new SpecNode { Spec = spec };

            // Assert
            Assert.Throws<BLLException>(() => specNode.Remove());
        }

        [Test]
        public void RemoveWithoutContainingInChildsOfSpecNodeTest()
        {
            // Arrange
            var parentNode = new SpecNode();
            var specNode = new SpecNode { ParentSpecNode = parentNode };

            // Assert
            Assert.Throws<BLLException>(() => specNode.Remove());
        }

        #endregion

        #region AddChild

        [Test]
        public void AddChildTest()
        {
            // Arrange
            var parentSpecNode = this.ArrangeSpecNode();
            parentSpecNode.ChildSpecNodes.Add(new SpecNode());
            var childNode = new SpecNode();

            // Act
            parentSpecNode.AddChild(childNode);

            // Assert
            Assert.AreEqual(parentSpecNode.ChildSpecNodes.Last(), childNode);
            Assert.AreEqual(childNode.Spec, parentSpecNode.Spec);
            Assert.AreEqual(childNode.SpecId, parentSpecNode.SpecId);
            Assert.AreEqual(childNode.ParentSpecNode, parentSpecNode);
            Assert.AreEqual(childNode.ParentSpecNodeId, parentSpecNode.Id);
        }

        [Test]
        public void AddChildWithSetParentnessTest()
        {
            // Arrange
            var parentNode = new SpecNode();
            var mockChild = new Mock<SpecNode>();

            // Act
            parentNode.AddChild(mockChild.Object);

            // Assert
            mockChild.Verify(t => t.SetParentness(), Times.Once());
        }

        #endregion

        #region SetParentness

        [Test]
        public void SetParentnessTest()
        {
            // Arrange
            var spec = new Spec { Id = 1 };

            var childNode = new SpecNode
            {
                Id = 2,
                ChildSpecNodes = new List<SpecNode>
                                    {
                                        new SpecNode()
                                    }
            };

            var parentNode = new SpecNode
            {
                Id = 1,
                Spec = spec,
                SpecId = spec.Id,
                ChildSpecNodes = new List<SpecNode>
                                    {
                                        childNode
                                    }
            };

            // Act
            parentNode.SetParentness();

            // Assert
            Assert.AreEqual(childNode.Spec, spec);
            Assert.AreEqual(childNode.ChildSpecNodes.First().Spec, spec);

            Assert.AreEqual(childNode.SpecId, spec.Id);
            Assert.AreEqual(childNode.ChildSpecNodes.First().SpecId, spec.Id);

            Assert.AreEqual(childNode.ParentSpecNode, parentNode);
            Assert.AreEqual(childNode.ParentSpecNodeId, parentNode.Id);

            Assert.AreEqual(childNode.ChildSpecNodes.First().ParentSpecNode, childNode);
            Assert.AreEqual(childNode.ChildSpecNodes.First().ParentSpecNodeId, childNode.Id);
        }

        #endregion
    }
}
