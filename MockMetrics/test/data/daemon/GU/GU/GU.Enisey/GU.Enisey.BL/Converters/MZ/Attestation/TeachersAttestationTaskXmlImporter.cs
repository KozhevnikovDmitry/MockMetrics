using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.MZ.Attestation
{
    public class TeachersAttestationTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/TeachersAttestation/1.4";

        private static readonly List<ContentXsdMatch> TeachersAttestationMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "mz/cert/teachersAttestation", RootName = Namespace + "TeachersAttestation"}
            };

        public TeachersAttestationTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(TeachersAttestationMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            var person = xml.Elements().Single(t => t.Name.LocalName == "ApplicantInfo");
            var name1 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
            var name2 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
            var name3 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");

            task.CustomerFio = string.Format("{0} {1} {2}", name1 != null ? name1.Value : "", name2 != null ? name2.Value : "", name3 != null ? name3.Value : "").Trim();
        }

    }
}
