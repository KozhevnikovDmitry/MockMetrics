using System.Xml.Linq;

namespace GU.Enisey.BL.Converters.Common
{
    public class ContentXsdMatch
    {
        public string ContentUri { get; set; }
        public XName RootName { get; set; }
        public XName SubRootName { get; set; }
        public XName SubSubRootName { get; set; }
    }
}
