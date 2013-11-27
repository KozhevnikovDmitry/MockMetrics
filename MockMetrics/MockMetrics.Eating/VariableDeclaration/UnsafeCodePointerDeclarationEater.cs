﻿using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class UnsafeCodeFixedPointerDeclarationEater : VariableDeclarationEater<IUnsafeCodeFixedPointerDeclaration>
    {
        private readonly IVariableInitializerEater _variableInitializerEater;

        public UnsafeCodeFixedPointerDeclarationEater(IEater eater, IVariableInitializerEater variableInitializerEater)
            : base(eater)
        {
            _variableInitializerEater = variableInitializerEater;
        }

        public override VarType Eat(ISnapshot snapshot, IUnsafeCodeFixedPointerDeclaration variableDeclaration)
        {
           var metrics = _variableInitializerEater.Eat(snapshot, variableDeclaration.Initial);
           snapshot.AddVariable(variableDeclaration, Scope.Local, metrics.First, metrics.Second);
           return metrics.Second;
        }
    }
}
