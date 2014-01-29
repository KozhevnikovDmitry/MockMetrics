using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class AssignmentExpressionEaterTests
    {
        [Test]
        public void EatAssignmentDeclarationReferenceExpressionTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var declaration = Mock.Of<ICSharpDeclaration>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source) == Variable.None
                                             && t.Eat(snapshot.Object, dest) == Variable.Library);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(Variable.Library, Variable.None) == Variable.Service);
            var eatExpressioHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceDeclaration(dest) == declaration);
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, metricHelper, eatExpressioHelper);

            // Act
            var result = assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(declaration, Variable.Service));
            Assert.AreEqual(result, Variable.None);
        }
        [Test]
        public void EatAssignmentNullDeclarationReferenceExpressionTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<IReferenceExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, source) == Variable.None
                                             && t.Eat(snapshot.Object, dest) == Variable.Library);
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsMerge(Variable.Library, Variable.None) == Variable.Service);
            var eatExpressioHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceDeclaration(dest) == new NullCsharpDeclaration());
            var assignmentExpressionEater = new AssignmentExpressionEater(eater, metricHelper, eatExpressioHelper);

            // Act
            var result = assignmentExpressionEater.Eat(snapshot.Object, assignmentExpression);

            // Assert
            snapshot.Verify(t => t.AddVariable(dest, Variable.Service), Times.Once);
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatAssignmentNotReferenceExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var source = Mock.Of<ICSharpExpression>();
            var dest = Mock.Of<ICSharpExpression>();
            var assignmentExpression = Mock.Of<IAssignmentExpression>(t => t.Dest == dest && t.Source == source);
            var eater = new Mock<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eatExpressioHelper = Mock.Of<EatExpressionHelper>();
            var assignmentExpressionEater = new AssignmentExpressionEater(eater.Object, metricHelper, eatExpressioHelper);

            // Act
            var result = assignmentExpressionEater.Eat(snapshot, assignmentExpression);

            // Assert
            eater.Verify(t => t.Eat(snapshot, source));
            eater.Verify(t => t.Eat(snapshot, dest));
            Assert.AreEqual(result, Variable.None);
        }
    }
}
