using System;
using Autofac;

namespace GU.MZ.BL.DomainLogic
{
    public class FluentDiActionContext<TClient>
    {
        private TClient _client;

        private readonly ILifetimeScope _container;

        public FluentDiActionContext(TClient client, ILifetimeScope container)
        {
            _client = client;
            _container = container;
        }

        public FluentDiActionContext<TClient> Act(Func<TClient, TClient> func)
        {
            _client = func(_client);
            return this;
        }

        public FluentDiActionContext<TClient> Act<T>(Func<TClient, T, TClient> func)
        {
            _client = func(_client, _container.Resolve<T>());
            return this;
        }

        public FluentDiActionContext<TClient> Act<T1, T2>(Func<TClient, T1, T2, TClient> func)
        {
            _client = func(_client, _container.Resolve<T1>(), _container.Resolve<T2>());
            return this;
        }

        public FluentDiActionContext<TClient> Act<T1, T2, T3>(Func<TClient, T1, T2, T3, TClient> func)
        {
            _client = func(_client, _container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>());
            return this;
        }

        public FluentDiActionContext<TClient> Act<T1, T2, T3, T4>(Func<TClient, T1, T2, T3, T4, TClient> func)
        {
            _client = func(_client, _container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>(), _container.Resolve<T4>());
            return this;
        }

        public TResult Get<T, TResult>(Func<TClient, T, TResult> func)
        {
            return func(_client, _container.Resolve<T>());
        }

        public TResult Get<T1, T2, TResult>(Func<TClient, T1, T2, TResult> func)
        {
            return func(_client, _container.Resolve<T1>(), _container.Resolve<T2>());
        }

        public TResult Get<T1, T2, T3, TResult>(Func<TClient, T1, T2, T3, TResult> func)
        {
            return func(_client, _container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>());
        }

        public TResult Get<T1, T2, T3, T4, TResult>(Func<TClient, T1, T2, T3, T4, TResult> func)
        {
            return func(_client, _container.Resolve<T1>(), _container.Resolve<T2>(), _container.Resolve<T3>(), _container.Resolve<T4>());
        }
    }
}