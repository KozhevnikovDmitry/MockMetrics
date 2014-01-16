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

    public class NotSingleMockGetInvacotaionArgumentException : EatingException
    {
        public NotSingleMockGetInvacotaionArgumentException(ICSharpNodeEater eater, ICSharpTreeNode node) :
            base("There is not single argument in argument of invocation of Mock.Get<>()", eater, node)
        {
        }
    }
}