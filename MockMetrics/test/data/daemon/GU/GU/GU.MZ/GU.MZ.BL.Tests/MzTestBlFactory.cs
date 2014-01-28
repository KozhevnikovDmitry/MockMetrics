using System;
using Autofac;
using Common.BL;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.MZ.BL.DomainLogic;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.Licensing;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Tests
{
    public class MzTestBlFactory : BlFactory
    {
        public MzTestBlFactory(IContainer container) : base(container)
        {
        }

        public virtual ITaskPolicy GetTaskPolicy()
        {
            return IocContainer.Resolve<ITaskPolicy>();
        }

        public virtual LicenseBlankReport GetLicenseBlankReport(License license)
        {
            return IocContainer.Resolve<LicenseBlankReport>(new NamedParameter("license", license));
        }

        public virtual FullActivityDataReport GetFullActivityDataReport(LicensedActivity licensedActivity,
            DateTime date1,
            DateTime date2)
        {
            return IocContainer.Resolve<FullActivityDataReport>(
                new NamedParameter("licensedActivity", licensedActivity),
                new NamedParameter("date1", date1),
                new NamedParameter("date2", date2));
        }

        public ILicenseProvider GetLicenseProvider()
        {
            return IocContainer.Resolve<ILicenseProvider>();
        }

        public virtual SupervisionFacade GetDossierFileSuperviser(DossierFile dossierFile)
        {
            return IocContainer.Resolve<SupervisionFacade>(new NamedParameter("dossierFile", dossierFile));
        }

        public virtual DossierFileBuilder GetDossierFileBuilder()
        {
            return IocContainer.Resolve<DossierFileBuilder>();
        }

        public virtual DossierFileRepository GetDossierFileRegistr()
        {
            return IocContainer.Resolve<DossierFileRepository>();
        }

        public IParser GetTaskParser()
        {
            return IocContainer.Resolve<IParser>();
        }

        public virtual IDictionaryManager GetDictionaryManager()
        {
            return IocContainer.Resolve<IDictionaryManager>();
        }

    }
}