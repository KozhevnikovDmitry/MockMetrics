using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public interface IMetricNode
    {
        ICSharpTreeNode Node { get; }

        int Depth { get; }
    }

    public interface IMetricVariable : IMetricNode
    {
        Dictionary<Guid, Variable> VarTypes { get; }

        IMetricVariable AddVarType(Variable variable);

        Variable GetVarType();

        bool NodeEquals(ICSharpTreeNode Node);

        bool NodeEquals(IDeclaredElement Node);
    }

    public interface IMetricMockOption : IMetricNode
    {
        FakeOption FakeOption { get; }
    }
}