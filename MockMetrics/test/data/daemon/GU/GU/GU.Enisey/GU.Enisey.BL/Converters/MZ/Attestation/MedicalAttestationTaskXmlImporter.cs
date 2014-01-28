using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.MZ.Attestation
{
    public class MedicalAttestationTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/MedicalAttestation/1.3";

        private static readonly List<ContentXsdMatch> MedicalAttestationMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "mz/cert/medicalAttestation", RootName = Namespace + "MedicalAttestation"}
            };

        public MedicalAttestationTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(MedicalAttestationMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            var person = xml.Elements().Single(t => t.Name.LocalName == "Applicant");
            var name1 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
            var name2 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
            var name3 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
            var phone = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
            var email = person.Elements().SingleOrDefault(t => t.Name.LocalName == "E-mail");

            task.CustomerFio = string.Format("{0} {1} {2}", name1 != null ? name1.Value : "", name2 != null ? name2.Value : "", name3 != null ? name3.Value : "").Trim();
            task.CustomerPhone = phone != null ? phone.Value : null;
            task.CustomerEmail = email != null ? email.Value : null;
        }

    }
}
