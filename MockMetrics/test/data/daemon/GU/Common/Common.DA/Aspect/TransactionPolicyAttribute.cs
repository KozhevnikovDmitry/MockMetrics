using System;
using System.Linq;
using System.Reflection;
using Common.DA.Interface;
using Common.Types.Exceptions;
using PostSharp.Aspects;

namespace Common.DA.Aspect
{
    [Serializable]
    public sealed class TransactionPolicyAttribute : OnMethodBoundaryAspect
    {
        private IDomainDbManager _dbManager;

        public override void OnEntry(MethodExecutionArgs args)
        {
            _dbManager =
                (IDomainDbManager)
                args.Arguments.SingleOrDefault(a => a.GetType().IsSubclassOf(typeof (DomainDbManager)));
            _dbManager.BeginDomainTransaction();
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception is BLLException)
            {
                throw args.Exception;
            }
            if (args.Exception is TransactionControlException)
            {
                throw new BLLException(args.Exception);
            }
            _dbManager.RollbackDomainTransaction();
            throw new BLLException(string.Format("Ошибка при проведении транзакции {0}.{1}.",
                                                 args.Instance.GetType().Name,
                                                 args.Method.Name),
                                   args.Exception);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            try
            {
                _dbManager.CommitDomainTransaction();
            }
            catch (TransactionControlException ex)
            {
                throw new BLLException(ex);
            }
        }

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            var dbInfo = method.GetParameters().SingleOrDefault(t => t.ParameterType.GetInterface(typeof(IDomainDbManager).Name) != null || t.ParameterType == typeof(IDomainDbManager));
            if (dbInfo == null)
            {
                throw new BLLException("Неверное применение аспекта. Среди аргументов метода отсутвует IDomainDbManager");
            }
        }
    }
}
