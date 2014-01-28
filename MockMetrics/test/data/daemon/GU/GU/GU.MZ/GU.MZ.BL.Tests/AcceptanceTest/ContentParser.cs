using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GU.DataModel;
using GU.Enisey.BL.Converters;
using GU.Enisey.BL.Converters.Common;

namespace GU.MZ.BL.Tests.AcceptanceTest
{
    /// <summary>
    /// Парсер для получения заявок из xml файлов полученных из GU.Enisey
    /// Только для тестов
    /// </summary>
    public class ContentParser
    {
        private readonly ConverterManager _converterManager;

        public ContentParser(ConverterManager converterManager)
        {
            _converterManager = converterManager;
        }

        public Task ParseTask(int serviceId)
        {
            return ParseTask(serviceId, RandomProvider.RandomNumberString(11), RandomProvider.RandomNumberString(13));
        }

        public Task ParseTask(string contentPath, string inn, string ogrn)
        {
            string current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "AcceptanceTest", "Content", contentPath);
            var filename = path + ".xml";
            return ParseXmlToTask(filename, inn, ogrn);
        }
        
        public Task ParseTask(int serviceId, string inn, string ogrn)
        {
            string current = Directory.GetCurrentDirectory();
            var path = Path.Combine(current, "AcceptanceTest", "Content");
            var files = Directory.GetFiles(path);
            var filename = files.SingleOrDefault(t => Path.GetFileName(t).StartsWith(serviceId.ToString().PadLeft(2, '0')));
            return ParseXmlToTask(filename, inn, ogrn);
        }

        private Task ParseXmlToTask(string filePath, string inn, string ogrn)
        {
            var xmlData = new XmlData();
            using (var reader = new XmlTextReader(filePath))
            {
                reader.MoveToContent();
                var xml = XNode.ReadFrom(reader) as XElement;
                SetupInnAndOgrn(xml, inn, ogrn);
                if (xml == null)
                    throw new Exception();
                xmlData.Xml = xml;
            }

            return _converterManager.ImportTaskFromXml(xmlData);
        }

        private void SetupInnAndOgrn(XElement xml, string inn, string ogrn)
        {
            foreach (var innNode in xml.Descendants().Where(t => t.Name.LocalName == "INN" && t.Value == "1005001005"))
            {
                innNode.SetValue(inn);
            }

            foreach (var ogrnNode in xml.Descendants().Where(t => (t.Name.LocalName == "OGRNnumber" || t.Name.LocalName == "OGRN") && t.Value == "1005001005001"))
            
            {
                ogrnNode.SetValue(ogrn);
            }
        }
    }
}
