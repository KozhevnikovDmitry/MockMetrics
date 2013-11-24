using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class SwitchLabelStatementEaterTests
    {
        [Test]
        public void EatValueExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var valueExpression = Mock.Of<ICSharpExpression>();
            var switchStatement = Mock.Of<ISwitchLabelStatement>(t => t.ValueExpression == valueExpression);
            var eater = new Mock<IEater>();
            var switchStatementEater = new SwitchLabelStatementEater(eater.Object);

            // Act
            switchStatementEater.Eat(snapshot, switchStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, valueExpression, false), Times.Once);
        }

        [Test]
        public void AddValueExpressionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var valueExpression = Mock.Of<ICSharpExpression>();
            var switchStatement = Mock.Of<ISwitchLabelStatement>(t => t.ValueExpression == valueExpression);

            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, valueExpression, false) == ExpressionKind.None);
            var switchStatementEater = new SwitchLabelStatementEater(eater);

            // Act
            switchStatementEater.Eat(snapshot.Object, switchStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, valueExpression), Times.Once);
        } 
    }
}
