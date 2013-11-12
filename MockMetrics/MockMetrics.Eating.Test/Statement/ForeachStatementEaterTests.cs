using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class ForeachStatementEaterTests
    {
        [Test]
        public void EatBodyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var body = Mock.Of<ICSharpStatement>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.Body == body);
            var eater = new Mock<IEater>();
            var foreachEater = new ForeachStatementEater(eater.Object);

            // Act
            foreachEater.Eat(snapshot, foreachStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, body), Times.Once);
        }

        [Test]
        public void EatCollectionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var collection = Mock.Of<ICSharpExpression>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.Collection == collection);
            var eater = new Mock<IEater>();
            var foreachEater = new ForeachStatementEater(eater.Object);

            // Act
            foreachEater.Eat(snapshot, foreachStatement);

            // Assert
            eater.Verify(t => t.Eat(snapshot, collection), Times.Once);
        }

        [Test]
        public void AddCollectionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var collection = Mock.Of<ICSharpExpression>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.Collection == collection);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot.Object, collection) == ExpressionKind.Stub);
            var foreachEater = new ForeachStatementEater(eater);

            // Act
            foreachEater.Eat(snapshot.Object, foreachStatement);

            // Assert
            snapshot.Verify(t => t.AddTreeNode(ExpressionKind.Stub, collection), Times.Once);
        }

        [Test]
        public void AddCurrentReferenceToSnapshotAsStubTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var currentRef = Mock.Of<IForeachStatementReference>();
            var foreachStatement = Mock.Of<IForeachStatement>(t => t.CurrentReference == currentRef);
            var eater = Mock.Of<IEater>();
            var foreachEater = new ForeachStatementEater(eater);

            // Act
            foreachEater.Eat(snapshot.Object, foreachStatement);

            // Assert
            snapshot.Verify(t => t.AddVariable(currentRef), Times.Once);
        }
    }
}