using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.MZ.Licensing
{
    public class LicensingMedActivityTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/LicensingMedActivity/1.9";

        private static readonly XName MedRoot = Namespace + "LicensingMedActivity";

        private static readonly List<ContentXsdMatch> MedMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "mz/lic/services/med/new", RootName = MedRoot, SubRootName = Namespace + "NewLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/med/reregistration", RootName = MedRoot, SubRootName = Namespace + "ReregistrationLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/med/stop", RootName = MedRoot, SubRootName = Namespace + "StopLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/med/dublicat", RootName = MedRoot, SubRootName = Namespace + "DublicatLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/med/copy", RootName = MedRoot, SubRootName = Namespace + "CopyLicense"}
            };

        public LicensingMedActivityTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(MedMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, GU.DataModel.Task task)
        {
            XElement name1 = null, name2 = null, name3 = null, phone = null, email = null;

            var org = xml.Elements().SingleOrDefault(t => t.Name.LocalName == xml.Name.LocalName + "UL");
            if (org != null)
            {
                var info = org.Elements().Single(t => t.Name.LocalName == "OrgInfo" || t.Name.LocalName == "ShortOrgInfo");
                name1 = info.Elements().SingleOrDefault(t => t.Name.LocalName == "FullName");
                phone = info.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
                email = info.Elements().SingleOrDefault(t => t.Name.LocalName == "Email");
            }

            var ip = xml.Elements().SingleOrDefault(t => t.Name.LocalName == xml.Name.LocalName + "IP");
            if (ip != null)
            {
                var info = ip.Elements().Single(t => t.Name.LocalName == "IPInfo" || t.Name.LocalName == "ShortIPInfo");
                name1 = info.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                name2 = info.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                name3 = info.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                phone = info.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
                email = info.Elements().SingleOrDefault(t => t.Name.LocalName == "Email");
            }

            task.CustomerFio = string.Format("{0} {1} {2}", name1 != null ? name1.Value : "", name2 != null ? name2.Value : "", name3 != null ? name3.Value : "").Trim();
            if (phone != null) task.CustomerPhone = phone.Value;
            if (email != null) task.CustomerEmail = email.Value;
        }
    }
}
