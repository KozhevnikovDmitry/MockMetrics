﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class CatchVariableDeclarationEater : VariableDeclarationEater<ICatchVariableDeclaration>
    {
        public CatchVariableDeclarationEater(IEater eater) : base(eater)
        {
        }

        public override Metrics Eat(ISnapshot snapshot, ICatchVariableDeclaration variableDeclaration)
        {
            var metrics = Metrics.Create(Scope.Local, VarType.Stub, Aim.Result);
            metrics.Scope = Scope.Local;
            snapshot.AddVariable(variableDeclaration, metrics);
            return metrics;
        }
    }
}
