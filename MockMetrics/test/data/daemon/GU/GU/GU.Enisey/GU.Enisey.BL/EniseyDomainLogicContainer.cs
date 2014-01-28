using System.Collections.Generic;
using System.Reflection;

using Autofac;

using Common.BL;
using Common.BL.DictionaryManagement;

using GU.BL.Policy;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL
{
    public class EniseyDomainLogicContainer : DomainLogicContainer
    {
        public EniseyDomainLogicContainer(Assembly assembly, DbUser dbUser, IDictionaryManager dictionaryManager)
            : this(new List<Assembly> { assembly }, dbUser, dictionaryManager)
        {
        }

        public EniseyDomainLogicContainer(IEnumerable<Assembly> assemblies, DbUser dbUser, IDictionaryManager dictionaryManager)
            : base(assemblies, dictionaryManager)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(dbUser);
            containerBuilder.RegisterModule<EniseyModule>();
            
            containerBuilder.Update(IocContainer);
        }
    }
}
