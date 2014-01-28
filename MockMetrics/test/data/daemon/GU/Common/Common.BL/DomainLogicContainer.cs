using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.BL.Search;
using Common.BL.Validation;
using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;

namespace Common.BL
{
    /// <summary>
    /// Контейнер классов бизнес-логики
    /// </summary>
    public class DomainLogicContainer : IDomainLogicContainer
    {
        /// <summary>
        /// IoC контейнер хранящий всё зависимости
        /// </summary>
        public IContainer IocContainer { get; private set; }

        /// <summary>
        /// Класс контейнер DataMapper'ов доменных объектов.
        /// </summary>
        /// <param name="assembly">Сборка с классами мапперов</param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public DomainLogicContainer(Assembly assembly, IDictionaryManager dictionaryManager)
            : this(new List<Assembly> { assembly }, dictionaryManager)
        {
            
        }

        /// <summary>
        ///  Класс контейнер DataMapper'ов доменных объектов.
        /// </summary>
        /// <param name="assemblies">Список сборок с классами мапперами</param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public DomainLogicContainer(IEnumerable<Assembly> assemblies, IDictionaryManager dictionaryManager)
        {
            var assemblyArray = assemblies as Assembly[] ?? assemblies.ToArray();
           
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance<IDomainContext>(new DomainContext.DomainContext(assemblyArray));
            containerBuilder.RegisterInstance(dictionaryManager);

            containerBuilder.RegisterAssemblyTypes(assemblyArray)
                            .Where(x => x.IsAssignableToGenericType(typeof(IDomainValidator<>)))
                            .AsImplementedInterfaces()
                            .SingleInstance();

            foreach (var assembly in assemblyArray)
            {
                containerBuilder.RegisterAssemblyTypes(assembly)
                                .Where(x => x.IsAssignableToGenericType(typeof(IDomainDataMapper<>)))
                                .WithProperty("DomainKey", assembly.FullName)
                                .PropertiesAutowired(PropertyWiringFlags.AllowCircularDependencies)
                                .AsImplementedInterfaces()
                                .SingleInstance();

                containerBuilder.RegisterAssemblyTypes(assembly)
                                .Where(x => x.IsAssignableToGenericType(typeof(ISearchStrategy<>)))
                                .WithProperty("DomainKey", assembly.FullName)
                                .WithProperty("Searcher", Activator.CreateInstance(assembly.GetTypes().Single(t => typeof(IDomainObjectSearcher).IsAssignableFrom(t))))
                                .AsImplementedInterfaces();
            }

            this.IocContainer = containerBuilder.Build();
        }

        protected DomainLogicContainer(IContainer container)
        {
            IocContainer = container;
        }

        /// <summary>
        /// Возвращает экземпляр произвольного доменно-зависимого класса   
        /// </summary>
        /// <typeparam name="T">Класс</typeparam>
        /// <returns>Экземпляр доменно-зависимого произвольного класса</returns>
        public T ResolveDomainDependent<T>(params object[] parameters) where T : IDomainDependent
        {
            if (!IocContainer.IsRegistered<T>())
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<T>();
                builder.Update(IocContainer);
            }

            var pars = parameters.Select((t, index) => new PositionalParameter(index, t)).ToList();
            var result = IocContainer.Resolve<T>(pars);
            result.SetDomainKey(typeof(T).Assembly.FullName);
            return result;
        }

        /// <summary>
        /// Возвращает экземпляр класса DataMapper'а.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <returns>Экземпляр DataMapper'а</returns>
        /// <exception cref="BLLException">Класс IDomainDataMapper не зарегистрирован в контейнере</exception>
        public IDomainDataMapper<T> ResolveDataMapper<T>() where T : IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<IDomainDataMapper<T>>();
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
        
        /// <summary>
        /// Возвращает экземпляр класса Validator'а.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <returns>Экземпляр DataMapper'а</returns>
        /// <exception cref="BLLException">Класс IDomainValidator не зарегистрирован в контейнере</exception>
        public IDomainValidator<T> ResolveValidator<T>() where T : IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<IDomainValidator<T>>();
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
    }
}
