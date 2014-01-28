using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных эксперта в списке привлечённых к проверке
    /// </summary>
    public class InspectionExpertListItemVm : SmartListItemVm<InspectionExpert>
    {
        public override void Initialize(InspectionExpert entity)
        {
            base.Initialize(entity);
            ExpertName = Entity.Expert.ExpertState.GetName();
            ExpertWorkData = Entity.Expert.ExpertState.GetWorkdata();
            Note = Entity.ExpertName;
        }

        #region Binding Properties

        /// <summary>
        /// Строка с именем эксперта
        /// </summary>
        public string ExpertName { get; private set; }

        /// <summary>
        /// Строка с рабочей информацией сотрудника
        /// </summary>
        public string ExpertWorkData { get; private set; }

        /// <summary>
        /// Примечение к эксперту
        /// </summary>
        public string Note { get; private set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => Note);
            RaisePropertyChanged(() => ExpertWorkData);
        }
    }
}
