using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ObjectCreationExpressionEaterTests
    {
        [Test]
        public void EatArgumentsTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>();
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(args); 
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater.Object, Mock.Of<IMetricHelper>());

            // Act
            objectCreationExpressionEater.Eat(snapshot, objectCreationExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatMemberInitializersTest()
        {
            // Arrange
            var expression = Mock.Of<ICSharpExpression>();
            var memberInitializer = Mock.Of<IMemberInitializer>(t => t.Expression == expression);
            var initializer = Mock.Of<ICreationExpressionInitializer>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Initializer == initializer);
            Mock.Get(initializer).Setup(t => t.InitializerElements)
                 .Returns(new TreeNodeCollection<IInitializerElement>(new[] { memberInitializer }));
            var snapshot = new Mock<ISnapshot>();
            var initialMetrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, expression) == initialMetrics);
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater, metricHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddOperand(memberInitializer, It.Is<Metrics>(m => m.Equals(initialMetrics) && m.Scope == Scope.Local)), Times.Once());
        }
       
        [Test]
        public void NotAddMemberInitializersToSnapshotTest()
        {
            // Arrange
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>();
            Mock.Get(objectCreationExpression)
                .Setup(t => t.Initializer.InitializerElements)
                .Returns(new TreeNodeCollection<IInitializerElement>(new IInitializerElement[0]));
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater, metricHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddOperand(It.IsAny<IMemberInitializer>(), It.IsAny<Metrics>()), Times.Never);
        }

        [Test]
        public void ReturnMetricsForConstructedTypeTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var type = Mock.Of<IType>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type() == type);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = new Mock<IArgumentsEater>();
            var typeMetrics = Metrics.Create();
            var metricsHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, type) == typeMetrics);
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater.Object, metricsHelper);

            // Act
            var result = objectCreationExpressionEater.Eat(snapshot, objectCreationExpression);

            // Assert
            Assert.AreEqual(result, typeMetrics);
        }
    }
}