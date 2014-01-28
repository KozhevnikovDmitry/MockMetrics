using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Person;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class AddInspectionOrderExpertVm : NotificationObject
    {
        public AddInspectionOrderExpertVm(IDictionaryManager dictionaryManager)
        {
            ExpertList = dictionaryManager.GetDynamicDictionary<Expert>().Where(t => t.ExpertStateType == ExpertStateType.Individual).ToList();
            Expert = ExpertList.First();
        }

        #region Binding Properties

        public Expert Expert { get; set; }

        public List<Expert> ExpertList { get; set; } 

        #endregion
    }
}
