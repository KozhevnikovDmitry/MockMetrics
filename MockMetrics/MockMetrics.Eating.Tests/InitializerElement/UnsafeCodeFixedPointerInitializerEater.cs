using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.InitializerElement;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.InitializerElement
{
    [TestFixture]
    public class UnsafeCodeFixedPointerInitializerEaterTests
    {
        [Test]
        public void EatTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var expressionInitializer = Mock.Of<IUnsafeCodeFixedPointerInitializer>(t => t.Value == expression);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == Variable.None);
            var unsafeCodeFixedPointerInitializerEater = new UnsafeCodeFixedPointerInitializerEater(eater);

            // Act
            var result = unsafeCodeFixedPointerInitializerEater.Eat(snapshot, expressionInitializer);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}
