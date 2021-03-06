﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Statement;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Statement
{
    [TestFixture]
    public class ForStatementEaterTests
    {
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

        [Test]
        public void AddConditionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var condition = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>(t => t.Condition == condition);
            Mock.Get(forStatement)
                .Setup(t => t.Initializer.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, condition) == ExpressionKind.None);
            var forEater = new ForStatementEater(eater);

            // Act
            forEater.Eat(snapshot.Object, forStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, condition), Times.Once);
        }

        [Test]
        public void EatInitializerTest()
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
        public void AddInitializerToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var initilizer = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>();
            Mock.Get(forStatement)
               .Setup(t => t.Initializer.Expressions)
               .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { initilizer }));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, initilizer) == ExpressionKind.None);
            var forEater = new ForStatementEater(eater);

            // Act
            forEater.Eat(snapshot.Object, forStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, initilizer), Times.Once);
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
        public void AdditeratorsToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var iterator = Mock.Of<ICSharpExpression>();
            var forStatement = Mock.Of<IForStatement>();
            Mock.Get(forStatement)
               .Setup(t => t.Initializer.Expressions)
               .Returns(new TreeNodeCollection<ICSharpExpression>(new ICSharpExpression[0]));
            Mock.Get(forStatement)
                .Setup(t => t.Iterators.Expressions)
                .Returns(new TreeNodeCollection<ICSharpExpression>(new[] { iterator }));
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, iterator) == ExpressionKind.None);
            var forEater = new ForStatementEater(eater);

            // Act
            forEater.Eat(snapshot.Object, forStatement);

            // Assert
            snapshot.Verify(t => t.Add(ExpressionKind.None, iterator), Times.Once);
        }
    }
}
