using System;
using Autofac;
using Common.DA.Interface;
using GU.MZ.BL.Tests.AcceptanceTest;
using Microsoft.Win32;
using NUnit.Framework;

namespace GU.MZ.Import.Tests.Tests
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class ImporterTests : MzAcceptanceTests 
    {
        [Test, RequiresSTA]
        public void ImporterDialogTest()
        {
            // Arrange
            var db = MzLogicFactory.IocContainer.Resolve<Func<IDomainDbManager>>()();
            var dialog = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "Excel files (*.xls)|*.xls",
                    RestoreDirectory = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

            // Act
            if (dialog.ShowDialog() == true)
            {
                MzLogicFactory.IocContainer.Resolve<Importer>().Import(db, dialog.FileName, "log.xml");
            }
        }

        [Test]
        public void ImportTest()
        {
            // Act
            try
            {
                var db = MzLogicFactory.IocContainer.Resolve<Func<IDomainDbManager>>()();
                MzLogicFactory.IocContainer.Resolve<Importer>().Import(db, "med.xls", "log.xml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [Test]
        public void SynchronizeTest()
        {
            // Act
            try
            {
                var db = MzLogicFactory.IocContainer.Resolve<Func<IDomainDbManager>>()();
                MzLogicFactory.IocContainer.Resolve<Importer>().Import(db, "med.xls", "log.xml");
                MzLogicFactory.IocContainer.Resolve<Importer>().Import(db, "med.xls", "log.xml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}