using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqStub
{
    public class MoqStubWrongSyntaxException : EatingException
    {
        public MoqStubWrongSyntaxException(string message, ICSharpNodeEater eater, ICSharpTreeNode node) 
            : base(message, eater, node)
        {
        }
    }
}