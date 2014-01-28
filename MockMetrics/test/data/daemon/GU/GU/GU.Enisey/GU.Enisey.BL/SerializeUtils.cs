using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GU.Enisey.BL
{
    public static class SerializeUtils
    {
        public static string GetXmlString(object obj)
        {
            using (var sw = new StringWriter())
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                ser.Serialize(sw, obj);
                string str = sw.ToString();
                sw.Close();

                return str;
            }
        }
    }
}
