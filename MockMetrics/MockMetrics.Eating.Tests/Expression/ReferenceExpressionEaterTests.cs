using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ReferenceExpressionEaterTests
    {
        [Test]
        public void EatSelfPropertyTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var propertyType = Mock.Of<IType>();
            var refElement = Mock.Of<IProperty>(t => t.Type == propertyType);
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>(t => t.EatVariableType(snapshot, propertyType) == ExpressionKind.Target);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatSelfFieldTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var fieldType = Mock.Of<IType>();
            var refElement = Mock.Of<IField>(t => t.Type == fieldType);
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>(t => t.EatVariableType(snapshot, fieldType) == ExpressionKind.Target);
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatSelfEventTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var refElement = Mock.Of<IEvent>();
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>();
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void EatLocalConstantTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var refElement = Mock.Of<ILocalConstantAndIDeclaredElement>();
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>();
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Stub);
        }

        [Test]
        public void EatLocalVariableTest()
        {
            // Arrange
            var refElement = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>();
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var snapshot = Mock.Of<ISnapshot>(t => t.GetVariableKind(refElement) == ExpressionKind.Target);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.Target);
        }

        [Test]
        public void EatByParentTest()
        {
            // Arrange
            var qualifierExpression = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == qualifierExpression);
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var typeEater = Mock.Of<ITypeEater>();
            var kindHelper = Mock.Of<ExpressionKindHelper>(t => t.ReferenceKindByParentReferenceKind(ExpressionKind.Target) == ExpressionKind.TargetCall);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, qualifierExpression) == ExpressionKind.Target);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);

            // Act
            var kind = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(kind, ExpressionKind.TargetCall);
        }

        [Test]
        public void UnexpectedAssignDestinationTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var refElement = Mock.Of<IDeclaredElement>();
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == refElement);
            var typeEater = Mock.Of<ITypeEater>();
            var kindHelper = Mock.Of<ExpressionKindHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, It.IsAny<ICSharpExpression>()) == ExpressionKind.None);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, kindHelper, eatHelper, typeEater);
            
            // Assert
            Assert.Throws<UnexpectedReferenceTypeException>(() => referenceExpressionEater.Eat(snapshot, referenceExpression));
        }
    }
}
