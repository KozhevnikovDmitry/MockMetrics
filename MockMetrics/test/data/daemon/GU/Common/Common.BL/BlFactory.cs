using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.BL.Validation;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL
{
    public class BlFactory : IDomainLogicContainer
    {
        public IContainer IocContainer { get; private set; }

        public BlFactory(IContainer iocContainer)
        {
            IocContainer = iocContainer;
        }
        
        public IDomainDataMapper<T> ResolveDataMapper<T>() where T : IPersistentObject
        {
            try
            {
                return IocContainer.Resolve<IDomainDataMapper<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new BLLException(string.Format("Класс IDomainDataMapper<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException(string.Format("Ошибка при создании экземпляра типа IDomainDataMapper<{0}>", typeof(T).Name), ex);
            }
        }

        public IDomainValidator<T> ResolveValidator<T>() where T : IPersistentObject
        {
            try
            {
                return IocContainer.Resolve<IDomainValidator<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new BLLException(string.Format("Класс IDomainValidator<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException(string.Format("Ошибка при создании экземпляра типа IDomainValidator<{0}>", typeof(T).Name), ex);
            }
        }

        public T ResolveDomainDependent<T>(params object[] parameters) where T : IDomainDependent
        {
            var pars = parameters.Select((t, index) => new PositionalParameter(index, t)).ToList();
            var result = IocContainer.Resolve<T>(pars);
            result.SetDomainKey(typeof(T).Assembly.FullName);
            return result;
        }
    }
}
