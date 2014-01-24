using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ForStatementEaterTests
    {
        [Test]
        public void EatInitializerExpressionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var initilizer = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>();
            Mock.Get(forStatement)
                .Setup(t => t.Initializer.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { initilizer }));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = new Mock<IEater>();
            var forEater = new ForStatementEater(eater.Object);

            // Act
            forEater.Eat(snapshot, forStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, initilizer), Times.Once);
        }

        [Test]
        public void EatInitializerDeclarationTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var declarator = Mock.Of<ILocalVariableDeclaration>();
            var declaration = Mock.Of<IMultipleLocalVariableDeclaration>();
            Mock.Get(declaration)
                .Setup(t => t.Declarators)
                .Returns(new TreeNodeCollection<IMultipleDeclarationMember>(new[] { declarator }));
            var forStatement = Mock.Of<IForStatement>(t => t.Initializer.Declaration == declaration); 
            Mock.Get(forStatement)
                 .Setup(t => t.Initializer.Expressions)
                 .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = new Mock<IEater>();
            var forEater = new ForStatementEater(eater.Object);

            // Act
            forEater.Eat(snapshot, forStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, declarator), Times.Once);
        }

        [Test]
        public void EatIteratorsTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var iterator = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>();
            Mock.Get(forStatement)
                .Setup(t => t.Initializer.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { iterator }));
            var eater = new Mock<IEater>();
            var forEater = new ForStatementEater(eater.Object);

            // Act
            forEater.Eat(snapshot, forStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, iterator), Times.Once);
        }

        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var forStatement = Mock.Of<IForStatement>(t => t.Body == body);
            Mock.Get(forStatement)
                .Setup(t => t.Initializer.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = new Mock<IEater>();
            var forEater = new ForStatementEater(eater.Object);

            // Act
            forEater.Eat(snapshot, forStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatConditionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>(t => t.Condition == condition);
            Mock.Get(forStatement)
                .Setup(t => t.Initializer.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = new Mock<IEater>();
            var forEater = new ForStatementEater(eater.Object);

            // Act
            forEater.Eat(snapshot, forStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, condition), Times.Once);
        }
    }
}
