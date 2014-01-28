using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Common.BL;
using Common.BL.DictionaryManagement;
using Common.BL.Search;
using GU.BL.Import;
using GU.HQ.BL.DomainLogic.AcceptTask;
using GU.HQ.BL.DomainLogic.AcceptTask.Interface;
using GU.HQ.BL.DomainLogic.QueueManage;
using GU.HQ.BL.Policy;
using GU.HQ.BL.Reporting.Mapping;
using GU.HQ.BL.Search;
using GU.HQ.BL.Search.Strategy;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainLogic
{
    public class HqLogicContainer : DomainLogicContainer
    {
        /// <summary>
        /// Контейнер классов бизнес логики предметной области "Постановки в очередь на муниципальное жильё"
        /// </summary>
        /// <param name="assemblies">Список сборок с классами бизнес-логики</param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public HqLogicContainer(IEnumerable<Assembly> assemblies, IDictionaryManager dictionaryManager)
            : base(assemblies, dictionaryManager)
        {
            var containerBuilder = new ContainerBuilder();
            var domainKey = this.GetType().Assembly.FullName;

            containerBuilder.RegisterType<TaskDataParser>().As<ITaskDataParser>();
            containerBuilder.RegisterType<ClaimStatusPolicy>().As<IClaimStatusPolicy>()
                .WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<QueueManager>().As<IQueueManager>()
                .WithProperty("DomainKey", domainKey);
            containerBuilder.RegisterType<ClaimSearchStrategy>()
                           .As<ISearchStrategy<Claim>>()
                           .WithProperty("DomainKey", domainKey)
                           .WithProperty("Searcher", new HqDomainObjectSearcher());

            containerBuilder.RegisterType<ClaimRegistrReport>()
                .WithProperty("DomainKey", domainKey);

            containerBuilder.Update(IocContainer);
        }
        
        /// <summary>
        /// Получить класс преобразователь объекта Task в Claim
        /// </summary>
        /// <param name="task">Объект заявка</param>
        /// <returns>преобразователь объекта Task в Claim</returns>
        public ITaskDataParser ResolveTaskDataParser()
        {
            return IocContainer.Resolve<ITaskDataParser>();
        }


        public IClaimStatusPolicy ResolveClaimStatusPolicy()
        {
            return IocContainer.Resolve<IClaimStatusPolicy>();
        }


        public IQueueManager ResolveQueueManager()
        {
            return IocContainer.Resolve<IQueueManager>();
        }

        public ClaimRegistrReport ResolveClaimRegistrReport(string username, DateTime startDate, DateTime endDate)
        {
            return IocContainer.Resolve<ClaimRegistrReport>(new NamedParameter("username", username),
                                                            new NamedParameter("startDate", startDate),
                                                            new NamedParameter("endDate", endDate));
        }
    }
}
