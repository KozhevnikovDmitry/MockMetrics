using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.Expression
{
    public interface ITypeEater
    {
        ExpressionKind EatCastType(ISnapshot snapshot, ITypeUsage typeUsage);

        ExpressionKind EatVariableType(ISnapshot snapshot, IType type);
    }

    public class TypeEater : ITypeEater
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public TypeEater(EatExpressionHelper eatExpressionHelper)
        {
            _eatExpressionHelper = eatExpressionHelper;
        }

        public ExpressionKind EatCastType(ISnapshot snapshot, ITypeUsage typeUsage)
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

        public ExpressionKind EatVariableType(ISnapshot snapshot, IType type)
        {
            if (snapshot.IsInTestScope(type.Module.Name))
            {
                return ExpressionKind.Target;
            }

            if (snapshot.IsInTestProject(type.Module.Name))
            {
                return ExpressionKind.Mock;
            }

            return ExpressionKind.StubCandidate;
        }
    }
}