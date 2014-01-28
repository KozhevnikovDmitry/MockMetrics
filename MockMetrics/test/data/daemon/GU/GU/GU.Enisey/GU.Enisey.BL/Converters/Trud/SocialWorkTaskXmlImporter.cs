using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.Trud
{
    public class SocialWorkTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/SocialWork/1.4";

        private static readonly List<ContentXsdMatch> SocialWorkMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "trud/services/socialWork", RootName = Namespace + "SocialWork"}
            };

        public SocialWorkTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(SocialWorkMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            var person = xml.Elements().Single(t => t.Name.LocalName == "Person");
            var name1 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
            var name2 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
            var name3 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
            var phone = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");

            task.CustomerFio = string.Format("{0} {1} {2}", name1 != null ? name1.Value : "", name2 != null ? name2.Value : "", name3 != null ? name3.Value : "").Trim();
            if (phone != null) task.CustomerPhone = phone.Value;
        }

    }
}
