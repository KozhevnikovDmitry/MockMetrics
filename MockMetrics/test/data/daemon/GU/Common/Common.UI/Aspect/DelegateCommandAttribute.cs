using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Common.Types.Exceptions;
using Microsoft.Practices.Prism.Commands;
using PostSharp.Aspects;
using PostSharp.Reflection;

namespace Common.UI.Aspect
{
    [Serializable]
    public class DelegateCommandAttribute : LocationInterceptionAspect
    {
        private readonly string _methodName;

        private ICommand _command;

        private MethodInfo _methodInfo;
        
        public DelegateCommandAttribute(string methodName)
        {
            _methodName = methodName;
        }

        public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
        {
            if (targetLocation.LocationType != typeof(DelegateCommand))
            {
                throw new GUException(string.Format("Атрибутов DelegateCommand размечено свойство не подходящего типа"));
            }

            var methods = targetLocation.PropertyInfo.ReflectedType.GetMethods().Where(m => m.Name == _methodName);

            if (!methods.Any())
            {
                throw new GUException(string.Format("Неверно задано имя метода команды {0}.{1}. Метод не существует или является закрытым.", 
                                                    targetLocation.PropertyInfo.ReflectedType.Name, 
                                                    targetLocation.PropertyInfo.Name));
            }

            _methodInfo = methods.SingleOrDefault(m => !m.GetParameters().Any() && m.ReturnType == typeof(void));

            if(_methodInfo == null)
            {
                throw new GUException(string.Format("Заданный метод имеет не подходящую сигнатуру"));
            }
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (_command == null)
            {
                _command = new DelegateCommand(() => _methodInfo.Invoke(args.Instance, null));
            }
            args.Value = _command;
        }
    }
}
