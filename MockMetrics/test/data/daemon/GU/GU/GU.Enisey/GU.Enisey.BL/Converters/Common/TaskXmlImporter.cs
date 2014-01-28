using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common.Exceptions;

namespace GU.Enisey.BL.Converters.Common
{
    public abstract class TaskXmlImporter : ContentXmlImporter, ITaskXmlImporter
    {
        protected readonly ITaskPolicy TaskPolicy;

        protected TaskXmlImporter(List<ContentXsdMatch> matchList, IContentPolicy contentPolicy, IDictionaryManager dictionaryManager, ITaskPolicy taskPolicy) 
            : base(matchList, contentPolicy, dictionaryManager)
        {
            TaskPolicy = taskPolicy;
        }

        public Task ImportTaskFromXml(XmlData xmlData)
        {
            var convertationContext = ImportFromXmlContext(xmlData);
            var content = convertationContext.Content;

            // определяем услугу по спеке. не уверен что это хорошо, но так проще
            // теоретически могут быть услуги с одинаковыми спеками
            var services = DictionaryManager.GetDictionary<Service>().Where(t => t.SpecId == content.Spec.Id).ToList();
            if (services.Count == 0)
                throw new ConverterException(string.Format("Не найдена услуга по спецификации (spec_id={0})", content.SpecId));
            if (services.Count > 1)
                throw new ConverterException(string.Format("Найдено несколько услуг с одной спецификацией (spec_id={0})", content.SpecId));
            var service = services.Single();
            
            var task = TaskPolicy.CreateEmptyTask(service, null);
            task.Content = content;

            //наверно лучше обрабатывать уже разобранный контент, а не исходную xml
            ConvertTaskInfo(convertationContext.XmlRoot, task);

            TaskPolicy.SetStatus(TaskStatusType.CheckupWaiting, string.Empty, task);

            return task;
        }

        protected abstract void ConvertTaskInfo(XElement xml, Task task);

    }
}
