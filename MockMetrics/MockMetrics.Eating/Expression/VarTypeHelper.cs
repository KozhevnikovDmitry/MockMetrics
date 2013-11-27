using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.Expression
{
    public class VarTypeHelper
    {
        public virtual VarType CastExpressionType(VarType valueType, VarType castType)
        {
            if (valueType >= castType)
            {
                return valueType;
            }
            return castType;
        }

        public virtual VarType InvocationKindByParentReferenceKind(VarType parentKind)
        {
            switch (parentKind)
            {
                case ExpressionKind.TargetCall:
                    {
                        return ExpressionKind.Result;
                    }
                case ExpressionKind.Target:
                    {
                        return ExpressionKind.TargetCall;
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
                default :
                {
                    return assignSourceKind;
                }
            }
        }
    }
}