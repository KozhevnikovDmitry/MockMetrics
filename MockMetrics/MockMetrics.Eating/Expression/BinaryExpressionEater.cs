using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class BinaryExpressionEater : ExpressionEater<IBinaryExpression>
    {
        private readonly VarTypeHelper _varTypeHelper;

        public BinaryExpressionEater(IEater eater, VarTypeHelper varTypeHelper) : base(eater)
        {
            _varTypeHelper = varTypeHelper;
        }

        public override VarType Eat(ISnapshot snapshot, IBinaryExpression expression)
        {
            var leftVarType = Eater.Eat(snapshot, expression.LeftOperand);
            var rightVarType = Eater.Eat(snapshot, expression.RightOperand);

            return _varTypeHelper.CastExpressionType(leftVarType, rightVarType);
        }
    }
}
