using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Common.BL;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using GU.Trud.BL.Export.Interface;
using GU.Trud.BL.Mailing;
using GU.Trud.BL.Mailing.Interface;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Контейнер служб экспорта данных.
    /// </summary>
    internal class TrudLogicContainer : DomainLogicContainer
    {

        /// <summary>
        /// Контейнер служб экспорта данных.
        /// </summary>
        public TrudLogicContainer(IEnumerable<Assembly> assemblies, IDictionaryManager dictionaryManager)
            : base(assemblies, dictionaryManager)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GenerateExportService>().As<IGenerateExportService>();
            builder.RegisterType<SendExportService>().As<ISendExportService>();
            builder.RegisterType<SaveExportService>().As<ISaveExportService>();
            builder.RegisterType<DbaseToFileDumper>().As<IToFileDumper>();
            builder.RegisterType<ZipCatalogArchivator>().As<ICatalogArchivator>();
            builder.RegisterType<MailClientInvoker>().As<IMailClientInvoker>();
            builder.Update(IocContainer);
        }

        /// <summary>
        /// Вовзращает службу осуществления выгрузок.
        /// </summary>
        /// <returns>Служба осуществления выгрузок</returns>
        public IGenerateExportService ResolveExportService(string operatingCatalog)
        {
            try
            {
                return IocContainer.Resolve<IGenerateExportService>(new NamedParameter("operatingCatalog", operatingCatalog));
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException("Ошибка при создании экземпляра IExportService", ex);
            }
        }

        /// <summary>
        /// Возвращает службу отправки выгрузок.
        /// </summary>
        /// <param name="operatingCatalog"> </param>
        /// <returns>Служба отправки выгрузок</returns>
        public ISendExportService ResolveSendExportService(string operatingCatalog)
        {
            try
            {
                return IocContainer.Resolve<ISendExportService>(new NamedParameter("operatingCatalog", operatingCatalog));
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException("Ошибка при создании экземпляра ISendExportService", ex);
            }
        }

        /// <summary>
        /// Возвращает службу сохранения выгрузок.
        /// </summary>
        /// <returns>Служба сохранения выгрузок</returns>
        public ISaveExportService ResolveSaveExportService()
        {
            try
            {
                return IocContainer.Resolve<ISaveExportService>();
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException("Ошибка при создании экземпляра ISaveExportService", ex);
            }
        }
    }
}
