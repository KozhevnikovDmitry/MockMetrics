using System;
using System.Linq;
using Autofac;
using Common.BL;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using Common.Types.Exceptions;
using GU.BL;
using GU.BL.DataMapping;
using GU.BL.Policy;
using GU.DataModel;
using GU.MZ.BL.DataMapping;
using GU.MZ.BL.DomainLogic;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Modules
{
    public class MzBlModule : BlModule
    {
        private readonly int _agencyId;

        public MzBlModule(int agencyId) :
            base(new[] { typeof(LicenseDataMapper).Assembly, typeof(TaskDataMapper).Assembly },
                 new[] { typeof(License).Assembly, typeof(Task).Assembly })
        {
            _agencyId = agencyId;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(ResolveDictionaryManager).PropertiesAutowired().SingleInstance();
            builder.Register(ResolveDbUser).SingleInstance();

            builder.RegisterInstance<Func<IDomainDbManager>>(() => new GumzDbManager());

            builder.RegisterModule<GuParseModule>();
            builder.RegisterModule<MzReportModule>();
            builder.RegisterModule<LinkageModule>();
            builder.RegisterModule<SupervisionModule>();
            builder.RegisterModule<GuForMzModule>();
            builder.RegisterModule<LicensingModule>();

            builder.RegisterType<DossierFileBuilder>();
            builder.RegisterType<DiActionContext>();

            var guPreferences = new GuUserPreferences();
            var gumzPreferences = new GumzUserPreferences();

            builder.RegisterInstance(guPreferences);
            builder.RegisterInstance(gumzPreferences);

            RegisterSearchProperties(builder, new IUserPreferences[] { guPreferences, gumzPreferences });

        }

        private IDictionaryManager ResolveDictionaryManager(IComponentContext context)
        {
            try
            {
                using (var db = context.Resolve<Func<IDomainDbManager>>()())
                {
                    var username = context.Resolve<IProviderConfiguration>().User;
                    var userAgencyId = UserPolicy.GetUserAgencyId(username, db);
                    var guDictMan = new GuDictionaryManager(db, userAgencyId);
                    var gumzDictMan = new GumzDictionaryManager(db);
                    gumzDictMan.Merge(guDictMan);
                    return gumzDictMan;
                }
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка инициализации менеджера стравочников", ex);
            }
        }

        private DbUser ResolveDbUser(IComponentContext context)
        {
            try
            {
                var user = 
                    context.Resolve<IDictionaryManager>()
                        .GetDynamicDictionary<DbUser>()
                        .Single(t => t.Name.Trim().ToUpper() == context.Resolve<IProviderConfiguration>().User.Trim().ToUpper()
                                  && t.AgencyId == _agencyId);

                return context.Resolve<IDomainDataMapper<DbUser>>().Retrieve(user.Id);
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GUException("Ошибка получения пользователя приложения", ex);
            }
        }
    }
}
