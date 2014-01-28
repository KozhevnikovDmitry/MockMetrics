using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision.Renewal
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class MedJuridicalRenewalTests : RenewalTests
    {
        protected override int NewLicenseServiceId
        {
            get { return 6; }
        }

        /// <summary>
        /// Реорганизация юридического лица в форме преобразования
        /// </summary>
        [TestCase("Renewal/med/jur/med_reorganization_jur")]
        public void JuridicalReorganizationTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Изменение наименования юридического лица
        /// </summary>
        [TestCase("Renewal/med/jur/med_rename_jur")]
        public void JuridicalRenameTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Изменение адреса места нахождения юридического лица
        /// </summary>
        [TestCase("Renewal/med/jur/med_change_address_jur")]
        public void JuridicalChangeAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Осуществление лицензируемого вида деятельности по адресу, не указанному в лицензии
        /// </summary>
        [TestCase("Renewal/med/jur/med_another_address_jur")]
        public void JuridicalAnotherAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Выполнение работ, оказание услуг, не указанных в лицензии
        /// </summary>
        [TestCase("Renewal/med/jur/med_another_works_jur")]
        public void JuridicalAnotherWorksTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Прекращение лицензируемого вида деятельности по одному или нескольким адресам
        /// </summary>
        [TestCase("Renewal/med/jur/med_stop_activity_address_jur")]
        public void JuridicalStopActivityAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }
        
        /// <summary>
        /// Прекращение выполнения работ, оказания услуг
        /// </summary>
        [TestCase("Renewal/med/jur/med_stop_works_jur")]
        public void JuridicalStopWorksTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }
    }
}