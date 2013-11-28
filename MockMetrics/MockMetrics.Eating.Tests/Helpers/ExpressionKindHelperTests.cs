using NUnit.Framework;
using Enum = System.Enum;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ExpressionKindHelperTests
    {
        [Test]
        public void ValueOfKindAsTypeOfKind_EatTargetCallTest()
        {
            // Arrange
            var helper = new VarTypeHelper();
            
            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                Assert.AreEqual(helper.CastExpressionType(ExpressionKind.TargetCall, kind), ExpressionKind.Result);
            }
        }

        [Test]
        public void ValueOfKindAsTypeOfKind_EatResultTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall)
                Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Result, kind), ExpressionKind.Result);
            }
        }

        [Test]
        public void ValueOfKindAsTypeOfKind_EatMockTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall && kind != ExpressionKind.Result)
                Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Mock, kind), ExpressionKind.Mock);
            }
        }

        [Test]
        public void ValueOfKindAsTypeOfKind_EatTargetTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall &&
                    kind != ExpressionKind.Result &&
                    kind != ExpressionKind.Mock)
                Assert.AreEqual(helper.CastExpressionType(ExpressionKind.Target, kind), ExpressionKind.Target);
            }
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall &&
                    kind != ExpressionKind.Result &&
                    kind != ExpressionKind.Mock)
                    Assert.AreEqual(helper.CastExpressionType(kind, ExpressionKind.Target), ExpressionKind.Target);
            }
        }

        [Test]
        public void ValueOfKindAsTypeOfKind_EatAnyValueKindTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            foreach (ExpressionKind valueKind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (valueKind != ExpressionKind.TargetCall &&
                    valueKind != ExpressionKind.Result &&
                    valueKind != ExpressionKind.Mock &&
                    valueKind != ExpressionKind.Target)
                foreach (ExpressionKind typeKind in Enum.GetValues(typeof(ExpressionKind)))
                {
                    if (typeKind != ExpressionKind.Target)
                        Assert.AreEqual(helper.CastExpressionType(valueKind, typeKind), valueKind);
                }
            }
        }

        [Test]
        public void InvocationKindByParentReferenceKind_EatTargetCallTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Act
            var kind = helper.InvocationKindByParentReferenceKind(ExpressionKind.TargetCall);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Result);

        }

        [Test]
        public void InvocationKindByParentReferenceKind_EatTargetTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Act
            var kind = helper.InvocationKindByParentReferenceKind(ExpressionKind.Target);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }

        [Test]
        public void InvocationKindByParentReferenceKindTest()
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall && kind != ExpressionKind.Target)
                    Assert.AreEqual(helper.InvocationKindByParentReferenceKind(kind), kind);
            }
        }

        [TestCase(ExpressionKind.Target, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.TargetCall, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Mock, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Result, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Stub, Result = ExpressionKind.Stub)]
        [TestCase(ExpressionKind.StubCandidate, Result = ExpressionKind.StubCandidate)]
        [TestCase(ExpressionKind.Assert, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.None, Result = ExpressionKind.None)]
        public ExpressionKind ReferenceKindByParentReferenceKindTest(ExpressionKind kind)
        {
            // Arrange
            var helper = new VarTypeHelper();
            
            // Assert
            return helper.ReferenceKindByParentReferenceKind(kind);
        }

        [TestCase(ExpressionKind.Target, Result = ExpressionKind.Target)]
        [TestCase(ExpressionKind.TargetCall, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Mock, Result = ExpressionKind.Mock)]
        [TestCase(ExpressionKind.Result, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.Stub, Result = ExpressionKind.Stub)]
        [TestCase(ExpressionKind.StubCandidate, Result = ExpressionKind.Stub)]
        [TestCase(ExpressionKind.Assert, Result = ExpressionKind.Result)]
        [TestCase(ExpressionKind.None, Result = ExpressionKind.None)]
        public ExpressionKind KindOfAssignmentTest(ExpressionKind kind)
        {
            // Arrange
            var helper = new VarTypeHelper();

            // Assert
            return helper.KindOfAssignment(kind);
        }
    }
}