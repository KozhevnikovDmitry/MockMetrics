using System;

namespace MockMetrics.Eating.Expression
{
    public class ExpressionKindHelper
    {
        public virtual ExpressionKind ValueOfKindAsTypeOfKind(ExpressionKind valueKind, ExpressionKind typeKind)
        {
            if (valueKind == ExpressionKind.TargetCall)
            {
                return ExpressionKind.Result;
            }

            if (valueKind == ExpressionKind.Result)
            {
                return ExpressionKind.Result;
            }

            if (valueKind == ExpressionKind.Mock)
            {
                return ExpressionKind.Mock;
            }

            if (valueKind == ExpressionKind.Target || typeKind == ExpressionKind.Target)
            {
                return ExpressionKind.Target;
            }

            return valueKind;
        }

        public virtual ExpressionKind InvocationKindByParentReferenceKind(ExpressionKind parentKind)
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

        public virtual ExpressionKind ReferenceKindByParentReferenceKind(ExpressionKind parentKind)
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
            }

            throw new NotSupportedException();
        }

        public virtual ExpressionKind KindOfAssignment(ExpressionKind assignSourceKind)
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