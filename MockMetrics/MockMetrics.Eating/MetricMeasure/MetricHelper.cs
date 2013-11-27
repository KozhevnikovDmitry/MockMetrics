using System;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricHelper
    {
        public virtual Aim AimOfExpression(VarType varType, ICSharpExpression expression)
        {
            throw new NotImplementedException();
        }

        public virtual Metrics MetricsForType(ISnapshot snapshot, IType type)
        {
            throw new NotImplementedException();
        }

        public virtual Metrics CastExpressionType(Metrics valueMetrics, Metrics castMetrics)
        {
            if (valueMetrics.VarType >= castMetrics.VarType)
            {
                valueMetrics.VarType = castMetrics.VarType;
            }
            return valueMetrics;
        }

        public virtual Call CallByParentVarType(VarType parentVarType)
        {
            switch (parentVarType)
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
                        return Call.Service;
                    }
                case VarType.External:
                    {
                        return Call.Service;
                    }
            }

            return Call.None;
        }

        public virtual Metrics RefMetricsByParentMetrics(Metrics parentMetrics)
        {
            throw new NotImplementedException();
        }

        public virtual Metrics MetricsOfAssignment(Metrics sourceMetrics)
        {
            throw new NotImplementedException();
        }
    }
}
