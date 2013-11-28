using System;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve.Managed;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.MetricMeasure;
using NUnit.Framework;
using Moq;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ParentReferenceEaterTests
    {
        [Test]
        public void ReturnScopeInternalIfInvocationQualifierNullTest()
        {
            // Arrange
            var eater = Mock.Of<IEater>();
            var invocationExpression = Mock.Of<IInvocationExpression>();
            var snapshot = Mock.Of<ISnapshot>();
            var parentReferenceEater = new ParentReferenceEater(eater);

            // Act
            var metrics = parentReferenceEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(metrics.Scope, Scope.Internal);
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
            var metrics = parentReferenceEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(metrics.Scope, Scope.Internal);
        }

        [Test]
        public void ReturnEatResultOfInvocationQualifierReferenceTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expression = Mock.Of<ICSharpExpression>();
            var metrics = Metrics.Create();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expression) == metrics);
            var managedConverible = new Mock<ExtensionArgumentInfo>(Mock.Of<ICSharpInvocationInfo>(), Mock.Of<ICSharpExpression>());
            var invocationExpression = Mock.Of<IInvocationExpression>(t => t.ExtensionQualifier.ManagedConvertible == managedConverible.Object);
            var parentReferenceEater = new ParentReferenceEater(eater);

            // Act
            var result = parentReferenceEater.Eat(snapshot, invocationExpression);

            // Assert
            Assert.AreEqual(result, metrics);
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
            Assert.Throws<ArgumentNullException>(() => parentReferenceEater.Eat(null, Mock.Of<IInvocationExpression>()));
        }

        [Test]
        public void NullInvocationExpressionTest()
        {
            // Arrange
            var parentReferenceEater = new ParentReferenceEater(Mock.Of<IEater>());

            // Assert
            Assert.Throws<ArgumentNullException>(() => parentReferenceEater.Eat(Mock.Of<ISnapshot>(), null));
        }
    }
}