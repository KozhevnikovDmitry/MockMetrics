using Common.UI.Interface;
using Common.UI.View;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.ValidationViewModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.EmployeeViewModel
{   
    /// <summary>
    /// Класс ViewModel для редактирования сущности Сотрудник.
    /// </summary>
    public class EmployeeVm : SmartEditableVm<Employee>
    {
        private readonly IDialogUiFactory _uiFactory;

        /// <summary>
        /// Класс ViewModel для редактирования сущности Сотрудник.
        /// </summary>
        public EmployeeVm(IDialogUiFactory uiFactory, 
                          ISmartValidateableVm<Employee> employeeDataVm, 
                          ISmartListVm<DossierFile> dossierFileListVm)
        {
            EmployeeDataVm = employeeDataVm;
            DossierFileListVm = dossierFileListVm;
            _uiFactory = uiFactory;
        }

        #region EditableVM

        /// <summary>
        /// Пересобирает поля привязки.
        /// </summary>
        protected override void Rebuild()
        {
            EmployeeDataVm.Initialize(Entity);
            DossierFileListVm.Initialize(Entity.DossierFileList);
            AvalonInteractor.RegisterCaller(DossierFileListVm as IAvalonDockCaller);
        }

        protected override void Save()
        {
            var validationResults = Validate();
            if (!validationResults.IsValid)
            {
                _uiFactory.ShowToolView(new ValidationsView(), new ValidationsVM(validationResults.Errors), "Ошибочно заполненные поля");
                EmployeeDataVm.RaiseIsValidChanged();
                return;
            }

            base.Save();
            EmployeeDataVm.ReadyToValidateCommand.Execute();
        }

        #endregion

        #region Binding Properties

        public ISmartValidateableVm<Employee> EmployeeDataVm { get; private set; }

        public ISmartListVm<DossierFile> DossierFileListVm { get; private set; }

        #endregion
    }
}
