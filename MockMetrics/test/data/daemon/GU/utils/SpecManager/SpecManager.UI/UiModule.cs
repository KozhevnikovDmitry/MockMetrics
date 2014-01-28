using System;
using System.Windows.Controls;

using Autofac;

using SpecManager.BL.Model;
using SpecManager.UI.View.SpecView;
using SpecManager.UI.ViewModel;
using SpecManager.UI.ViewModel.SpecSourceViewModel;
using SpecManager.UI.ViewModel.SpecViewModel;

namespace SpecManager.UI
{
    public class UiModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainVm>();
            builder.RegisterType<DbConfigVm>();
            builder.RegisterType<OpenSpecFromDbVm>();
            builder.RegisterType<SpecVm>().Keyed<SpecVmBase>(typeof(Spec));
            builder.RegisterType<SpecNodeVm>().Keyed<SpecVmBase>(typeof(SpecNode));
            builder.RegisterType<SpecView>().Keyed<UserControl>(typeof(Spec));
            builder.RegisterType<SpecNodeView>().Keyed<UserControl>(typeof(SpecNode));
            builder.RegisterType<SpecTreeClipboard>().SingleInstance();

            builder.Register<Func<DbConfigVm>>(c => c.Resolve<IComponentContext>().Resolve<DbConfigVm>);
            builder.Register<Func<OpenSpecFromDbVm>>(c => c.Resolve<IComponentContext>().Resolve<OpenSpecFromDbVm>);
        }
    }
}
