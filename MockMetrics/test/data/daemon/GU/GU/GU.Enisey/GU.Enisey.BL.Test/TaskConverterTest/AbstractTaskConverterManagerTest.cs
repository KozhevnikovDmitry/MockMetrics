using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Common.BL.DataMapping;
using GU.BL;
using GU.DataModel;
using GU.Enisey.BL.Converters;
using GU.Enisey.BL.Converters.Common;
using Ionic.Zip;
using NUnit.Framework;

namespace GU.Enisey.BL.Test.TaskConverterTest
{
    [TestFixture]
    public abstract class AbstractTaskConverterManagerTest : IntegrationFixture
    {
        private readonly bool _isSave;

        protected AbstractTaskConverterManagerTest(bool isSave)
        {
            _isSave = isSave;
        }

        [TestCase(@"LicensingDrugActivity-Copy-1.5_20130528-1238_190.xml")]
        public void ConvertMzLicLicensingDrugActivity(string filename)
        {
            ConvertTask(@"TestXml\mz\lic\LicensingDrugActivity", filename);
        }

        public void ConvertMzLicLicensingFarmActivity(string filename)
        {
            ConvertTask(@"TestXml\mz\lic\LicensingFarmActivity", filename);
        }

        
        [TestCase(@"LicensingMedActivity-CopyLicense-1.8_20130529-0831_204.zip")]
        [TestCase(@"LicensingMedActivity-CopyLicense-CopyLicenseUL-1.8_20130529-0826_203.xml")]
        [TestCase(@"LicensingMedActivity-DublicatLicense-DublicatLicenseIP-1.8_20130529-0822_202.xml")]
        [TestCase(@"LicensingMedActivity-DublicatLicense-DublicatLicenseUL-1.8_20130529-0813_201.xml")]
        [TestCase(@"LicensingMedActivity-NewLicense-NewLicenseIP-1.8_20130528-1438_192.xml")]
        [TestCase(@"LicensingMedActivity-NewLicense-NewLicenseUL-1.8_20130528-1417_191.xml")]
        [TestCase(@"LicensingMedActivity-ReregistrationLicense-ReregistrationLicenseIP-1.8_20130528-1454_193.xml")]
        [TestCase(@"LicensingMedActivity-ReregistrationLicense-ReregistrationLicenseUL-1.8_20130528-1638_194.xml")]
        [TestCase(@"LicensingMedActivity-ReregistrationLicense-ReregistrationLicenseUL-1.8_20130530-0828_207.xml")]
        [TestCase(@"LicensingMedActivity-StopLicense-1.8_20130529-0840_206.zip")]
        [TestCase(@"LicensingMedActivity-StopLicense-StopLicenseUL-1.8_20130529-0835_205.xml")]
        public void ConvertMzLicLicensingMedActivity(string filename)
        {
            ConvertTask(@"TestXml\mz\lic\LicensingMedActivity", filename);
        }

        [TestCase(@"ManagersAttestation-1.2_20130507-1645_169.xml")]
        public void ConvertMzCertManagersAttestation(string filename)
        {
            ConvertTask(@"TestXml\mz\cert\ManagersAttestation", filename);
        }

        [TestCase(@"TeachersAttestation-1.3_20130507-1638_168.xml")]
        public void ConvertMzCertTeachersAttestation(string filename)
        {
            ConvertTask(@"TestXml\mz\cert\TeachersAttestation", filename);
        }

        [TestCase(@"PharmaceutistAttestation-1.3_20130429-1100_156.xml")]
        public void ConvertMzCertPharmaceutistAttestation(string filename)
        {
            ConvertTask(@"TestXml\mz\cert\PharmaceutistAttestation", filename);
        }

        [TestCase(@"MedicalAttestation-1.3_20130429-1129_160.xml")]
        public void ConvertMzCertMedicalAttestation(string filename)
        {
            ConvertTask(@"TestXml\mz\cert\MedicalAttestation", filename);
        }

        [TestCase(@"MedicalADC-1.4_20130429-1134_161.xml")]
        public void ConvertMzCertMedicalADC(string filename)
        {
            ConvertTask(@"TestXml\mz\cert\MedicalADC", filename);
        }

        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpFirstTime-1.5_20130406-1030_100.xml")]
        [TestCase(@"SearchWorkEmployees-SearchWork-FirstTime-1.5_20130406-1024_99.xml")]
        [TestCase(@"SearchWorkEmployees-SearchWork-FirstTime-1.5_20130408-1056_107.xml")]
        [TestCase(@"SearchWorkEmployees-SearchWork-ToRe-1.5_20130408-1102_108.xml")]
        [TestCase(@"SearchWorkEmployees-SearchWork-ActiveEmployment-1.5_20130408-1136_109.xml")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpFirstTime-1.5_20130408-1434_110.xml")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpFirstTime-1.5_20130408-1455_111.xml")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpToRe-1.5_20130408-1531_112.xml")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpActiveEmployment-1.5_20130408-1555_113.zip")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpToRe-1.5_20130408-1606_114.xml")]
        [TestCase(@"SearchWorkEmployees-SearchEmployees-EmpActiveEmployment-1.5_20130408-1611_115.xml")]
        public void ConvertTrudSearchWorkEmployees(string filename)
        {
            ConvertTask(@"TestXml\trud\SearchWorkEmployees", filename);
        }

        [TestCase(@"SocialWork-1.3_20130404-1622_98.xml")]
        [TestCase(@"SocialWork-1.3_20130406-1800_105.zip")]
        [TestCase(@"SocialWork-1.3_20130408-1004_106.xml")]
        [TestCase(@"SocialWork-1.3_20130408-1642_119.xml")]
        public void ConvertTrudSocialWork(string filename)
        {
            ConvertTask(@"TestXml\trud\SocialWork", filename);
        }

        [TestCase(@"TemporaryWork-1.3_20130406-1258_104.xml")]
        [TestCase(@"TemporaryWork-1.3_20130408-1633_117.xml")]
        [TestCase(@"TemporaryWork-1.3_20130408-1635_118.xml")]
        public void ConvertTrudTemporaryWork(string filename)
        {
            ConvertTask(@"TestXml\trud\TemporaryWork", filename);
        }

        [TestCase(@"UnemployedPayment-1.4_20130406-1240_103.xml")]
        public void ConvertTrudUnemployedPaymentWork(string filename)
        {
            ConvertTask(@"TestXml\trud\UnemployedPayment", filename);
        }

        [TestCase(@"ConstructingPermition-1.2_20130409-1707_120.xml")]
        [TestCase(@"ConstructingPermition-1.2_20130409-1709_121.xml")]
        [TestCase(@"ConstructingPermition-1.2_20130410-1011_124.xml")]
        public void ConvertBuildingConstructingPermition(string filename)
        {
            ConvertTask(@"TestXml\building\ConstructingPermition", filename);
        }

        [TestCase(@"Commissioning-1.4_20130507-1701_170.xml")]
        public void ConvertBuildingCommissioning(string filename)
        {
            ConvertTask(@"TestXml\building\Commissioning", filename);
        }

        [TestCase(@"RegistrationHousingNeeded-1.4_20130419-0953_147.xml")]
        public void ConvertRegistrationHousingNeeded(string filename)
        {
            ConvertTask(@"TestXml\mun\RegistrationHousingNeeded", filename);
        }

        [TestCase(@"InformationProvision-1.5.2_20131106-1259_354.xml")]
        [TestCase(@"InformationProvision-1.5.2_20131107-0638_356.xml")]
        [TestCase(@"InformationProvision-1.5.2_20131107-0642_357.xml")]
        public void ConvertInformationProvision(string filename)
        {
            ConvertTask(@"TestXml\archive\InformationProvision", filename);
        }

        private void ConvertTask(string dir, string filename)
        {
            // Arrange
            var dm = GuFacade.GetDataMapper<Task>();
            var taskConverterManager = EniseyFacade.GetConverterManager();

            // Act
            var task = ConvertTask(Path.Combine("TaskConverterTest", dir, filename), _isSave, dm, taskConverterManager);

            // Assert
            Assert.NotNull(task.Content);
            if (_isSave)
                Assert.AreNotEqual(task.Id, 0);
        }

        private Task ConvertTask(string filename, bool save, IDomainDataMapper<Task> taskDm, ConverterManager converterManager)
        {
            var xmlData = new XmlData();

            if (filename.EndsWith(".xml"))
            {
                using (var reader = new XmlTextReader(filename))
                {
                    reader.MoveToContent();
                    var xml = XNode.ReadFrom(reader) as XElement;
                    if (xml == null)
                        throw new Exception();
                    xmlData.Xml = xml;
                }
            }
            else if (filename.EndsWith(".zip"))
            {
                string xmlFileName = null;
                using (var zip = ZipFile.Read(filename))
                {
                    xmlFileName = zip.EntryFileNames.Single(t => t.EndsWith(".xml"));
                }
                xmlData = XmlDataUtils.LoadFromZip(File.ReadAllBytes(filename), xmlFileName);
            }
            else throw new Exception();

            var task = converterManager.ImportTaskFromXml(xmlData);
            if (save)
                task = taskDm.Save(task);

            return task;
        }
    }
}
