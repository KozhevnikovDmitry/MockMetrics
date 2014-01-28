using System;
using Common.BL.DataMapping;
using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.WeakEvent.EventSubscriber;
using GU.Archive.DataModel;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.Archive.UI.ViewModel.EmployeeViewModel
{
    public class EmployeeVM : EditableVM<Employee>
    {
        public EmployeeVM(Employee entity, 
                          IDomainObjectEventSubscriber<Employee> eventSubscriber,
                          IDomainDataMapper<Employee> dataMapper,
                          bool isEditable = true)
            : base(entity, eventSubscriber, dataMapper, isEditable)
        { }

        protected override void Rebuild()
        {
            EmployeeDataVM = new EmployeeDataVM(Entity);
        }

        #region Binding Properties

        private EmployeeDataVM _employeeDataVM;

        public EmployeeDataVM EmployeeDataVM
        {
            get
            {
                return _employeeDataVM;
            }
            set
            {
                if (_employeeDataVM != value)
                {
                    _employeeDataVM = value;
                    RaisePropertyChanged(() => EmployeeDataVM);
                }
            }
        }

        #endregion

        protected override void Save()
        {
            try
            {
                var validationResult = Validate();
                if (!validationResult.IsValid)
                {
                    UIFacade.ShowToolView(new ValidationsView(),
                                          new ValidationsVM(validationResult.Errors),
                                          "Ошибочно заполненные поля");
                    return;
                }

                base.Save();
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private ValidationErrorInfo Validate()
        {
            var validationResult = new ValidationErrorInfo();

            if (Entity.AgencyId == 0)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Агентство"));

            if (Entity.Surname == string.Empty)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Фамилия"));

            if (Entity.Name == string.Empty)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Имя"));

            if (Entity.Patronymic == string.Empty)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Отчество"));

            return validationResult;
        }
    }
}
