using System;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace MockMetrics.Eat
{
    public interface IEater<T> where T : ICSharpTreeNode
    {
        Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, T parameterDeclaration);
    }

    public class UnitTestEater 
    {
        public Snapshot EatUnitTest(IMethodDeclaration unitTest)
        {
            throw new NotImplementedException();
        }
    }

    public class RegularParameterEater : IEater<IRegularParameterDeclaration>
    {
        public Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, IRegularParameterDeclaration parameterDeclaration)
        {
            throw new NotImplementedException();
        }
    }

    public class BlockEater : IEater<IRegularParameterDeclaration>
    {
        public Snapshot Eat(Snapshot snapshot, IMethodDeclaration unitTest, IRegularParameterDeclaration parameterDeclaration)
        {
            throw new NotImplementedException();
        }
    }

    public class PostEater
    {
        public void PostEat(Snapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
