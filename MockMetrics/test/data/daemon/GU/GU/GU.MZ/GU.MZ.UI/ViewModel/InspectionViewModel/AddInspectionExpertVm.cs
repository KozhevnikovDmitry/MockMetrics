using System.Collections.Generic;
using System.Linq;

using GU.MZ.DataModel.Person;

using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel выбора эксперта
    /// </summary>
    public class AddInspectionExpertVm : NotificationObject
    {
        /// <summary>
        /// Класс ViewModel выбора эксперта 
        /// </summary>
        /// <param name="expertList">Список экспертов</param>
        public AddInspectionExpertVm(List<Expert> expertList)
        {
            ExpertList = new Dictionary<Expert, string>();
            expertList.ForEach(t => ExpertList[t] = t.ExpertState.GetName());
            Expert = ExpertList.First().Key;
        }

        #region Binding Properties

        /// <summary>
        /// Выбранный эксперт
        /// </summary>
        private Expert _expert;

        /// <summary>
        /// Возвращает или устанавливает выбранного экспертa
        /// </summary>
        public Expert Expert
        {
            get
            {
                return _expert;
            }
            set
            {
                if (_expert != value)
                {
                    _expert = value;
                    RaisePropertyChanged(() => Expert);
                }
            }
        }

        /// <summary>
        /// Список экспертов
        /// </summary>
        public Dictionary<Expert, string> ExpertList { get; private set; }

        #endregion
    }
}
