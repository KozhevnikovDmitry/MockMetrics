using System;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using MockMetrics.Eating.Expression;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ParentReferenceEaterTests
    {
        [Test]
        public void ReturnNoneIfInvocationQualifierNullTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReferenceEater = new ParentReferenceEater(eater);

            // Act
            var kind = parentReferenceEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [Test]
        public void ReturnNoneIfInvocationQualifierReferenceIsNotExtensionArgumentInfoTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var managedConverible = Mock.Of<IManagedConvertible>();
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier.ManagedConvertible == managedConverible);
            var snapshot = Mock.Of<ISnapshot>();
            var parentReferenceEater = new ParentReferenceEater(eater);

            // Act
            var kind = parentReferenceEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.None);
        }

        [Test]
        public void ReturnEatResultOfInvocationQualifierReferenceTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<ICSharpExpression>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression, false) == ExpressionKind.StubCandidate);
            var managedConverible = new Mock<ExtensionArgumentInfo>(Mock.Of<ICSharpInvocationInfo>(), Mock.Of<ICSharpExpression>());
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier.ManagedConvertible == managedConverible.Object);
            var parentReferenceEater = new ParentReferenceEater(eater);

            // Act
            var kind = parentReferenceEater.Eat(snapshot, invocationExpression, false);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.StubCandidate);
        }

        [Test]
        public void NullEaterTest()
        {

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ParentReferenceEater(null));
        }

        [Test]
        public void NullSnapshotTest()
        {
            // Arrange
            var parentReferenceEater = new ParentReferenceEater(Mock.Of<IEater>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => parentReferenceEater.Eat(null, Mock.Of<IInvocationExpression>(), false));
        }

        [Test]
        public void NullInvocationExpressionTest()
        {
            // Arrange
            var parentReferenceEater = new ParentReferenceEater(Mock.Of<IEater>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => parentReferenceEater.Eat(Mock.Of<ISnapshot>(), null, false));
        }
    }
}