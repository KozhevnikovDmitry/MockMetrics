using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.Archive
{
    public class InformationProvisionTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/InformationProvision/1.5.2";

        private static readonly List<ContentXsdMatch> ConstructingPermitionMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch
                    {
                        ContentUri = "archive/services/informationProvision",
                        RootName = Namespace + "InformationProvision"
                    }
            };

        public InformationProvisionTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy,
                                                      IDictionaryManager dictionaryManager)
            : base(ConstructingPermitionMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            XElement name1 = null, name2 = null, name3 = null, phone = null, email = null;

            var applicant = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "Applicant");
            var agent = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "Agent");
            var org = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "Organization");

            if (applicant != null)
            {
                name1 = applicant.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                name2 = applicant.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                name3 = applicant.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                email = applicant.Elements().SingleOrDefault(t => t.Name.LocalName == "Email");
                phone = applicant.Elements().FirstOrDefault(t => t.Name.LocalName == "Phone");
            }
            else if (agent != null)
            {
                name1 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "AgentFamilyName");
                name2 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "AgentFirstName");
                name3 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "AgentPatronymic");
                phone = agent.Elements().FirstOrDefault(t => t.Name.LocalName == "AgentPhone");
            }
            else if (org != null)
            {
                name1 = org.Elements().SingleOrDefault(t => t.Name.LocalName == "FullName");
                email = org.Elements().SingleOrDefault(t => t.Name.LocalName == "Email");
                phone = org.Elements().FirstOrDefault(t => t.Name.LocalName == "Phone");
            }

            task.CustomerFio =
                string.Format("{0} {1} {2}",
                              name1 != null ? name1.Value : "",
                              name2 != null ? name2.Value : "",
                              name3 != null ? name3.Value : "")
                      .Trim();
            task.CustomerPhone = phone != null ? phone.Value : "";
            task.CustomerEmail = email != null ? email.Value : "";
        }

        
        protected override List<XElement> GetElementsForSpecNode(List<XElement> elements, SpecNode specNode)
        {
            var nodeElements = base.GetElementsForSpecNode(elements, specNode);
            if (specNode.Tag == "PhysicalPerson")
            {
                if (elements.Any(t => t.Name.LocalName == "Applicant" || t.Name.LocalName == "AgentApplicant")) 
                    nodeElements.Add(elements.First().Parent);
            }
            else if (specNode.Tag == "Applicant")
            {
                var specialNodeElements = elements.Where(t => t.Name.LocalName == "AgentApplicant").ToList();
                nodeElements.AddRange(specialNodeElements);
            }

            return nodeElements;
        }
    }
}
