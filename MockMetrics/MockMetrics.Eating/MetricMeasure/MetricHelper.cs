using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricHelper
    {
        public virtual Aim AimOfExpression(VarType varType, ICSharpExpression expression)
        {
            throw new NotImplementedException();
        }

        public virtual Pair<Aim, VarType> VarTypeAndAim(ISnapshot snapshot, IType type)
        {
            throw new NotImplementedException();
        }

        public virtual VarType CastExpressionType(VarType valueType, VarType castType)
        {
            if (valueType >= castType)
            {
                return valueType;
            }
            return castType;
        }

        public virtual Call InvocationKindByParentReferenceKind(VarType parentKind)
        {
            switch (parentKind)
            {
                case VarType.Target:
                    {
                        return Call.TargetCall;
                    }
                case VarType.Mock:
                    {
                        return Call.TargetCall;
                    }
                case VarType.Library:
                    {
                        return Call.Library;
                    }
                case VarType.Internal:
                    {
                        return Call.Internal;
                    }
                case VarType.External:
                    {
                        return Call.External;
                    }
            }

            return parentKind;
        }

        public virtual VarType ReferenceKindByParentReferenceKind(VarType parentKind)
        {
            switch (parentKind)
            {
                case ExpressionKind.TargetCall:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.Target:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.Stub:
                    {
                        return ExpressionKind.Stub;
                    }
                case ExpressionKind.Mock:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.StubCandidate:
                    {
                        return ExpressionKind.StubCandidate;
                    }
                case ExpressionKind.Assert:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.None:
                    {
                        return ExpressionKind.None;
                    }
                case ExpressionKind.Result:
                    {
                        return ExpressionKind.Result;
                    }
                default:
                    {
                        return ExpressionKind.None;
                    }
            }
        }

        public virtual VarType KindOfAssignment(VarType assignSourceKind)
        {
            switch (assignSourceKind)
            {
                case ExpressionKind.TargetCall:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.StubCandidate:
                    {
                        return ExpressionKind.Stub;
                    }
                case ExpressionKind.Assert:
                    {
                        return ExpressionKind.Result;
                    }
                default:
                    {
                        return assignSourceKind;
                    }
            }
        }
    }
}
