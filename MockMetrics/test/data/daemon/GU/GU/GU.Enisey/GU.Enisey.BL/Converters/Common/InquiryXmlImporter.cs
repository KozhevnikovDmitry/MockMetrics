using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.DataModel.Inquiry;
using GU.Enisey.BL.Converters.Common.Exceptions;

namespace GU.Enisey.BL.Converters.Common
{
    public abstract class InquiryXmlImporter : ContentXmlImporter, IInquiryXmlImporter
    {
        protected readonly IInquiryPolicy InquiryPolicy;

        protected InquiryXmlImporter(List<ContentXsdMatch> matchList, IContentPolicy contentPolicy, IDictionaryManager dictionaryManager, IInquiryPolicy inquiryPolicy)
            : base(matchList, contentPolicy, dictionaryManager)
        {
            InquiryPolicy = inquiryPolicy;
        }

        public Inquiry ImportInquiryFromXml(XmlData xmlData, Task task = null)
        {
            var convertationContext = ImportFromXmlContext(xmlData);
            var content = convertationContext.Content;

            // определяем вид запроса по спеке. не уверен что это хорошо, но так проще
            // теоретически могут быть запросы с одинаковыми спеками
            var inquiryTypes = DictionaryManager.GetDictionary<InquiryType>().Where(t => t.RequestSpecId == content.Spec.Id).ToList();
            if (inquiryTypes.Count == 0)
                throw new ConverterException(string.Format("Не найдена запрос по спецификации запроса (request_spec_id={0})", content.SpecId));
            if (inquiryTypes.Count > 1)
                throw new ConverterException(string.Format("Найдено несколько запросов с одной спецификацией запроса (request_spec_id={0})", content.SpecId));
            var inquiryType = inquiryTypes.Single();

            var inquiry = InquiryPolicy.CreateEmptyInquiry(inquiryType, task);
            inquiry.RequestContent = content;

            //наверно лучше обрабатывать уже разобранный контент, а не исходную xml
            //ConvertTaskInfo(convertationContext.XmlRoot, task);

            //TaskPolicy.SetStatus(TaskStatusType.CheckupWaiting, string.Empty, task);

            return inquiry;
        }

    }
}
