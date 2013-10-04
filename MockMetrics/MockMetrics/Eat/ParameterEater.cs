using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat
{
    public class ParameterEater : IEater<ICSharpParameterDeclaration>
    {
        public Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, ICSharpParameterDeclaration parameterDeclaration)
        {
            throw new NotImplementedException();
        }
    }
}