using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;

namespace Common.Types.Aspect
{
    [Serializable]
    public sealed class HasParameterAttribute : OnMethodBoundaryAspect
    {
        private List<int> _notNullArguments;

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            _notNullArguments= new List<int>();
            foreach(var arg in method.GetParameters().Where(t => t.GetCustomAttributes(typeof(NotNullAttribute),true).Count() != 0))
            {
                _notNullArguments.Add(method.GetParameters().ToList().IndexOf(arg));
            }
        }

        public override void OnEntry(MethodExecutionArgs eventArgs)
        {
            if(_notNullArguments != null)
            {
                if (_notNullArguments.Any(item => eventArgs.Arguments[item] == null))
                {
                    throw new ArgumentNullException();
                }
            }
        }
    }

    [Serializable]
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}
