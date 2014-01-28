using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Policy.Interface;
using GU.DataModel;
using GU.Enisey.BL.Converters.Common;

namespace GU.Enisey.BL.Converters.Mun
{
    public class RegistrationHouseingNeededTaskXmlImporter : TaskXmlImporter
    {
        private static readonly XNamespace Namespace =
            @"urn://x-artefacts-it-ru/dob/state-services/krsk/RegistrationHousingNeeded/1.7";

        private static readonly List<ContentXsdMatch> RegistrationHousingNeededMatches = new List<ContentXsdMatch>
            {
                new ContentXsdMatch
                    {
                        ContentUri = "mun/services/registrationHousingNeeded",
                        RootName = Namespace + "RegistrationHousingNeeded"
                    }
            };

        public RegistrationHouseingNeededTaskXmlImporter(IContentPolicy contentPolicy, ITaskPolicy taskPolicy,
                                                      IDictionaryManager dictionaryManager)
            : base(RegistrationHousingNeededMatches, contentPolicy, dictionaryManager, taskPolicy)
        {
        }

        protected override void ConvertTaskInfo(XElement xml, Task task)
        {
            XElement name1 = null, name2 = null, name3 = null, phone = null;

            var customer = xml.Elements().SingleOrDefault(t => t.Name.LocalName == "PhysicalPerson" || t.Name.LocalName == "SoleTrader");

            if (customer != null)
            {
                name1 = customer.Elements().SingleOrDefault(t => t.Name.LocalName == "FamilyName");
                name2 = customer.Elements().SingleOrDefault(t => t.Name.LocalName == "FirstName");
                name3 = customer.Elements().SingleOrDefault(t => t.Name.LocalName == "Patronymic");
                phone = customer.Elements().SingleOrDefault(t => t.Name.LocalName == "Phone");
            }

            task.CustomerFio =
                string.Format("{0} {1} {2}",
                              name1 != null ? name1.Value : "",
                              name2 != null ? name2.Value : "",
                              name3 != null ? name3.Value : "")
                      .Trim();
            task.CustomerPhone = phone != null ? phone.Value : null;
        }

        protected override List<XElement> GetElementsForSpecNode(List<XElement> elements, SpecNode specNode)
        {
            if (specNode.Tag == "Applicant")
            {
                var synonyms = new List<string>
                    {
                        "PhysicalPerson",
                        "SoleTrader",
                        "AgentPhysicalPerson",
                        "AgentSoleTrader"
                    };

                return elements.Where(t => synonyms.Contains(t.Name.LocalName)).Take(1).ToList();
            }

            if (specNode.Tag == "Agent")
            {
                if (elements.Any(t => t.Name.LocalName == "AgentPhysicalPerson" || t.Name.LocalName == "AgentSoleTrader"))
                {
                    return elements.Where(t => t.Name.LocalName == "PhysicalPerson").Take(1).ToList();
                }
                return new List<XElement>();
            }

            if (specNode.Tag == "IPInfo")
            {
                if (elements.Any(t => t.Name.LocalName == "OGRNIP"))
                    return new List<XElement>{elements.First().Parent};
                return new List<XElement>();
            }

            return base.GetElementsForSpecNode(elements, specNode);
        }
    }
}
