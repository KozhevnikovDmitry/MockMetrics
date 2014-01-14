using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ArrayCreationExpressionEaterTests
    {
        [Test]
        public void EatSizesTest()
        {
            // Arrange
            var size = Mock.Of<ICSharpExpression>();
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>();
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { size }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater.Object, Mock.Of<IVariableInitializerEater>());

            // Act
            arrayCreationExpressionEater.Eat(snapshot, arrayCreationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, size));
        }

        [Test]
        public void EatArrayInitializerTest()
        {
            // Arrange
            var initializer = Mock.Of<IArrayInitializer>();
            var arrayCreationExpression = Mock.Of<IArrayCreationExpression>(t => t.ArrayInitializer == initializer);
            Mock.Get(arrayCreationExpression).Setup(t => t.Sizes)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var initilizerEater = Mock.Of<IVariableInitializerEater>(t => t.Eat(snapshot, initializer) == Variable.None);
            var arrayCreationExpressionEater = new ArrayCreationExpressionEater(eater, initilizerEater);

            // Act
            var result = arrayCreationExpressionEater.Eat(snapshot, arrayCreationExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }
    }
}