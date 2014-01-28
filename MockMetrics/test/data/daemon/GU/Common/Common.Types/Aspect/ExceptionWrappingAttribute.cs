using System;
using Common.Types.Exceptions;
using PostSharp.Aspects;

namespace Common.Types.Aspect
{
    [Serializable]
    public class ExceptionWrappingAttribute : OnMethodBoundaryAspect
    {
        private readonly string _message;

        private readonly Type _exceptionType;

        public ExceptionWrappingAttribute(Type exceptionType, string message)
        {
            if (!exceptionType.IsSubclassOf(typeof(GUException)))
            {
                throw new GUException("Задан неверный тип исключения в аспекте ExceptionWrapping");
            }
            _message = message;
            _exceptionType = exceptionType;
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (!(args.Exception is GUException))
            {
                var pars = new object[2];
                pars[0] = _message;
                pars[1] = args.Exception;
                throw (Exception)Activator.CreateInstance(_exceptionType, pars);
            }
        }
    }
}
