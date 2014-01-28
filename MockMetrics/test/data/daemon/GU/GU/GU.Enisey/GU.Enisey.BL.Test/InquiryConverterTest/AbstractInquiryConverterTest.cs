using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Common.BL.DataMapping;
using GU.BL;
using GU.DataModel;
using GU.DataModel.Inquiry;
using GU.Enisey.BL.Converters;
using GU.Enisey.BL.Converters.Common;
using Ionic.Zip;
using NUnit.Framework;
using Autofac;

namespace GU.Enisey.BL.Test.InquiryConverterTest
{
    [TestFixture]
    public abstract class AbstractInquiryConverterTest : IntegrationFixture
    {
        private readonly bool _isSave;

        protected AbstractInquiryConverterTest(bool isSave)
        {
            _isSave = isSave;
        }

        [TestCase(@"test.xml")]
        public void ConvertZagsTest(string filename)
        {
            ConvertInquiry(@"TestXml\zags-test", filename);
        }

        private void ConvertInquiry(string dir, string filename)
        {
            // Arrange
            var dm = GuFacade.GetDataMapper<Inquiry>();
            var converterManager = EniseyFacade.GetConverterManager();

            // Act
            var inquiry = ConvertInquiry(Path.Combine("InquiryConverterTest", dir, filename), _isSave, dm, converterManager);

            // Assert
            Assert.NotNull(inquiry.RequestContent);
            if (_isSave)
                Assert.AreNotEqual(inquiry.Id, 0);
        }

        private Inquiry ConvertInquiry(string filename, bool save, IDomainDataMapper<Inquiry> inquiryDm, ConverterManager converterManager)
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

            var inquiry = converterManager.ImportInquiryFromXml(xmlData);
            if (save)
                inquiry = inquiryDm.Save(inquiry);

            return inquiry;
        }
    }
}
