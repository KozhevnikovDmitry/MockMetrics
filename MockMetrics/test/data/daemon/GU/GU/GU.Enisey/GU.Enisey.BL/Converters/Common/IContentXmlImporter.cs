using System.Xml.Linq;
using GU.DataModel;

namespace GU.Enisey.BL.Converters.Common
{
    public interface IContentXmlImporter
    {
        XName[] SupportedTags { get; }

        Content ImportFromXml(XmlData xmlData);
    }
}
