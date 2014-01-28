using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GU.DataModel;
using GU.DataModel.Inquiry;
using GU.Enisey.BL.Converters.Common;
using GU.Enisey.BL.Converters.Common.Exceptions;

namespace GU.Enisey.BL.Converters
{
    public class ConverterManager
    {
        private readonly IEnumerable<ITaskXmlImporter> _taskImporters;
        private readonly IEnumerable<IInquiryXmlImporter> _inquiryImporters;

        public ConverterManager(IEnumerable<ITaskXmlImporter> taskImporters, IEnumerable<IInquiryXmlImporter> inquiryImporters)
        {
            _taskImporters = taskImporters;
            _inquiryImporters = inquiryImporters;
        }

        #region task import/export

        private ITaskXmlImporter ResolveTaskImporter(XName tag)
        {
            var conv = _taskImporters.SingleOrDefault(t => t.SupportedTags.Contains(tag));
            if (conv == null)
                throw new ConverterException(string.Format("Не найден TaskImporter для элемента {0}", tag));
            return conv;
        } 

        public Task ImportTaskFromXml(XElement eniseyXml)
        {
            return ImportTaskFromXml(new XmlData { Xml = eniseyXml });
        }

        public Task ImportTaskFromXml(XmlData xmlData)
        {
            var converter = ResolveTaskImporter(xmlData.Xml.Name);
            return converter.ImportTaskFromXml(xmlData);
        }

        public XElement ExportTaskToXml(Task task)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region inquiry import/export

        private IInquiryXmlImporter ResolveInquiryImporter(XName tag)
        {
            var conv = _inquiryImporters.SingleOrDefault(t => t.SupportedTags.Contains(tag));
            if (conv == null)
                throw new ConverterException(string.Format("Не найден InquiryImporter для элемента {0}", tag));
            return conv;
        }

        public Inquiry ImportInquiryFromXml(XElement eniseyXml, Task task = null)
        {
            return ImportInquiryFromXml(new XmlData { Xml = eniseyXml }, task);
        }

        public Inquiry ImportInquiryFromXml(XmlData xmlData, Task task = null)
        {
            var converter = ResolveInquiryImporter(xmlData.Xml.Name);
            return converter.ImportInquiryFromXml(xmlData, task);
        }

        #endregion
    }
}
