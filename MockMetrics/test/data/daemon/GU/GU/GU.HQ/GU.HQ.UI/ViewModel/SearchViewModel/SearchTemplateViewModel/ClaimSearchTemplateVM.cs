using System;
using System.Collections.Generic;
using System.Linq;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using GU.HQ.BL;
using GU.HQ.BL.Search.SearchDomain;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.HQ.UI.ViewModel.SearchViewModel.SearchTemplateViewModel
{
    public class ClaimSearchTemplateVM : NotificationObject
    {
        public Dictionary<int, string> ListClaimStatus { get; private set; }
        public List<Agency> ListAgency { get; private set; }

        public ClaimSearchTemplateVM(SearchClaim searchClaim)
        {
            SearchClaim = searchClaim;

            if (SearchClaim.Declarer == null)
                SearchClaim.Declarer = Person.CreateInstance();

            ListClaimStatus = new Dictionary<int, string> {{0, "<не указано>"}};
            HqFacade.GetDictionaryManager()
                .GetEnumDictionary<ClaimStatusType>().ToList()
                .ForEach(t => ListClaimStatus.Add(t.Key,  t.Value));

            var rootAgency = GuFacade.GetDbUser().Agency;
            ListAgency = AgencyPolicy.GetActualAgencies(rootAgency).OrderBy(t => t.Name).ToList<Agency>();
            var noAgency = Agency.CreateInstance();
            noAgency.Id = 0;
            noAgency.Name = "<не указано>";
            ListAgency.Add(noAgency);
            ListAgency = ListAgency.OrderBy(t => t.Id).ToList();
        }

        public SearchClaim SearchClaim { get; private set; }

        /// <summary>
        /// Дата подачи заявки
        /// </summary>
        public DateTime? ClaimDate
        {
            get{ return SearchClaim.ClaimDate; }
            set
            {
                if (SearchClaim.ClaimDate == value) return;
                SearchClaim.ClaimDate = value; 
            }
        }

        /// <summary>
        /// Фамилия заявителя
        /// </summary>
        public string Sname
        {
            get
            {   
                return SearchClaim.Declarer.Sname;
            }
            set
            {
                if (SearchClaim.Declarer.Sname == value) return;
                SearchClaim.Declarer.Sname = value;
            }
        }

        /// <summary>
        /// Имя заявителя 
        /// </summary>
        public string Name
        {
            get
            {
                return SearchClaim.Declarer.Name;
            }
            set
            {
                if (SearchClaim.Declarer.Name == value) return;
                SearchClaim.Declarer.Name = value;
            }
        }

        /// <summary>
        /// Отчество заявителя
        /// </summary>
        public string Patronymic
        {
            get
            {
                return SearchClaim.Declarer.Patronymic;
            }
            set
            {
                if (SearchClaim.Declarer.Patronymic == value) return;
                SearchClaim.Declarer.Patronymic = value;
            }
        }
       
        /// <summary>
        /// Статус заявки
        /// </summary>
        public int ClaimStatusVal
        {
            get { return (int)SearchClaim.CurrentStatusTypeId; }
            set
            {
                SearchClaim.CurrentStatusTypeId = (ClaimStatusType)value;
            }
        } 
        
        /// <summary>
        /// Территориальное подразделение
        /// </summary>
        public int AgencyId
        {
            get { return SearchClaim.AgencyId; }
            set
            {
                SearchClaim.AgencyId = value;
            }
        }
    }
}
