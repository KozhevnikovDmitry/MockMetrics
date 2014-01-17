using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.DeclaredElements;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ReferenceEatHelperTests
    {
        [Test]
        public void EatVariableDeclarationReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>(t => t.GetVarMetrics(declaredElement) == Variable.None);
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatParameterReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IParameter>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>(t => t.GetVarMetrics(declaredElement) == Variable.None);
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatPropertyReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var type = Mock.Of<IType>();
            var declaredElement = Mock.Of<IProperty>(t => t.Type == type);
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, type) == Variable.None);
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatFieldReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var type = Mock.Of<IType>();
            var declaredElement = Mock.Of<IField>(t => t.Type == type);
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var eater = Mock.Of<IEater>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, type) == Variable.None);
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatEnumReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IEnum>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.Library);
        }

        [Test]
        public void EatTypeElementReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<ITypeElement>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper =
                Mock.Of<IMetricHelper>(t => t.MetricForTypeReferece(snapshot, declaredElement) == Variable.None);
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatMethodReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IMethod>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.Service);
        }

        [Test]
        public void EatEventReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IEvent>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.Service);
        }

        [Test]
        public void EatAliasReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IAlias>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void EatNamespaceReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<INamespace>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [Test]
        public void UnexpectedReferenceTypeTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IDeclaredElement>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Assert
            Assert.Throws<UnexpectedReferenceTypeException>(() => refereceEatHelper.Eat(snapshot, referenceExpression));
        }

        [Test]
        public void GetParentedVarTypeNullParentReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IDeclaredElement>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            var result = refereceEatHelper.GetParentedVarType(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, Variable.None);
        }

        [TestCase(Variable.Library, Result = Variable.Library)]
        [TestCase(Variable.None, Result = Variable.None)]
        [TestCase(Variable.Target, Result = Variable.Service)]
        [TestCase(Variable.Stub, Result = Variable.Service)]
        [TestCase(Variable.Service, Result = Variable.Service)]
        [TestCase(Variable.Mock, Result = Variable.Service)]
        public Variable GetParentedVarTypeTest(Variable parentReference)
        {
            // Arrange
            var qualifierReference = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == qualifierReference);
            var declaredElement = Mock.Of<IDeclaredElement>();
            var eatHelper =
                Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, qualifierReference) == parentReference);
            var refereceEatHelper = new RefereceEatHelper(eater, metricHelper, eatHelper);

            // Act
            return refereceEatHelper.GetParentedVarType(snapshot, referenceExpression);
        }
    }
}