using GU.DataModel;
using GU.DataModel.Inquiry;

namespace GU.Enisey.BL.Converters.Common
{
    public interface IInquiryXmlImporter : IContentXmlImporter
    {
        Inquiry ImportInquiryFromXml(XmlData xmlData, Task task = null);
    }
}
