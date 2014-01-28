using System.IO;
using System.Xml.Linq;
using Ionic.Zip;

namespace GU.Enisey.BL.Converters.Common
{
    public static class XmlDataUtils
    {
        public static XmlData LoadFromZip(byte[] zip, string xmlFileName)
        {
            using (var zipStream = new MemoryStream(zip))
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                var xmlData = new XmlData();

                // получение xml-файла с заявлением
                ZipEntry entry = zipFile[xmlFileName];
                using (var ms = new MemoryStream())
                {
                    entry.Extract(ms);
                    ms.Position = 0;
                    xmlData.Xml = XElement.Load(ms);
                }

                // получение вложений
                foreach (var attachementZipEntry in zipFile.Entries)
                {
                    if (attachementZipEntry.FileName != xmlFileName && !attachementZipEntry.FileName.EndsWith(".sig"))
                    {
                        using (var attachmentStream = new MemoryStream())
                        {
                            attachementZipEntry.Extract(attachmentStream);
                            xmlData.Attachements.Add(attachementZipEntry.FileName, attachmentStream.ToArray());
                        }
                    }
                }

                return xmlData;
            }
        }
    }
}
