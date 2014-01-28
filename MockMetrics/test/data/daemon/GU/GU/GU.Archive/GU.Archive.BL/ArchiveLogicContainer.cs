using System.Collections.Generic;
using System.Reflection;

using Autofac;

using Common.BL;
using Common.BL.DictionaryManagement;

using GU.Archive.BL.Reporting.Mapping;

namespace GU.Archive.BL
{
    public class ArchiveLogicContainer : DomainLogicContainer
    {
        public ArchiveLogicContainer(IEnumerable<Assembly> assemblies, IDictionaryManager dictionaryManager)
            : base(assemblies, dictionaryManager)
        {
            string domainKey = typeof(TaskPostStatReport).Assembly.FullName;
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TaskPostStatReport>().WithProperty("DomainKey", domainKey);
            containerBuilder.Update(IocContainer);
        }

        public TaskPostStatReport ResolveTaskPostStatReport()
        {
            return IocContainer.Resolve<TaskPostStatReport>();
        }
    }
}
