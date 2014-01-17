using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.InitializerElement
{
    [TestFixture]
    public class MemberInitializerEaterTests
    {
        [Test]
        public void EatAndAddToSnapshotTest()
        {
            // Arrange
            var initializer = Mock.Of<IMemberInitializer>();
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, initializer) == Variable.None);
            var memberInitializerEater = new MemberInitializerEater(eater);

            // Act
            var result = memberInitializerEater.Eat(snapshot.Object, initializer);

            // Assert
            Assert.AreEqual(result, Variable.None);
            snapshot.Verify(t => t.AddVariable(initializer, Variable.None), Times.Once);
        }
    }
}
