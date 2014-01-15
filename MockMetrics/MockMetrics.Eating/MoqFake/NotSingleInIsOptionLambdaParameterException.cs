using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqFake
{
    public class NotSingleInIsOptionLambdaParameterException : EatingException
    {
        public NotSingleInIsOptionLambdaParameterException(ICSharpNodeEater eater, ICSharpTreeNode node) : 
            base("There is not single argument of invocation of It.Is<>()", eater, node)
        {
        }
    }
}