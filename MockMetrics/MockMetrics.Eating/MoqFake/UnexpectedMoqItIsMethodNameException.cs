using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqFake
{
    public class UnexpectedMoqItIsMethodNameException :EatingException
    {
        public UnexpectedMoqItIsMethodNameException(string methodName, ICSharpNodeEater eater, ICSharpTreeNode node)
            : base(string.Format("Unexpected It.Is Moq-stub invocation method name [{0}]", methodName), eater, node)
        {
        }

    }
}