using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.Tests.StubTypes;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ReferenceExpressionEaterTests
    {
        [Test]
        public void EatReferenceByParentTest()
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expressionQualifier = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == expressionQualifier);
            var eatHelper = Mock.Of<EatExpressionHelper>();
            var qualifierMetrics = Metrics.Create(Scope.Local, VarType.Stub);
            var resultMetrics = Metrics.Create();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForReference(qualifierMetrics) == resultMetrics);
            var eater = Mock.Of<IEater>(t => t.Eat(snapshot, expressionQualifier) == qualifierMetrics);
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
        }

        [Test]
        public void EatVariableDeclarationReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IVariableDeclarationAndIDeclaredElement>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var resultMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>(t => t.GetVarMetrics(declaredElement) == resultMetrics);
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
        }

        [Test]
        public void EatParameterReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IParameter>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var resultMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>(t => t.GetVarMetrics(declaredElement) == resultMetrics);
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
        }

        [Test]
        public void EatPropertyReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var type = Mock.Of<IType>();
            var declaredElement = Mock.Of<IProperty>(t => t.Type == type);
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var resultMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, type) == resultMetrics);
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
            Assert.AreEqual(resultMetrics.Scope, Scope.Internal);
        }

        [Test]
        public void EatFieldReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var type = Mock.Of<IType>();
            var declaredElement = Mock.Of<IField>(t => t.Type == type);
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var resultMetrics = Metrics.Create();
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.MetricsForType(snapshot, type) == resultMetrics);
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result, resultMetrics);
            Assert.AreEqual(resultMetrics.Scope, Scope.Internal);
        }

        [Test]
        public void EatEnumReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IEnum>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result.Scope, Scope.Local);
            Assert.AreEqual(result.VarType, VarType.Library);
        }

        [Test]
        public void EatTypeElementReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<ITypeElement>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>(t => t.GetTypeScope(snapshot, declaredElement) == Scope.Local);
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result.Scope, Scope.Local);
        }

        [Test]
        public void EatMethodReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IMethod>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result.Scope, Scope.Internal);
        }

        [Test]
        public void EatEventReferenceTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IEvent>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Act
            var result = referenceExpressionEater.Eat(snapshot, referenceExpression);

            // Assert
            Assert.AreEqual(result.Scope, Scope.Internal);
            Assert.AreEqual(result.VarType, VarType.Internal);
        }

        [Test]
        public void UnexpectedReferenceTypeTest()
        {
            // Arrange
            var referenceExpression = Mock.Of<IReferenceExpression>();
            var declaredElement = Mock.Of<IDeclaredElement>();
            var eatHelper = Mock.Of<EatExpressionHelper>(t => t.GetReferenceElement(referenceExpression) == declaredElement);
            var snapshot = Mock.Of<ISnapshot>();
            var metricHelper = Mock.Of<IMetricHelper>();
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, metricHelper, eatHelper);

            // Assert
            Assert.Throws<UnexpectedReferenceTypeException>(() => referenceExpressionEater.Eat(snapshot, referenceExpression));
       }
    }
}
