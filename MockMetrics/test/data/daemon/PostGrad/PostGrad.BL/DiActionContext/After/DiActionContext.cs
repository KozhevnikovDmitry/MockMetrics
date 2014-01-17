using System;
using PostGrad.Core.BL;

namespace PostGrad.BL.DiActionContext.After
{
    public interface IDiActionContext
    {
        void Act<T>(Action<T> action);
        void Act<T1, T2>(Action<T1, T2> action);
        void Act<T1, T2, T3>(Action<T1, T2, T3> action);
        void Act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action);
        TResult Get<T, TResult>(Func<T, TResult> func);
        TResult Get<T1, T2, TResult>(Func<T1, T2, TResult> func);
        TResult Get<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func);
        TResult Get<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func);
    }

    public class DiActionContext : IDiActionContext
    {
        private readonly ILifetimeScope _container;

        public DiActionContext(ILifetimeScope container)
        {
            _container = container;
        }
        
        public void Act<T>(Action<T> action)
        {
            action(_container.Resolve<T>());
        }

        public void Act<T1, T2>(Action<T1, T2> action)
        {
            action(_container.Resolve<T1>(), _container.Resolve<T2>());
        }

        public void Act<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            action(_container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>());
        }

        public void Act<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            action(_container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>(), _container.Resolve<T4>());
        }
        
        public TResult Get<T, TResult>(Func<T, TResult> func)
        {
            return func(_container.Resolve<T>());
        }
        
        public TResult Get<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            return func(_container.Resolve<T1>(), _container.Resolve<T2>());
        }
        
        public TResult Get<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            return func(_container.Resolve<T1>(), _container.Resolve<T2>(),_container.Resolve<T3>());
        }
        
        public TResult Get<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            return func(_container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>(), _container.Resolve<T4>());
        }
        
        //public FluentDiActionContext<TResult> Fluent<T, TResult>(Func<T, TResult> func)
        //{
        //    return new FluentDiActionContext<TResult>(func(_container.Resolve<T>()), _container);
        //}
        
        //public FluentDiActionContext<TResult> Fluent<T1, T2, TResult>(Func<T1, T2, TResult> func)
        //{
        //    return new FluentDiActionContext<TResult>(func(_container.Resolve<T1>(), _container.Resolve<T2>()), _container);
        //}
        
        //public FluentDiActionContext<TResult> Fluent<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        //{
        //    return new FluentDiActionContext<TResult>(func(_container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>()), _container);
        //}
        
        //public FluentDiActionContext<TResult> Fluent<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        //{
        //    return new FluentDiActionContext<TResult>(func(_container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>(), _container.Resolve<T4>()), _container);
        //}
    }
}
