using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public interface ITypeUsageEater
    {
        ExpressionKind Eat(ISnapshot snapshot, ITypeUsage typeUsage);
    }

    public class TypeUsageEater : ITypeUsageEater
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public TypeUsageEater(EatExpressionHelper eatExpressionHelper)
        {
            _eatExpressionHelper = eatExpressionHelper;
        }

        public ExpressionKind Eat(ISnapshot snapshot, ITypeUsage typeUsage)
        {
            if (typeUsage is IDynamicTypeUsage)
            {
                return ExpressionKind.StubCandidate;
            }

            if (typeUsage is IPredefinedTypeUsage)
            {
                return ExpressionKind.StubCandidate;
            }

            if (typeUsage is IUserTypeUsage)
            {
                var userTypeUsage = typeUsage as IUserTypeUsage;
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