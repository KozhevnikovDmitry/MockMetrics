using System.Xml.Linq;
using GU.DataModel;

namespace GU.Enisey.BL.Converters.Common
{
    public interface IContentXmlExporter
    {
        string[] SupportedContentUris { get; }
        
        XElement ExportToXml(Content content);
    }
}
