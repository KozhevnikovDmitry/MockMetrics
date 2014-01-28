using System;
using System.Configuration;
using System.Windows;
using Autofac;
using Common.DA;
using Common.DA.Interface;
using Common.DA.ProviderConfiguration;
using GU.MZ.BL;
using GU.MZ.BL.DomainLogic.Authentification;
using GU.MZ.BL.Modules;
using Microsoft.Win32;

namespace GU.MZ.Import
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var context = Initialize();

                var dialog = new OpenFileDialog
                {
                    Filter = "Excel files (*.xls)|*.xls",
                    RestoreDirectory = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static IContainer Initialize()
        {
            var confFactory = new ProviderConfigurationFactory();
            var configuration = confFactory.GetConfiguration(ConfigurationManager.AppSettings["ConnectionConfig"]);
            new MzAutentificator(configuration, new DataAccessLayerInitializer()).AuthentificateUser("mz_lic_tokarev", "tokarev");

            var builder = new ContainerBuilder();
            builder.RegisterModule(new MzBlModule(Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"])));
            builder.RegisterModule<ImportModule>();
            var result = builder.Build();

            builder = new ContainerBuilder();
            builder.RegisterInstance(result).As<IContainer>();
            builder.Update(result);

            return result;
        }
    }
}
