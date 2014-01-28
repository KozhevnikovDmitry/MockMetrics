using System.IO;
using System.Xml.Serialization;
using GU.Enisey.Contract.ApplicationInquiryNotifierPortType12;
using NUnit.Framework;

namespace GU.Enisey.BL.Test.LogicTest
{
    public class AppInquiryNotifierLogicTest : IntegrationFixture
    {
        [TestCase("notifyApplicationSubmittedRequest-1.2.xml")]
        public void NotifyApplicationSubmittedTest(string requestXmlPath)
        {
            // Arrange
            var stream = new StreamReader(Path.Combine(@"LogicTest\TestXml", requestXmlPath), true);
            var xser = new XmlSerializer(typeof(notifyApplicationSubmittedRequest));
            var request = (notifyApplicationSubmittedRequest)xser.Deserialize(stream);

            // Act
            var response = AppInquiryNotifierLogic.notifyApplicationSubmitted(request);

            // Assert
            Assert.IsInstanceOf<tagType>(response.Item);
        }
    }
}
