using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface IMetricNode
    {
        ICSharpTreeNode Node { get; }

        int Depth { get; }
    }
    public interface IMetricOperand : IMetricNode
    {
        Scope Scope { get; }

        Operand Operand { get; set; }

        Dictionary<Guid, Aim> Aims { get; }

        Dictionary<Guid, VarType> VarTypes { get; }

        IMetricOperand AddAim(Aim aim);

        IMetricOperand AddVarType(VarType varType);
    }

    public interface IMetricCall : IMetricNode
    {
        Scope Scope { get; }

        Call Call { get; }
    }

    public interface IMetricMockOption : IMetricNode
    {
        FakeOption FakeOption { get; }
    }

}