using JetBrains.ReSharper.Psi.ExtensionsAPI.Caches2;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;
using Enum = System.Enum;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ExpressionKindHelperTests
    {
        [Test]
        public void EatTargetCallTest()
        {
            // Arrange
            var helper = new ExpressionKindHelper();
            
            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(ExpressionKind.TargetCall, kind), ExpressionKind.Result);
            }
        }

        [Test]
        public void EatResultTest()
        {
            // Arrange
            var helper = new ExpressionKindHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall)
                Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(ExpressionKind.Result, kind), ExpressionKind.Result);
            }
        }

        [Test]
        public void EatMockTest()
        {
            // Arrange
            var helper = new ExpressionKindHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall && kind != ExpressionKind.Result)
                Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(ExpressionKind.Mock, kind), ExpressionKind.Mock);
            }
        }

        [Test]
        public void EatTargetTest()
        {
            // Arrange
            var helper = new ExpressionKindHelper();

            // Assert
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall &&
                    kind != ExpressionKind.Result &&
                    kind != ExpressionKind.Mock)
                Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(ExpressionKind.Target, kind), ExpressionKind.Target);
            }
            foreach (ExpressionKind kind in Enum.GetValues(typeof(ExpressionKind)))
            {
                if (kind != ExpressionKind.TargetCall &&
                    kind != ExpressionKind.Result &&
                    kind != ExpressionKind.Mock)
                    Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(kind, ExpressionKind.Target), ExpressionKind.Target);
            }
        }

        [Test]
        public void EatAnyValueKindTest()
        {
            // Arrange
            var helper = new ExpressionKindHelper();

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
                        Assert.AreEqual(helper.ValueOfKindAsTypeOfKind(valueKind, typeKind), valueKind);
                }
            }
        }
    }
}