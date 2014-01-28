using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

using Common.UI.ViewModel.ValidationViewModel;

using GU.BL;
using GU.BL.Policy;
using GU.DataModel;

namespace GU.UI.ViewModel.TaskViewModel
{
    public class NewTaskVM : ValidateableVM
    {
        public NewTaskVM()
        {
            var rootAgency = GuFacade.GetDbUser().Agency;
            var agencyList = AgencyPolicy.GetActualAgencies(rootAgency).OrderBy(t => t.Name).ToList();
            AgencyList = new ListCollectionView(agencyList);
            Agency = agencyList.FirstOrDefault();
        }

        #region Binding Properties

        private Service _service;
        public Service Service
        {
            get
            {
                return _service;
            }
            set
            {
                if (_service != value)
                {
                    _service = value;
                    RaisePropertyChanged(() => Service);
                }
            }
        }

        private ListCollectionView _serviceList;
        
        public ListCollectionView ServiceList
        {
            get { return _serviceList; }
            set
            {
                if (_serviceList != value)
                {
                    _serviceList = value;
                    RaisePropertyChanged(() => ServiceList);
                }
            }
        }

        private Agency _agency;

        public Agency Agency
        {
            get
            {
                return _agency;
            }
            set
            {
                if (_agency != value)
                {
                    _agency = value;
                    RaisePropertyChanged(() => Agency);

                    var serviceList = AgencyPolicy.GetActualServices(_agency).ToList();
                    ServiceList = new ListCollectionView(serviceList);
                    ServiceList.GroupDescriptions.Add(new PropertyGroupDescription("ServiceGroup.ServiceGroupName"));

                    Service = serviceList.Count == 0 ? null : serviceList[0];
                }
            }
        }
        
        public ListCollectionView AgencyList { get; set; }

        #endregion
    }
}
