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
                        break;
                    }
                case ExpressionKind.Target:
                    {
                        return ExpressionKind.TargetCall;
                        break;
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
                        break;
                    }
                case ExpressionKind.Target:
                    {
                        return ExpressionKind.Result;
                        break;
                    }
                case ExpressionKind.Stub:
                    {
                        return ExpressionKind.Stub;
                        break;
                    }
                case ExpressionKind.Mock:
                    {
                        return ExpressionKind.Result;
                        break;
                    }
                case ExpressionKind.StubCandidate:
                    {
                        return ExpressionKind.StubCandidate;
                        break;
                    }
                case ExpressionKind.Assert:
                    {
                        return ExpressionKind.Result;
                        break;
                    }
                case ExpressionKind.None:
                    {
                        return ExpressionKind.None;
                        break;
                    }
                case ExpressionKind.Result:
                    {
                        return ExpressionKind.Result;
                        break;
                    }
            }

            throw new NotSupportedException();
        }
    }
}