using Autofac;
using Common.BL.Search;
using GU.BL.Import;
using GU.BL.Policy;
using GU.BL.Policy.Interface;
using GU.BL.Reporting.Mapping;
using GU.BL.Search;
using GU.BL.Search.Strategy;
using GU.DataModel;

namespace GU.MZ.BL.Modules
{
    public class GuForMzModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mzDomainKey = GetType().Assembly.FullName;

            builder.RegisterType<TaskPolicy>().As<ITaskStatusPolicy>().As<ITaskPolicy>();
            builder.RegisterType<ContentPolicy>().As<IContentPolicy>();
            builder.RegisterType<ContentImporter>().As<IContentImporter>().SingleInstance();
            builder.RegisterType<TaskSearchStrategy>()
                           .As<ISearchStrategy<Task>>()
                           .WithProperty("DomainKey", mzDomainKey)
                           .WithProperty("Searcher", new GuDomainObjectSearcher());

            builder.RegisterType<TaskStatReport>().WithProperty("DomainKey", mzDomainKey);
            builder.RegisterType<TaskDataReport>().WithProperty("DomainKey", mzDomainKey);
            builder.RegisterType<TaskDataListReport>().WithProperty("DomainKey", mzDomainKey);
            builder.RegisterType<TaskInfoReport>().WithProperty("DomainKey", mzDomainKey);
            builder.RegisterType<TaskRegistrReport>().WithProperty("DomainKey", mzDomainKey);
        }
    }
}
