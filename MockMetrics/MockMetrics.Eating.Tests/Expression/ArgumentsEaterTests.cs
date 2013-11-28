using System;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ArgumentsEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var mockRep = new MockRepository(MockBehavior.Default);
            var args = new  TreeNodeCollection<ICSharpArgument>(mockRep.Of<ICSharpArgument>().Where(t => t.Value == expression).Take(10).ToArray());
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var argumentsEater = new ArgumentsEater(eater.Object);

            // Act
            argumentsEater.Eat(snapshot, args);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression), Times.Exactly(10));
        }

        [Test]
        public void NullSnapshotTest()
        {
            // Arrange
            var argumentsEater = new ArgumentsEater(Mock.Of<IEater>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => argumentsEater.Eat(null, new TreeNodeCollection<ICSharpArgument>(new ICSharpArgument[0])));
        }
    }
}
