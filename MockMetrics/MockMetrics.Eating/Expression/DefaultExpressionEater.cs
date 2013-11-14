using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public class DefaultExpressionEater : ExpressionEater<IDefaultExpression>
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public DefaultExpressionEater(IEater eater, EatExpressionHelper eatExpressionHelper) : base(eater)
        {
            _eatExpressionHelper = eatExpressionHelper;
        }

        public override ExpressionKind Eat(ISnapshot snapshot, IDefaultExpression expression)
        {
            if (expression.TypeName is IDynamicTypeUsage)
            {
                return ExpressionKind.StubCandidate;
            }

            if (expression.TypeName is IPredefinedTypeUsage)
            {
                return ExpressionKind.StubCandidate;
            }

            if (expression.TypeName is IUserTypeUsage)
            {
                var userTypeUsage = expression.TypeName as IUserTypeUsage;
                var classType = _eatExpressionHelper.GetUserTypeUsageClass(userTypeUsage);

                if (snapshot.IsInTestScope(classType.Module.Name))
                {
                    return ExpressionKind.Target;
                }

                if (snapshot.IsInTestProject(classType.Module.Name))
                {
                    return ExpressionKind.Mock;
                }
            }

            return ExpressionKind.StubCandidate;
        }
    }
}
