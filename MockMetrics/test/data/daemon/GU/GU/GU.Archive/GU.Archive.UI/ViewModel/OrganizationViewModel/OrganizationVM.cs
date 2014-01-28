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
using GU.BL.Policy;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.TaskViewModel;

namespace GU.Archive.UI.ViewModel.OrganizationViewModel
{
    public class OrganizationVM : EditableVM<Organization>
    {
        public OrganizationVM(Organization entity, 
                          IDomainObjectEventSubscriber<Organization> eventSubscriber, 
                          IDomainDataMapper<Organization> dataMapper,
                          bool isEditable = true)
            : base(entity, eventSubscriber, dataMapper, isEditable)
        { }

        #region EditableVM

        protected override void Rebuild()
        {
            OrganizationDataVM = new OrganizationDataVM(Entity);
        }

        #endregion

        #region Binding Properties

        private OrganizationDataVM _organizationDataVM;

        public OrganizationDataVM OrganizationDataVM
        {
            get
            {
                return _organizationDataVM;
            }
            set
            {
                if (_organizationDataVM != value)
                {
                    _organizationDataVM = value;
                    RaisePropertyChanged(() => OrganizationDataVM);
                }
            }
        }

        #endregion

        #region EditableVM

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

            if (Entity.Address == null)
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Адрес"));

            if (string.IsNullOrEmpty(Entity.FullName))
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Полное наименование"));

            if (string.IsNullOrEmpty(Entity.ShortName))
                validationResult.AddError(string.Format("Поле \"{0}\" не заполнено", "Краткое наименование"));

            return validationResult;
        }

        #endregion
    }
}
