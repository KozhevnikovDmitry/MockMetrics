using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.MZ.Licensing
{
    public class LicensingFarmActivityTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/LicensingFarmActivity/1.8";

        private static readonly XName FarmRoot = Namespace + "LicensingFarmActivity";

        private static readonly List<ContentXsdMatch> FarmMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "mz/lic/services/farm/new", RootName = FarmRoot, SubRootName = Namespace + "NewLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/farm/reregistration", RootName = FarmRoot, SubRootName = Namespace + "ReregistrationLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/farm/stop", RootName = FarmRoot, SubRootName = Namespace + "StopLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/farm/dublicat", RootName = FarmRoot, SubRootName = Namespace + "DublicatLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/farm/copy", RootName = FarmRoot, SubRootName = Namespace + "CopyLicense"}
            };

        public LicensingFarmActivityTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(FarmMatches, contentPolicy, dictionaryManager, taskPolicy)
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

            var ip = xml.Elements().SingleOrDefault(t => t.Name.LocalName == xml.Name.LocalName+"IP");
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
