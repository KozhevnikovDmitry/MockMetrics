using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.InitializerElement
{
    [TestFixture]
    public class ArrayInitializerEaterTests
    {
        [Test]
        public void EatElementInitializerAndRetursLibraryTest()
        {
            // Arrange
            var elementInitializer = Mock.Of<IVariableInitializer>();
            var initializer = Mock.Of<IArrayInitializer>();
            Mock.Get(initializer)
                .SetupGet(t => t.ElementInitializers)
                .Returns(new TreeNodeCollection<IVariableInitializer>(new[] {elementInitializer}));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var arrayInitializerEater = new ArrayInitializerEater(eater.Object);

            // Act
            var result = arrayInitializerEater.Eat(snapshot, initializer);

            // Assert
            Assert.AreEqual(result, Variable.Library);
            eater.Verify(t => t.Eat(snapshot, elementInitializer), Times.Once);
        }
    }
}
