using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.Trud
{
    public class SearchWorkEmployeesTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/SearchWorkEmployees/1.6";

        private static readonly XName RootName = Namespace + "SearchWorkEmployees";
        private static readonly XName SubRootWorkName = Namespace + "SearchWork";
        private static readonly XName SubRootEmployeesName = Namespace + "SearchEmployees";

        private static readonly List<ContentXsdMatch> SearchWorkEmployeesMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch {ContentUri = "trud/services/searchWork/firstTime", RootName = RootName, SubRootName = SubRootWorkName, SubSubRootName = Namespace + "FirstTime"},
                new ContentXsdMatch {ContentUri = "trud/services/searchWork/toRe", RootName = RootName, SubRootName = SubRootWorkName, SubSubRootName = Namespace + "ToRe"},
                new ContentXsdMatch {ContentUri = "trud/services/searchWork/activeEmployment", RootName = RootName, SubRootName = SubRootWorkName, SubSubRootName = Namespace + "ActiveEmployment"},
                new ContentXsdMatch {ContentUri = "trud/services/searchEmployees/firstTime", RootName = RootName, SubRootName = SubRootEmployeesName, SubSubRootName = Namespace + "EmpFirstTime"},
                new ContentXsdMatch {ContentUri = "trud/services/searchEmployees/toRe", RootName = RootName, SubRootName = SubRootEmployeesName, SubSubRootName = Namespace + "EmpToRe"},
                new ContentXsdMatch {ContentUri = "trud/services/searchEmployees/activeEmployment", RootName = RootName, SubRootName = SubRootEmployeesName, SubSubRootName = Namespace + "EmpActiveEmployment"}
            };

        public SearchWorkEmployeesTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy, IDictionaryManager dictionaryManager)
            : base(SearchWorkEmployeesMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            if (task.Service.Id == 45 || task.Service.Id == 63 || task.Service.Id == 64)
            {
                var person = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "Person");
                if (person != null)
                {
                    var name1 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                    var name2 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                    var name3 = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                    task.CustomerFio = string.Format("{0} {1} {2}", name1 != null ? name1.Value : "", name2 != null ? name2.Value : "", name3 != null ? name3.Value : "").Trim();

                    var phone = person.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
                    if (phone != null) task.CustomerPhone = phone.Value;
                }
            }
            else if (task.Service.Id == 48 || task.Service.Id == 65 || task.Service.Id == 66)
            {
                var hirer = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "Hirer");
                if (hirer != null)
                {
                    XElement name1 = null, name2 = null, name3 = null;

                    var ulOrg = hirer.Elements().SingleOrDefault(t => t.Name.LocalName == "UL");
                    if (ulOrg != null)
                    {
                        name1 = ulOrg.Elements().SingleOrDefault(t => t.Name.LocalName == "ULName");
                    }

                    var personOrg = hirer.Elements().SingleOrDefault(t => t.Name.LocalName == "IP" || t.Name.LocalName == "FL");
                    if (personOrg != null)
                    {
                        name1 = personOrg.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                        name2 = personOrg.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                        name3 = personOrg.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                    }

                    task.CustomerFio =
                        string.Format("{0} {1} {2}", name1 != null ? name1.Value : "",
                                      name2 != null ? name2.Value : "",
                                      name3 != null ? name3.Value : "").Trim();
                    
                    var phone = hirer.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
                    if (phone != null) task.CustomerPhone = phone.Value;
                    var email = hirer.Elements().SingleOrDefault(t => t.Name.LocalName == "e-mail");
                    if (email != null) task.CustomerEmail = email.Value;
                }
            }
        }
    }
}
