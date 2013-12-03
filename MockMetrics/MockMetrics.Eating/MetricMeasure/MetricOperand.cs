using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eating.MetricMeasure
{
    public class MetricOperand : IMetricOperand
    {
        public MetricOperand(ICSharpTreeNode node, Scope scope, Aim aim, VarType varType, Operand operand)
        {
            Node = node;
            Aims = new Dictionary<Guid, Aim>();
            VarTypes = new Dictionary<Guid, VarType>();

            Scope = scope;
            Operand = operand;
            Aims[Guid.NewGuid()] = aim;
            VarTypes[Guid.NewGuid()] = varType;
        }

        public ICSharpTreeNode Node { get; private set; }
        public int Depth { get; private set; }
        public Scope Scope { get; private set; }
        public Operand Operand { get; set; }
        public Dictionary<Guid, Aim> Aims { get; private set; }
        public Dictionary<Guid, VarType> VarTypes { get; private set; }
        public IMetricOperand AddAim(Aim aim)
        {
            if (!Aims.ContainsValue(aim))
            {
                Aims[Guid.NewGuid()] = aim;
            }

            return this;
        }

        public IMetricOperand AddVarType(VarType varType)
        {
            if (!VarTypes.ContainsValue(varType))
            {
                VarTypes[Guid.NewGuid()] = varType;
            }

            return this;
        }
    }
}