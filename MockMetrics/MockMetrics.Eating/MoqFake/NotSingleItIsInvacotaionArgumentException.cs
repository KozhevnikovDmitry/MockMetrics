using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqFake
{
    public class NotSingleItIsInvacotaionArgumentException : EatingException
    {
        public NotSingleItIsInvacotaionArgumentException(ICSharpNodeEater eater, ICSharpTreeNode node) :
            base("There is not single labmda parameter in argument of invocation of It.Is<>()", eater, node)
        {
        }
    }
}