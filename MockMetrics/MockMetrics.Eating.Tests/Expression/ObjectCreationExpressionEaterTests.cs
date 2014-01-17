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
            var metricHelper = Mock.Of<IMetricHelper>();
            var argsEater = new Mock<IArgumentsEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater.Object, metricHelper, eatExpressionHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot, objectCreationExpression);

            // Assert
            argsEater.Verify(t => t.Eat(snapshot, args), Times.Once);
        }

        [Test]
        public void EatMemberInitializersTest()
        {
            // Arrange
            var memberInitializer = Mock.Of<IMemberInitializer>();
            var initializer = Mock.Of<ICreationExpressionInitializer>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Initializer == initializer);
            Mock.Get(initializer).Setup(t => t.InitializerElements)
                 .Returns(new TreeNodeCollection<IInitializerElement>(new[] { memberInitializer }));
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater.Object, argsEater, metricHelper, eatExpressionHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot, objectCreationExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, memberInitializer), Times.Once());
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
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater, metricHelper, eatExpressionHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(It.IsAny<IMemberInitializer>(), It.IsAny<Variable>()), Times.Never);
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
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricsHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot.Object, type) == Variable.None);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater, metricsHelper, eatExpressionHelper);

            // Act
            var result = objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(objectCreationExpression, It.IsAny<Variable>()), Times.Never);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void AddStandaloneObjectCreationExpressionToSnapshotTest()
        {
            // Arrange
            var args = new TreeNodeCollection<ICSharpArgument>();
            var type = Mock.Of<IType>();
            var objectCreationExpression = Mock.Of<IObjectCreationExpression>(t => t.Type() == type);
            Mock.Get(objectCreationExpression).Setup(t => t.Arguments)
                .Returns(args);
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var argsEater = Mock.Of<IArgumentsEater>();
            var metricsHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot.Object, type) == Variable.None);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.IsStandaloneObjectCreationExpression(objectCreationExpression));
            var objectCreationExpressionEater = new ObjectCreationExpressionEater(eater, argsEater, metricsHelper, eatExpressionHelper);

            // Act
            objectCreationExpressionEater.Eat(snapshot.Object, objectCreationExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(objectCreationExpression, Variable.None), Times.Once);
        }
    }
}