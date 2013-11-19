using System;
using JetBrains.Annotations;
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

        public TypeEater([NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null) 
                throw new ArgumentNullException("eatExpressionHelper");

            _eatExpressionHelper = eatExpressionHelper;
        }

        public ExpressionKind EatCastType([NotNull] ISnapshot snapshot, [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null) 
                throw new ArgumentNullException("typeUsage");

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

        public ExpressionKind EatVariableType([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (type == null) 
                throw new ArgumentNullException("type");
            
            var classType = _eatExpressionHelper.GetTypeClass(type);
            if (snapshot.IsInTestScope(classType.Module.Name))
            {
                return ExpressionKind.Target;
            }

            if (snapshot.IsInTestProject(classType.Module.Name))
            {
                return ExpressionKind.Mock;
            }

            return ExpressionKind.StubCandidate;
        }
    }
}