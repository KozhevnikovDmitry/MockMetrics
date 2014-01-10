using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricVariable : IMetricVariable
    {
        public ICSharpTreeNode Node { get; private set; }

        public MetricVariable(ICSharpTreeNode node, Variable variable)
        {
            VarTypes = new Dictionary<Guid, Variable>();
            Node = node;
            VarTypes[Guid.NewGuid()] = variable;
        }

        public int Depth { get; private set; }
        public Dictionary<Guid, Variable> VarTypes { get; private set; }

        public IMetricVariable AddVarType(Variable variable)
        {
            if (!VarTypes.ContainsValue(variable))
            {
                VarTypes[Guid.NewGuid()] = variable;
            }

            return this;
        }

        public bool NodeEquals(ICSharpTreeNode node)
        {
            return Node.Equals(node);
        }

        public bool NodeEquals(IDeclaredElement node)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0}, Variable=[{1}]; VarTypeCount=[{2}];", Node, VarTypes.Values.Max(), VarTypes.Count);
        }
    }



    public class MetricMockOption : IMetricMockOption
    {
        public ICSharpTreeNode Node { get; private set; }
        public int Depth { get; private set; }
        public FakeOption FakeOption { get; private set; }

        public MetricMockOption(ICSharpExpression expression, FakeOption fakeOption)
        {
            Node = expression;
            FakeOption = fakeOption;
        }

        public override string ToString()
        {
            return string.Format("{0}, FakeOption=[{1}];", Node, FakeOption);
        }
    }
}