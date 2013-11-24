using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class UsingStatementEaterTests
    {
        [Test]
        public void EatVariablesTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var variable = Mock.Of<ILocalVariableDeclaration>();
            var usingStatement = Mock.Of<IUsingStatement>();
            Mock.Get(usingStatement)
                .Setup(t => t.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(usingStatement)
                .Setup(t => t.VariableDeclarations)
                .Returns(new TreeNodeCollection<ILocalVariableDeclaration>(new[] { variable }));

            var usingStatementEater = new UsingStatementEater(eater.Object);

            // Act
            usingStatementEater.Eat(snapshot, usingStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, variable), Times.Once);
        }

        [Test]
        public void EatExpressionsTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var expression = Mock.Of<ICSharpExpression>();
            var usingStatement = Mock.Of<IUsingStatement>();
            Mock.Get(usingStatement)
                .Setup(t => t.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { expression }));
            Mock.Get(usingStatement)
                .Setup(t => t.VariableDeclarations)
                .Returns(new TreeNodeCollection<ILocalVariableDeclaration>(new ILocalVariableDeclaration[0]));

            var usingStatementEater = new UsingStatementEater(eater.Object);

            // Act
            usingStatementEater.Eat(snapshot, usingStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, expression, false), Times.Once);
        }

        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = new Mock<IEater>();
            var body = Mock.Of<IBlock>();
            var usingStatement = Mock.Of<IUsingStatement>(t => t.Body == body);
            Mock.Get(usingStatement)
                .Setup(t => t.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(usingStatement)
                .Setup(t => t.VariableDeclarations)
                .Returns(new TreeNodeCollection<ILocalVariableDeclaration>(new ILocalVariableDeclaration[0]));

            var usingStatementEater = new UsingStatementEater(eater.Object);

            // Act
            usingStatementEater.Eat(snapshot, usingStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }
    }
}
