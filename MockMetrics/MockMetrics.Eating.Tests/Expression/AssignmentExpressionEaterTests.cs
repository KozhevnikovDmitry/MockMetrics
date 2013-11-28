using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AssignmentExpressionEaterTests
    {
        [Test]
        public void EatAssignmentSourceExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = new Mock<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IVariableDeclarationAndIDeclaredElement>());
            var metricHelper = Mock.Of<IMetricHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater.Object, eatExpressionHelper, metricHelper);

            // Act
            assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, source));
        }

        [Test]
        public void EatEventAssigmentDestTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var resultMetrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, dest) == resultMetrics);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IEventDeclarationAndIDeclaredElement>());
            var metricHelper = Mock.Of<IMetricHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, metricHelper);

            // Act
            var result = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.That(result, Is.EqualTo(resultMetrics));
        }

        [Test]
        public void EatVariableAssignmentTest()
        {
            // Arrange
            var sourceMetrics = Metrics.Create();
            var destMetrics = Metrics.Create(); 
            var mergeMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, source) == sourceMetrics
                                          && t.Eat(snapshot, dest) == destMetrics);
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IVariableDeclarationAndIDeclaredElement>());
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(destMetrics, sourceMetrics) == mergeMetrics);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, metricHelper);

            // Act
            var result = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            Assert.That(result, Is.EqualTo(mergeMetrics));
        }

        [Test]
        public void UnexpectedAssignDestinationNotVariableDeclarationTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest);
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(dest) == Mock.Of<IDeclaredElement>());
            var metricHelper = Mock.Of<IMetricHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, metricHelper);

            // Assert
            Assert.Throws<UnexpectedAssignDestinationException>(() => assignmentExpressionEater.Eat(snapshot, assignmentExpression));
        }

        [Test]
        public void UnexpectedAssignDestinationNotReferenceTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var dest = Mock.Of<ICSharpExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest);
            var eater = Mock.Of<IEater>();
            var eatExpressionHelper = Mock.Of<EatExpressionHelper>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, eatExpressionHelper, metricHelper);

            // Assert
            Assert.Throws<UnexpectedAssignDestinationException>(() => assignmentExpressionEater.Eat(snapshot, assignmentExpression));
        }
    }
}
