using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Expression;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;
using Moq;
using NUnit.Framework;

namespace MockMetrics.Eating.Tests.Expression
{
    [TestFixture]
    public class ReferenceExpressionEaterTests
    {
        [TestCase(Variable.None, Result = Variable.None)]
        [TestCase(Variable.Library, Result = Variable.Library)]
        [TestCase(Variable.Stub, Result = Variable.None)]
        [TestCase(Variable.Mock, Result = Variable.None)]
        [TestCase(Variable.Target, Result = Variable.None)]
        [TestCase(Variable.Service, Result = Variable.None)]
        public Variable EatReferenceByParentTest(Variable parentVarType)
        {
            // Arrange
            var snapshot = Mock.Of<ISnapshot>();
            var expressionQualifier = Mock.Of<ICSharpExpression>();
            var referenceExpression = Mock.Of<IReferenceExpression>(t => t.QualifierExpression == expressionQualifier);
            var eatHelper = Mock.Of<IRefereceEatHelper>(t => t.Eat(snapshot, referenceExpression) == parentVarType
                                                          && t.ExecuteResult(It.IsAny<Variable>(), snapshot, referenceExpression) == Variable.None
                                                          && t.ExecuteResult(Variable.Library, snapshot, referenceExpression) == Variable.Library);
            var eater = Mock.Of<IEater>();
            var referenceExpressionEater = new ReferenceExpressionEater(eater, eatHelper);

            // Act
            return referenceExpressionEater.Eat(snapshot, referenceExpression);
        }
    }
}
