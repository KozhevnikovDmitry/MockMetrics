using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac;

using Common.BL;
using Common.BL.DictionaryManagement;
using Common.BL.Search;
using Common.BL.Validation;

using GU.BL.DomainValidation;
using GU.BL.Policy.Interface;
using GU.BL.Reporting.Mapping;
using GU.BL.Search;
using GU.BL.Search.Strategy;
using GU.DataModel;

namespace GU.BL.Policy
{
    /// <summary>
    /// Контейнер классов бизнес логики предметной области "Работа с заявлениями"
    /// </summary>
    public class GuDomainLogicContainer : DomainLogicContainer
    {
        /// <summary>
        /// Контейнер классов бизнес логики предметной области "Работа с заявлениями"
        /// </summary>
        /// <param name="assembly">Сборка с классами бизнес-логики</param>
        /// <param name="dbUser">Зарегистрированный пользователь</param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public GuDomainLogicContainer(Assembly assembly, DbUser dbUser, IDictionaryManager dictionaryManager)
            : this(new List<Assembly> { assembly }, dbUser, dictionaryManager)
        {
        }

        /// <summary>
        /// Контейнер классов бизнес логики предметной области "Работа с заявлениями"
        /// </summary>
        /// <param name="assemblies">Список сборок с классами бизнес-логики</param>
        /// <param name="dbUser">Зарегистрированный пользователь</param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public GuDomainLogicContainer(IEnumerable<Assembly> assemblies, DbUser dbUser, IDictionaryManager dictionaryManager)
            : base(assemblies, dictionaryManager)
        {
            var containerBuilder = new ContainerBuilder();

            var domainKey = typeof(TaskSearchStrategy).Assembly.FullName;

            containerBuilder.RegisterInstance(dbUser);
            containerBuilder.RegisterType<TaskPolicy>().As<ITaskPolicy>();
            containerBuilder.RegisterType<ContentPolicy>().As<IContentPolicy>();
            containerBuilder.RegisterType<TaskSearchStrategy>()
                            .As<ISearchStrategy<Task>>()
                            .WithProperty("DomainKey",domainKey)
                            .WithProperty("Searcher", new GuDomainObjectSearcher());

            containerBuilder.RegisterType<TaskStatReport>().WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<TaskDataReport>().WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<TaskDataListReport>().WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<TaskInfoReport>().WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<TaskRegistrReport>().WithProperty("DomainKey", domainKey);

            containerBuilder.Update(IocContainer);
        }

        public GuDomainLogicContainer(IContainer container)
            : base(container)
        {
            
        }

        /// <summary>
        /// Возвращает экземпляр политики управления заявками.
        /// </summary>
        /// <returns>Политика управления заявками</returns>
        public ITaskPolicy ResolveTaskPolicy()
        {
            return IocContainer.Resolve<ITaskPolicy>();
        }

        /// <summary>
        /// Возвращает экземпляр стратегии поиска заявок.
        /// </summary>
        /// <returns>Стратегия поиска заявок</returns>
        public ISearchStrategy<Task> ResolveTaskSearchStrategy()
        {
            return IocContainer.Resolve<ISearchStrategy<Task>>();
        }

        public TaskStatReport ResolveTaskStatReport()
        {
            return IocContainer.Resolve<TaskStatReport>();
        }

        public TaskRegistrReport ResolveTaskRegistrReport(string username, DateTime startDate, DateTime endDate)
        {
            return IocContainer.Resolve<TaskRegistrReport>(new NamedParameter("username", username),
                                                           new NamedParameter("startDate", startDate),
                                                           new NamedParameter("endDate", endDate));
        }

        public TaskInfoReport ResolveTaskInfoReport(Task task)
        {
            return IocContainer.Resolve<TaskInfoReport>(new NamedParameter("task", task));
        }

        public TaskDataReport ResolveTaskDataReport(Task task, string username)
        {
            return IocContainer.Resolve<TaskDataReport>(
                new NamedParameter("task", task), new NamedParameter("username", username));
        }

        public TaskDataListReport ResolveTaskDataListReport(string username)
        {
            return IocContainer.Resolve<TaskDataListReport>(new NamedParameter("username", username));
        }
    }
}
