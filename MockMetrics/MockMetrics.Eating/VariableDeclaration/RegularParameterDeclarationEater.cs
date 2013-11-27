using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Eating.VariableDeclaration
{
    public class RegularParameterDeclarationEater : VariableDeclarationEater<IRegularParameterDeclaration>
    {
        public RegularParameterDeclarationEater(IEater eater)
            : base(eater)
        {
        }

        // TODO : investigate, what is IRegularParameterDeclaration
        public override VarType Eat(ISnapshot snapshot, IRegularParameterDeclaration variableDeclaration)
        {
           return VarType.None;
        }
    }
}