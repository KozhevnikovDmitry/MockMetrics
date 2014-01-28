using System.Collections.Generic;
using System.Linq;

using GU.BL.Extensions;
using GU.DataModel;

namespace GU.BL.Policy
{
    public class AgencyPolicy
    {
        public static bool IsSubAgency(Agency agency, Agency subAgency)
        {
            return subAgency.Ancestors(t => t.ParentAgency, true).Contains(agency);
        }

        public static bool IsSubAgency(Agency agency, int subAgencyId)
        {
            return agency.Descendants(t => t.ChildAgencyList, true).Select(t => t.Id).Contains(subAgencyId);
        }

        public static bool IsSubAgency(int rootAgencyId, Agency subAgency)
        {
            return subAgency.Ancestors(t => t.ParentAgency, true).Select(t => t.Id).Contains(rootAgencyId);
        }

        /// <summary>
        /// Получение списка доступных для заведения услуг для заданного ведомства
        /// </summary>
        public static IEnumerable<Service> GetActualServices(Agency rootAgency)
        {
            //доступные услуги наследуются от предков
            //ненастроенные услуги (без спецификаций) не являются доступными для заведения
            return rootAgency
                .Ancestors(t => t.ParentAgency, true)
                .SelectMany(t => t.ServiceGroupList)
                .SelectMany(t => t.ServiceList)
                //.Where(t => t.DocSpecList.Count > 0);
                .Where(t => t.Spec != null);
        }

        /// <summary>
        /// Получение списка подведомств, у которых есть доступные для заведения услуги
        /// </summary>
        public static IEnumerable<Agency> GetActualAgencies(Agency rootAgency)
        {
            return rootAgency
                .Descendants(t => t.ChildAgencyList, true)
                .Where(t => GetActualServices(t).Any());
        }
    }
}
