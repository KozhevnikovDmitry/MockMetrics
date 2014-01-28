using System.Collections.Generic;
using System.Xml.Linq;
using GU.DataModel;

namespace GU.Enisey.BL.Converters.Common
{
    public class ConvertationContext
    {
        public XElement XmlRoot { get; private set; }
        public Content Content { get; set; }
        
        public Dictionary<string, byte[]> Attachements { get; set; }
        
        public List<XElement> VisitedElements { get; private set; }
        public List<XAttribute> VisitedAttributes { get; private set; }

        public ConvertationContext(XElement xmlRoot)
        {
            XmlRoot = xmlRoot;
            VisitedElements = new List<XElement>();
            VisitedAttributes = new List<XAttribute>();
            Attachements = new Dictionary<string, byte[]>();
        }

        public void Visit(params XElement[] elements)
        {
            if (elements != null)
                VisitedElements.AddRange(elements);
        }

        public void Visit(params XAttribute[] attributes)
        {
            if (attributes != null)
                VisitedAttributes.AddRange(attributes);
        }
    }
}
