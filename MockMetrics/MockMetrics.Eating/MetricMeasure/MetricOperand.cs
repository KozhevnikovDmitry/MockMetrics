using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricOperand : IMetricOperand
    {
        public ICSharpTreeNode Node { get; private set; }

        public MetricOperand(ICSharpTreeNode node, Scope scope, Variable variable, Operand operand)
        {
            VarTypes = new Dictionary<Guid, Variable>();
            
            Node = node;
            Scope = scope;
            Operand = operand;
            VarTypes[Guid.NewGuid()] = variable;
        }

        public int Depth { get; private set; }
        public Scope Scope { get; private set; }
        public Operand Operand { get; set; }
        public Dictionary<Guid, Variable> VarTypes { get; private set; }

        public IMetricOperand AddVarType(Variable variable)
        {
            if (!VarTypes.ContainsValue(variable))
            {
                VarTypes[Guid.NewGuid()] = variable;
            }

            return this;
        }

        public bool NodeEquals(ICSharpTreeNode node)
        {
            throw new NotImplementedException();
        }

        public bool NodeEquals(IDeclaredElement node)
        {
            throw new NotImplementedException();
        }
    }
}