using System;
using Common.Types.Exceptions;
using PostSharp.Aspects;

namespace Common.UI.Aspect
{
    [Serializable]
    public sealed class NoticeExceptionAttribute : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if(args.Exception is GUException)
            {
                NoticeUser.ShowError(args.Exception);
            }
            else
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", args.Exception));
            }
            args.FlowBehavior = FlowBehavior.Return;
        }
    }
}
