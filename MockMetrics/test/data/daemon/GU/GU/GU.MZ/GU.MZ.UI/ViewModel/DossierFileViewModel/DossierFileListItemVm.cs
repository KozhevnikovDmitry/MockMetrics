using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewMolel для отображения элемента в списке томов лицензионного дела
    /// </summary>
    public class DossierFileListItemVm : SmartListItemVm<DossierFile>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public DossierFileListItemVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region AbstractListItemVM

        /// <summary>
        /// Инициализирует поля привязки.
        /// </summary>
        public override void Initialize(DossierFile entity)
        {
            base.Initialize(entity);
            DossierFileString = string.Format("№{0} от {1}", Item.RegNumber, Item.CreateDate.ToLongDateString());
            CurrentStepString = _dictionaryManager.GetDictionaryItem<ScenarioStep>(Item.CurrentScenarioStepId).Name;
            EmployeeString = _dictionaryManager.GetDictionaryItem<Employee>(Item.EmployeeId).ToString();
        }

        #endregion

        #region Binding Properties

        private string _dossierFileString;

        /// <summary>
        /// Строка с информацией о тома
        /// </summary>
        public string DossierFileString
        {
            get { return _dossierFileString; }
            private set
            {
                if (value != _dossierFileString)
                {
                    _dossierFileString = value;
                    RaisePropertyChanged(() => DossierFileString);
                }
            }
        }

        private string _currentStepString;

        /// <summary>
        /// Строка с информацией о текущем этапе ведения тма
        /// </summary>
        public string CurrentStepString
        {
            get { return _currentStepString; }
            private set
            {
                if (value != _currentStepString)
                {
                    _currentStepString = value;
                    RaisePropertyChanged(() => CurrentStepString);
                }
            }
        }

        private string _employeeString;

        /// <summary>
        /// Строка с информацией об ответсвенном исполнителе
        /// </summary>
        public string EmployeeString
        {
            get { return _employeeString; }
            private set
            {
                if (value != _employeeString)
                {
                    _employeeString = value;
                    RaisePropertyChanged(() => EmployeeString);
                }
            }
        }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            RaisePropertyChanged(() => CurrentStepString);
            RaisePropertyChanged(() => DossierFileString);
            RaisePropertyChanged(() => EmployeeString);
        }
    }
}
