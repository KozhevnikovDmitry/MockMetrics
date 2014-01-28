using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using GU.BL.Policy;
using GU.BL.Policy.Interface;
using GU.Enisey.BL.Converters;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL
{
    public class EniseyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContentPolicy>().As<IContentPolicy>();
            builder.RegisterType<TaskPolicy>().As<ITaskPolicy>();
            builder.RegisterType<InquiryPolicy>().As<IInquiryPolicy>();

            builder.RegisterType<ConverterManager>();

            builder.RegisterAssemblyTypes(this.GetType().Assembly).AssignableTo<ITaskXmlImporter>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AssignableTo<IInquiryXmlImporter>().AsImplementedInterfaces();
        }
    }
}
