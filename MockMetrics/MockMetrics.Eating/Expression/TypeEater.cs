using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public interface ITypeEater
    {
        VarType EatCastType(ISnapshot snapshot, ITypeUsage typeUsage);

        VarType VarTypeVariableType(ISnapshot snapshot, IType type);

        Aim AimVariableType(ISnapshot snapshot, IType type);
    }

    public class TypeEater : ITypeEater, ICSharpNodeEater
    {
        private readonly EatExpressionHelper _eatExpressionHelper;

        public TypeEater([NotNull] EatExpressionHelper eatExpressionHelper)
        {
            if (eatExpressionHelper == null) 
                throw new ArgumentNullException("eatExpressionHelper");

            _eatExpressionHelper = eatExpressionHelper;
        }

        public VarType EatCastType([NotNull] ISnapshot snapshot, [NotNull] ITypeUsage typeUsage)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (typeUsage == null) 
                throw new ArgumentNullException("typeUsage");

            if (typeUsage is IDynamicTypeUsage)
            {
                return VarType.Library;
            }

            if (typeUsage is IPredefinedTypeUsage)
            {
                return VarType.Library;
            }

            if (typeUsage is IUserTypeUsage)
            {
                var userTypeUsage = typeUsage as IUserTypeUsage;
                var classType = _eatExpressionHelper.GetUserTypeUsageClass(userTypeUsage);

                if (snapshot.IsInTestScope(classType.Module.Name))
                {
                    return VarType.Target;
                }

                if (snapshot.IsInTestProject(classType.Module.Name))
                {
                    return VarType.Mock;
                }
            }

            return VarType.Library;
        }

        public VarType VarTypeVariableType([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (type == null) 
                throw new ArgumentNullException("type");
            
            var classType = _eatExpressionHelper.GetTypeClass(type);
            if (snapshot.IsInTestScope(classType.Module.Name))
            {
                //TODO if type is interface or abstract class return stub/mock? enum struct delegate?
                return VarType.Target;
            }

            if (snapshot.IsInTestProject(classType.Module.Name))
            {
                return VarType.Internal;
            }

            if (classType.ToString().StartsWith("Moq.Mock"))
            {
                return VarType.Mock;
            }

            return VarType.Library;
        }

        // TODO : Cover by unit tests
        public Aim AimVariableType([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null) 
                throw new ArgumentNullException("snapshot");

            if (type == null) 
                throw new ArgumentNullException("type");

            var classType = _eatExpressionHelper.GetTypeClass(type);
            if (snapshot.IsInTestScope(classType.Module.Name))
            {
                return Aim.Tested;
            }

            if (snapshot.IsInTestProject(classType.Module.Name))
            {
                return Aim.Data;
            }
            
            if (classType.ToString().StartsWith("Moq.Mock"))
            {
                return Aim.Data;
            }

            if (classType.Module.Name.ToLower().StartsWith("nunit.framework") ||
                classType.Module.Name.ToLower().StartsWith("moq"))
            {
                return Aim.Service;
            }

            return Aim.Data;
        }

        public Pair<Aim, VarType> VarTypeAndAim([NotNull] ISnapshot snapshot, [NotNull] IType type)
        {
            if (snapshot == null) throw new ArgumentNullException("snapshot");
            if (type == null) throw new ArgumentNullException("type");

            return new Pair<Aim, VarType>(AimVariableType(snapshot, type), VarTypeVariableType(snapshot, type));
        }
    }
}