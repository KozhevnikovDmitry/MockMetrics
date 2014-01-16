using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Helpers;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class ReferenceExpressionEater : ExpressionEater<IReferenceExpression>
    {
        private readonly IRefereceEatHelper _refereceEatHelper;

        public ReferenceExpressionEater(IEater eater, IRefereceEatHelper refereceEatHelper)
            : base(eater)
        {
            _refereceEatHelper = refereceEatHelper;
        }

        public override Variable Eat(ISnapshot snapshot, IReferenceExpression expression)
        {
            var parentedVarType = _refereceEatHelper.GetParentedVarType(snapshot, expression);
            var curVarType = _refereceEatHelper.Eat(snapshot, expression);

            if (parentedVarType == Variable.Library)
            {
                curVarType = Variable.Library;
            }

            var result = _refereceEatHelper.ExecuteResult(curVarType, snapshot, expression);

            // TODO : Cover by unit tests
            if (_refereceEatHelper.IsStandaloneMethodReference(expression))
            {
                snapshot.AddVariable(expression, result);
            }

            return result;
        }

        
    }
}