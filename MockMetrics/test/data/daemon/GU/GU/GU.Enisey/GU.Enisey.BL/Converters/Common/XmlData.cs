using System.Collections.Generic;
using System.Xml.Linq;

namespace GU.Enisey.BL.Converters.Common
{
    public class XmlData
    {
        public XElement Xml { get; set; }
        public Dictionary<string, byte[]> Attachements { get; set; }

        public XmlData()
        {
            Attachements = new Dictionary<string, byte[]>();
        }

    }
}
