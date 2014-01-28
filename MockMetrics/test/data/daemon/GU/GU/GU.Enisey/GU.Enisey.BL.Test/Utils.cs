using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GU.Enisey.BL.Converters.Common;
using GU.Enisey.DataModel;

using Ionic.Zip;

namespace GU.Enisey.BL.Test
{
    public static class Utils
    {
        public static void RetrieveTaskReceiveXml(string path, DateTime? minDate)
        {
            using (var db = new EniseyDbManager())
            {
                var query = db.GetDomainTable<TaskReceive>();
                if (minDate.HasValue)
                    query = query.Where(t => t.Stamp >= minDate.Value);

                foreach (var taskReceive in query)
                {
                    try
                    {

                        XElement innerXml = null;
                        bool hasAttachements = false;
                        byte[] zip = null;

                        string envData = taskReceive.ReceivedMessage;
                        var xdoc = XDocument.Parse(envData);

                        var xmlContent = xdoc.Descendants().Where(t => t.Name.LocalName == "xmlContent").ToList();
                        if (xmlContent.Count == 1)
                        {
                            innerXml = xmlContent[0];
                        }
                        else
                        {
                            var conts = xdoc.Descendants().Where(x => x.Name.LocalName == "binaryContent").ToList();
                            if (conts.Count == 1)
                            {
                                var cont = conts[0];
                                var attr = cont.Attributes().FirstOrDefault(x => x.Name == "XMLFileName");
                                if (attr == null) continue;
                                string fileName = attr.Value;
                                var bytes = Convert.FromBase64String(cont.Value);

                                var xmlData = XmlDataUtils.LoadFromZip(bytes, fileName);

                                innerXml = xmlData.Xml;
                                hasAttachements = xmlData.Attachements.Count > 0;
                                zip = bytes;
                            }
                        }

                        if (innerXml != null)
                        {
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            var name = innerXml.Name.LocalName;
                            if (innerXml.Elements().Count() == 1)
                            {
                                name = name + "-" + innerXml.Elements().Single().Name.LocalName;
                                if (innerXml.Elements().Elements().Count() == 1)
                                    name = name + "-" + innerXml.Elements().Elements().Single().Name.LocalName;
                            }
                            var filename = string.Format(
                                @"{7}\{0}-{8}_{1:D4}{2:D2}{3:D2}-{4:D2}{5:D2}_{6}",
                                name,
                                taskReceive.Stamp.Year, taskReceive.Stamp.Month, taskReceive.Stamp.Day,
                                taskReceive.Stamp.Hour, taskReceive.Stamp.Minute,
                                taskReceive.Id, path,
                                innerXml.Name.Namespace.NamespaceName.Split(new[] { '/' }).LastOrDefault());

                            if (hasAttachements)
                            {
                                filename = filename + ".zip";
                                File.WriteAllBytes(filename, zip);
                            }
                            else
                            {
                                filename = filename + ".xml";
                                innerXml.Save(filename);
                            }
                            File.SetCreationTime(filename, taskReceive.Stamp);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}
