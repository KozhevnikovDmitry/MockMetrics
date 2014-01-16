using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqFake
{
    public class NotSingleMoqFakeOptionLambdaParameterException : EatingException
    {
        public NotSingleMoqFakeOptionLambdaParameterException(ICSharpNodeEater eater, ICSharpTreeNode node) : 
            base("There is not single lambda parameter in expression of Moq fake option", eater, node)
        {
        }
    }
}