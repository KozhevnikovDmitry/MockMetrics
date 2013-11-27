using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;
using MockMetrics.Eating.VariableDeclaration;

namespace MockMetrics.Eating.Expression
{
    public class ArrayCreationExpressionEater : ExpressionEater<IArrayCreationExpression>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;

        public ArrayCreationExpressionEater(IEater eater, IVariableInitializerEater variableInitializerEater) : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
        }

        public override Metrics Eat(ISnapshot snapshot, IArrayCreationExpression expression)
        {
            // TODO : check in functional tests
            foreach (ICSharpExpression size in expression.Sizes)
            {
                Eater.Eat(snapshot, size);
            }

            _variableInitializerEater.Eat(snapshot, expression.ArrayInitializer);

            return Metrics.Create(Scope.Local, VarType.Library);
        }
    }
}
