using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.MoqFake;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.MoqFake
{
    [TestFixture]
    public class MoqStubOptionsEaterTests
    {
        [Test]
        public void EatInvocationOptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var option = Mock.Of<IInvocationExpression>();
            var stubOptionTargetEater = new Mock<IMoqStubOptionTargetEater>();
            var stubOptionsEater = new MoqStubOptionsEater(eater, stubOptionTargetEater.Object);

            // Act
            stubOptionsEater.EatStubOptions(snapshot, option);

            // Assert
            stubOptionTargetEater.Verify(t => t.EatOption(snapshot, option), Times.Once);
        }
        
        [Test]
        public void AddInvocationOptionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var option = Mock.Of<IInvocationExpression>();
            var stubOptionTargetEater = Mock.Of<IMoqStubOptionTargetEater>(t => t.EatOption(snapshot.Object, option) == FakeOption.Method);
            var stubOptionsEater = new MoqStubOptionsEater(eater, stubOptionTargetEater);

            // Act
            stubOptionsEater.EatStubOptions(snapshot.Object, option);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(option, FakeOption.Method), Times.Once);
        }
        
        [Test]
        public void EatReferenceOptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var option = Mock.Of<IReferenceExpression>();
            var stubOptionTargetEater = new Mock<IMoqStubOptionTargetEater>();
            var stubOptionsEater = new MoqStubOptionsEater(eater, stubOptionTargetEater.Object);

            // Act
            stubOptionsEater.EatStubOptions(snapshot, option);

            // Assert
            stubOptionTargetEater.Verify(t => t.EatOption(snapshot, option), Times.Once);
        } 

        [Test]
        public void AddReferenceOptionToSnapshotTest()
        {
            // Arrange
            var snapshot = new Mock<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var option = Mock.Of<IReferenceExpression>();
            var stubOptionTargetEater = Mock.Of<IMoqStubOptionTargetEater>(t => t.EatOption(snapshot.Object, option) == FakeOption.Property);
            var stubOptionsEater = new MoqStubOptionsEater(eater, stubOptionTargetEater);

            // Act
            stubOptionsEater.EatStubOptions(snapshot.Object, option);

            // Assert
            snapshot.Verify(t => t.AddFakeOption(option, FakeOption.Property), Times.Once);
        }

        [Test]
        public void EatConditionalOptionTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var leftOption = Mock.Of<IInvocationExpression>();
            var rightOption = Mock.Of<IInvocationExpression>();
            var conditionalOption = Mock.Of<IConditionalAndExpression>(t => t.LeftOperand == leftOption && t.RightOperand == rightOption);
            var stubOptionTargetEater = new Mock<IMoqStubOptionTargetEater>();
            var stubOptionsEater = new Mock<MoqStubOptionsEater>(eater, stubOptionTargetEater.Object) {CallBase = true};

            // Act
            stubOptionsEater.Object.EatStubOptions(snapshot, conditionalOption);

            // Assert
            stubOptionsEater.Verify(t => t.EatStubOptions(snapshot, leftOption), Times.Once);
            stubOptionsEater.Verify(t => t.EatStubOptions(snapshot, rightOption), Times.Once);
        }

        [Test]
        public void EatEqualityOptionLeftOperandTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var leftOption = Mock.Of<IInvocationExpression>();
            var rightOption = Mock.Of<IInvocationExpression>();
            var conditionalOption = Mock.Of<IEqualityExpression>(t => t.LeftOperand == leftOption && t.RightOperand == rightOption);
            var stubOptionTargetEater = new Mock<IMoqStubOptionTargetEater>();
            var stubOptionsEater = new Mock<MoqStubOptionsEater>(eater, stubOptionTargetEater.Object) { CallBase = true };

            // Act
            stubOptionsEater.Object.EatStubOptions(snapshot, conditionalOption);

            // Assert
            stubOptionsEater.Verify(t => t.EatStubOptions(snapshot, leftOption), Times.Once);
        }

        [Test]
        public void EatEqualityOptionRightOperandTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var leftOption = Mock.Of<IInvocationExpression>();
            var rightOption = Mock.Of<IInvocationExpression>();
            var conditionalOption = Mock.Of<IEqualityExpression>(t => t.LeftOperand == leftOption && t.RightOperand == rightOption);
            var stubOptionTargetEater = new Mock<IMoqStubOptionTargetEater>();
            var stubOptionsEater = new Mock<MoqStubOptionsEater>(eater, stubOptionTargetEater.Object) { CallBase = true };

            // Act
            stubOptionsEater.Object.EatStubOptions(snapshot, conditionalOption);

            // Assert
            stubOptionsEater.Verify(t => t.EatStubOptions(snapshot, leftOption), Times.Once);
        }
    }
}