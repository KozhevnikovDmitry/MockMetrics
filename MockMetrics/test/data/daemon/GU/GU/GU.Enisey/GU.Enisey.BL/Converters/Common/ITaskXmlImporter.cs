using GU.DataModel;

namespace GU.Enisey.BL.Converters.Common
{
    public interface ITaskXmlImporter : IContentXmlImporter
    {
        Task ImportTaskFromXml(XmlData xmlData);
    }
}
