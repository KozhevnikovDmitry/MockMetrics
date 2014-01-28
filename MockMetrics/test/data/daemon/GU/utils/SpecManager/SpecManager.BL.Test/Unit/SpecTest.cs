using System;
using System.Linq;

using Moq;

using NUnit.Framework;

using SpecManager.BL.Model;

namespace SpecManager.BL.Test.Unit
{
    [TestFixture]
    public class SpecTest
    {

        [Test]
        public void AddChildTest()
        {
            // Arrange
            var spec = new Spec { Id = 1 };
            spec.ChildSpecNodes.Add(new SpecNode());
            var childNode = new SpecNode();

            // Act
            spec.AddChild(childNode);

            // Assert
            Assert.AreEqual(spec.ChildSpecNodes.Last(), childNode);
            Assert.AreEqual(childNode.Spec, spec);
            Assert.AreEqual(childNode.SpecId, spec.Id);
            Assert.AreEqual(childNode.ParentSpecNode, null);
            Assert.AreEqual(childNode.ParentSpecNodeId, null);
        }

        [Test]
        public void AddChildWithSetParentnessTest()
        {
            // Arrange
            var spec = new Spec { Id = 1 };
            var mockChild = new Mock<SpecNode>();

            // Act
            spec.AddChild(mockChild.Object);

            // Assert
            mockChild.Verify(t => t.SetParentness(), Times.Once());
        }
    }
}
