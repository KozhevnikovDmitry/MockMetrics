using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.Building
{
    public class CommissioningTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/Commissioning/1.5";

        private static readonly List<ContentXsdMatch> ConstructingPermitionMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch
                    {
                        ContentUri = "building/services/commissioning",
                        RootName = Namespace + "Commissioning"
                    }
            };

        public CommissioningTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy,
                                                      IDictionaryManager dictionaryManager)
            : base(ConstructingPermitionMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            XElement name1 = null, name2 = null, name3 = null;

            var agent = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "RepresentativeInfo");

            if (agent == null)
            {
                var personApplicant =
                    xml.Elements()
                       .SingleOrDefault(t => t.Name.LocalName == "Applicant" || t.Name.LocalName == "ApplicantInfo");
                if (personApplicant != null)
                {
                    name1 = personApplicant.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                    name2 = personApplicant.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                    name3 = personApplicant.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                }

                var jurApplicant = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "LegalPerson");
                if (jurApplicant != null)
                {
                    name1 = jurApplicant.Elements().SingleOrDefault(t => t.Name.LocalName == "FullName");
                }
            }
            else
            {
                name1 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                name2 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                name3 = agent.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
            }

            task.CustomerFio =
                string.Format("{0} {1} {2}",
                              name1 != null ? name1.Value : "",
                              name2 != null ? name2.Value : "",
                              name3 != null ? name3.Value : "")
                      .Trim();
        }

        protected override List<XElement> GetElementsForSpecNode(List<XElement> elements, SpecNode specNode)
        {
            var nodeElements = base.GetElementsForSpecNode(elements, specNode);
            if (specNode.Tag == "Applicant")
            {
                var specialNodeElements = elements.Where(t => t.Name.LocalName == "ApplicantInfo").ToList();
                nodeElements.AddRange(specialNodeElements);
            }
            return nodeElements;
        }
    }
}
