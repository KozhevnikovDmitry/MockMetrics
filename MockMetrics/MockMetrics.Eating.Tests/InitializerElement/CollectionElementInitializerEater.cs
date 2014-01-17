using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.InitializerElement
{
    [TestFixture]
    public class CollectionElementInitializerEaterTests
    {
        [Test]
        public void EatArgumentAndReturnNoneTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var initializer = Mock.Of<ICollectionElementInitializer>();
            Mock.Get(initializer).SetupGet(t => t.Arguments).Returns(args);
            var argumentsEater = new Mock<IArgumentsEater>();
            var eater = Mock.Of<IEater>();
            var snapshot = Mock.Of<ISnapshot>();
            var collectionElementInitializerEater = new CollectionElementInitializerEater(eater, argumentsEater.Object);

            // Act
            var result = collectionElementInitializerEater.Eat(snapshot, initializer);

            // Assert
            Assert.AreEqual(result, Variable.None);
            argumentsEater.Verify(t =>t.Eat(snapshot, args), Times.Once);
        }
    }
}
