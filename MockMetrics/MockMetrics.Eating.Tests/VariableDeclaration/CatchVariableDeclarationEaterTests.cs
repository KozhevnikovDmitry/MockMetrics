using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.VariableDeclaration
{
    [TestFixture]
    public class CatchVariableDeclarationEaterTests
    {
        [Test]
        public void AddCatchVariableToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var catchVariableDeclaration = Mock.Of<ICatchVariableDeclaration>();
            var eater = Mock.Of<IEater>();
            var catchVariableDeclarationEater = new CatchVariableDeclarationEater(eater);

            // Act
            var result = catchVariableDeclarationEater.Eat(snapshot.Object, catchVariableDeclaration);

            // Assert
            Assert.AreEqual(result, Variable.Service);
            snapshot.Verify(t => 
                t.AddVariable(catchVariableDeclaration, Variable.Service), 
                Times.Once);
        }
    }
}