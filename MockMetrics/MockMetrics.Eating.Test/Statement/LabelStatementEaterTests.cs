using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Statement;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Test.Statement
{
    [TestFixture]
    public class LabelStatementEaterTests
    {
        [Test]
        public void AddLabelToSnapshotAsStubTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var label = Mock.Of<ILabelStatement>();
            var eater = Mock.Of<IEater>();
            var foreachEater = new LabelStatementEater(eater);

            // Act
            foreachEater.Eat(snapshot.Object, label);

            // Assert
            snapshot.Verify(t => t.AddLabel(label), Times.Once);
        } 
    }
}