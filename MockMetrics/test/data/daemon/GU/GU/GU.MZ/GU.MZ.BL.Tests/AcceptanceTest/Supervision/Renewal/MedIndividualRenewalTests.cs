using NUnit.Framework;

namespace GU.MZ.BL.Tests.AcceptanceTest.Supervision.Renewal
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class MedIndividualRenewalTests : RenewalTests
    {
        protected override int NewLicenseServiceId
        {
            get { return 6; }
        }

        /// <summary>
        /// Изменение имени, фамилии и (в случае, если имеется) отчества индивидуального предпринимателя
        /// </summary>
        [TestCase("Renewal/med/ind/med_rename_ind")]
        public void IndividualRenameTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Изменение места жительства индивидуального предпринимателя
        /// </summary>
        [TestCase("Renewal/med/ind/med_change_address_ind")]
        public void IndividualChangeAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Изменение реквизитов документа, удостоверяющего личность индивидуального предпринимателя
        /// </summary>
        [TestCase("Renewal/med/ind/med_change_document_ind")]
        public void IndividualReorganizationTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Осуществление лицензируемого вида деятельности по адресу, не указанному в лицензии
        /// </summary>
        [TestCase("Renewal/med/ind/med_another_address_ind")]
        public void IndividualAnotherAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Выполнение работ, оказание услуг, не указанных в лицензии
        /// </summary>
        [TestCase("Renewal/med/ind/med_another_works_ind")]
        public void IndividualAnotherWorksTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Прекращение лицензируемого вида деятельности по одному или нескольким адресам
        /// </summary>
        [TestCase("Renewal/med/ind/med_stop_activity_address_ind")]
        public void IndividualStopActivityAddressTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }

        /// <summary>
        /// Прекращение выполнения работ, оказания услуг
        /// </summary>
        [TestCase("Renewal/med/ind/med_stop_works_ind")]
        public void IndividualStopWorksTest(string xmlContentName)
        {
            // Act
            var dossierFile = Reneawal(NewLicenseServiceId, xmlContentName);

            // Assert
            Assert.NotNull(dossierFile.License);
        }
    }
}