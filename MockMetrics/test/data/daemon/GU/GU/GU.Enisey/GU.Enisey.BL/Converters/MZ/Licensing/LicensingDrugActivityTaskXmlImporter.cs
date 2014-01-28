using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.MZ.Licensing
{
    public class LicensingDrugActivityTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/LicensingDrugActivity/1.5";

        private static readonly XName DrugRoot = Namespace + "LicensingDrugActivity";

        private static readonly List<ContentXsdMatch> DrugMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "mz/lic/services/drug/new", RootName = DrugRoot, SubRootName = Namespace + "NewLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/drug/reregistration", RootName = DrugRoot, SubRootName = Namespace + "Reregistration"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/drug/stop", RootName = DrugRoot, SubRootName = Namespace + "StopLicense"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/drug/dublicat", RootName = DrugRoot, SubRootName = Namespace + "Dublicat"},
                new ContentXsdMatch {ContentUri = "mz/lic/services/drug/copy", RootName = DrugRoot, SubRootName = Namespace + "Copy"}
            };

        public LicensingDrugActivityTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(DrugMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, GU.DataModel.Task task)
        {
            var orgInfo = xml.Elements().Single(t => t.Name.LocalName == "OrgInfo");
            var fullName = orgInfo.Elements().SingleOrDefault(t => t.Name.LocalName == "FullName");
            var phone = orgInfo.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
            var email = orgInfo.Elements().SingleOrDefault(t => t.Name.LocalName == "Email");

            if (fullName != null) task.CustomerFio = fullName.Value;
            if (phone != null) task.CustomerPhone = phone.Value;
            if (email != null) task.CustomerEmail = email.Value;
        }
    }
}
