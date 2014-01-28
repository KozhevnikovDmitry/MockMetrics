using Common.UI;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertViewModel
{
    /// <summary>
    /// Класс ViewModel предназначенный для редактирования данных Эксперта
    /// </summary>
    public class ExpertDockableVm : SmartEditableVm<Expert>
    {
        private readonly IValidateableUiFactory _uiFactory;

        /// <summary>
        /// Класс ViewModel предназначенный для редактирования данных Эксперта
        /// </summary>
        public ExpertDockableVm(IValidateableUiFactory uiFactory, 
                                ISmartValidateableVm<Expert> expertDataVm, 
                                ISmartValidateableVm<IndividualExpertState> indExpertVm, 
                                ISmartValidateableVm<JuridicalExpertState> jurExpertVm)
        {
            ExpertDataVm = expertDataVm;
            IndExpertVm = indExpertVm;
            JurExpertVm = jurExpertVm;
            _uiFactory = uiFactory;;
        }

        #region Binding Properties

        /// <summary>
        /// Флаг видимости View редактирования для физ лица
        /// </summary>
        private bool _isIndividual;

        /// <summary>
        /// Возвращает или устанаваливает Флаг видимости View редактирования для физ лица
        /// </summary>
        public bool IsIndividual
        {
            get
            {
                return _isIndividual;
            }
            set
            {
                if (_isIndividual != value)
                {
                    _isIndividual = value;
                    RaisePropertyChanged(() => IsIndividual);
                }
            }
        }

        /// <summary>
        /// Флаг видимости View редактирования для юр лица
        /// </summary>
        private bool _isJuridical;

        /// <summary>
        /// Возвращает или устанаваливает Флаг видимости View редактирования для юр лица
        /// </summary>
        public bool IsJuridical
        {
            get
            {
                return _isJuridical;
            }
            set
            {
                if (_isJuridical != value)
                {
                    _isJuridical = value;
                    RaisePropertyChanged(() => IsJuridical);
                }
            }
        }

        public ISmartValidateableVm<Expert> ExpertDataVm { get; private set; }

        public ISmartValidateableVm<IndividualExpertState> IndExpertVm { get; private set; }

        public ISmartValidateableVm<JuridicalExpertState> JurExpertVm { get; private set; }

        #endregion

        #region Overrides of EditableVM<Expert>

        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected override void Rebuild()
        {
            ExpertDataVm.Initialize(Entity);
            Entity.ExpertStateTypeChanged += ArrangeStateView;
            ArrangeStateView(Entity.ExpertStateType);
        }

        /// <summary>
        /// Создаёт VM отображения состояния.
        /// </summary>
        /// <param name="expertStateType">Тип состояния эксперта</param>
        private void ArrangeStateView(ExpertStateType expertStateType)
        {
            IsIndividual = expertStateType == ExpertStateType.Individual;
            IsJuridical = !IsIndividual;
            if (IsIndividual)
            {
                IndExpertVm.Initialize(Entity.ExpertState as IndividualExpertState);
                JurExpertVm.Initialize(JuridicalExpertState.CreateInstance());
            }
            else
            {
                JurExpertVm.Initialize(Entity.ExpertState as JuridicalExpertState);
                IndExpertVm.Initialize(IndividualExpertState.CreateInstance());
            }
        }
        
        /// <summary>
        /// Сохраняет изменения в редактируемом эксперте.
        /// </summary>
        protected override void Save()
        {
            var validationResults = Validate();
            if (!validationResults.IsValid)
            {
                _uiFactory.ShowValidationErrorsView(validationResults);
                ExpertDataVm.RaiseIsValidChanged();
                IndExpertVm.RaiseIsValidChanged();
                JurExpertVm.RaiseIsValidChanged();
                return;
            }

            base.Save();
        }

        #endregion
    }
}
