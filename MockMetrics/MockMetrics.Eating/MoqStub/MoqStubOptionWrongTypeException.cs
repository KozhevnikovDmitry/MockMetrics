using JetBrains.ReSharper.Psi.CSharp.Tree;
using MockMetrics.Eating.Exceptions;

namespace MockMetrics.Eating.MoqStub
{
    public class MoqStubOptionWrongTypeException : EatingException
    {
        public MoqStubOptionWrongTypeException(ICSharpNodeEater eater, ICSharpTreeNode node) 
            : base(string.Format("Moq-stub options has wrong type. Expected conditional-and, equility, invocation, reference expressions. But was [{0}]", node.GetType()), eater, node)
        {
        }
    }

    public class MoqStubOptionTargetWrongTypeException : EatingException
    {
        public MoqStubOptionTargetWrongTypeException(ICSharpNodeEater eater, ICSharpTreeNode node)
            : base(string.Format("Moq-stub option target has wrong type. Expected invocation or reference expression. But was [{0}]", node.GetType()), eater, node)
        {
        }
    }
}