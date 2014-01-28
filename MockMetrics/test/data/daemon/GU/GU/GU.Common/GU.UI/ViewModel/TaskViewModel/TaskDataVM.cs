using System;
using System.Linq;
using System.Windows.Data;

using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ValidationViewModel;

using GU.BL;
using GU.BL.Extensions;
using GU.BL.Policy;
using GU.BL.Reporting.Mapping;
using GU.BL.Search.SearchDomain;
using GU.BL.Search.Strategy;
using GU.DataModel;
using GU.UI.ViewModel.SearchViewModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.TaskViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения и валидации данных заявки.
    /// </summary>
    public class TaskDataVM : DomainValidateableVM<Task>, IAvalonDockCaller
    {
        /// <summary>
        /// Класс ViewModel для отображения и валидации данных заявки.
        /// </summary>
        /// <param name="entity">Объект заявка</param>
        /// <param name="domainValidator">Валидатор заявок</param>
        /// <param name="isValidateable">Флаг возможности валидации</param>
        public TaskDataVM(Task entity, IDomainValidator<Task> domainValidator, bool isValidateable)
            : base(entity, domainValidator, isValidateable)
        {
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            var rootAgency = GuFacade.GetDbUser().Agency;
            var agencyList = AgencyPolicy.GetActualAgencies(rootAgency)
                             .Where(x => GuFacade.GetTaskPolicy().CanSetAgency(x, Entity))
                             .OrderBy(t => t.Name).ToList();
            AgencyListView = new ListCollectionView(agencyList);
            ShowTaskByCustomerCommand = new DelegateCommand(this.ShowTaskByCustomer);
            PrintTaskInformationCommand = new DelegateCommand(PrintTaskInformation, CanPrintTaskInfoReport);
        }

        #region Binding Properties

        /// <summary>
        /// Возвращает Id заявки.
        /// </summary>
        public string TaskId
        {
            get
            {
                return this.Entity.Id == 0 ? string.Empty : this.Entity.Id.ToString();
            }
        }

        /// <summary>
        /// Возвращает наименование услуги.
        /// </summary>
        public string ServiceName
        {
            get
            {
                return GuFacade.GetDictionaryManager().GetDictionaryItem<Service>(this.Entity.ServiceId).Name;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает код текущего статуса заявки.
        /// </summary>
        public string CurrentState
        {
            get
            {
                return GuFacade.GetDictionaryManager().GetEnumDictionary<TaskStatusType>()[(int)this.Entity.CurrentState];
            }
        }    

        /// <summary>
        /// Возвращает или устанавливает дату подачи заявки.
        /// </summary>
        public DateTime? CreateDate
        {
            get
            {
                return this.Entity.CreateDate;
            }
            set
            {
                if (this.Entity.CreateDate != value)
                {
                    this.Entity.CreateDate = value;
                    RaisePropertyChanged(() => CreateDate);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает дату предполагаемого завершения обработки заявки.
        /// </summary>
        public DateTime? DueDate
        {
            get
            {
                return this.Entity.DueDate;
            }
            set
            {
                if (this.Entity.DueDate != value)
                {
                    this.Entity.DueDate = value;
                    RaisePropertyChanged(() => DueDate);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает дату фактического завершения обработки заявки.
        /// </summary>
        public DateTime? CloseDate
        {
            get
            {
                return this.Entity.CloseDate;
            }
            set
            {
                if (this.Entity.CloseDate != value)
                {
                    this.Entity.CloseDate = value;
                    RaisePropertyChanged(() => CloseDate);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает ФИО заявиетеля
        /// </summary>
        public string CustomerFio
        {
            get
            {
                return this.Entity.CustomerFio;
            }
            set
            {
                if (this.Entity.CustomerFio != value)
                {
                    this.Entity.CustomerFio = value;
                    RaisePropertyChanged(() => CustomerFio);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает телефон заявиетеля
        /// </summary>
        public string CustomerPhone
        {
            get
            {
                return this.Entity.CustomerPhone;
            }
            set
            {
                if (this.Entity.CustomerPhone != value)
                {
                    this.Entity.CustomerPhone = value;
                    RaisePropertyChanged(() => CustomerPhone);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает email заявиетеля
        /// </summary>
        public string CustomerEmail
        {
            get
            {
                return this.Entity.CustomerEmail;
            }
            set
            {
                if (this.Entity.CustomerEmail != value)
                {
                    this.Entity.CustomerEmail = value;
                    RaisePropertyChanged(() => CustomerEmail);
                }
            }
        }

        /// <summary>
        /// Объекьт CollectionView для отображения иерархии ведомств
        /// </summary>
        public ListCollectionView AgencyListView { get; set; }

        /// <summary>
        /// Возвращает или устанавливает Id ведомства, в которое подана заявка
        /// </summary>
        public int AgencyId
        {
            get
            {
                return this.Entity.AgencyId;
            }
            set
            {
                if (this.Entity.AgencyId != value)
                {
                    this.Entity.AgencyId = value;
                    RaisePropertyChanged(() => AgencyId);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает код авторизации для веб-интерфейса
        /// </summary>
        public string AuthCode
        {
            get
            {
                return this.Entity.AuthCode;
            }
            set
            {
                if (this.Entity.AuthCode != value)
                {
                    this.Entity.AuthCode = value;
                    RaisePropertyChanged(() => AuthCode);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает примечание
        /// </summary>
        public string Note
        {
            get
            {
                return this.Entity.Note;
            }
            set
            {
                if (this.Entity.Note != value)
                {
                    this.Entity.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        public bool IsChangeAgencyAvailable
        {
            get
            {
                return Entity.CurrentState == TaskStatusType.None
                       || Entity.CurrentState == TaskStatusType.NotFilled
                       || Entity.CurrentState == TaskStatusType.CheckupWaiting;
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда поиска заявок данного заявителя
        /// </summary>
        public DelegateCommand ShowTaskByCustomerCommand { get; private set; }

        /// <summary>
        /// Открывает окно поиска заявок по заявителю
        /// </summary>
        private void ShowTaskByCustomer()
        {
            try
            {
                ISearchVM<Task> taskSearchVM = new TaskSearchVM(GuFacade.GetTaskSearchStrategy(), GuFacade.GetDataMapper<Task>(), new SearchTask { CustomerFio = this.CustomerFio });
                taskSearchVM.SearchCommand.Execute(taskSearchVM.SearchTemplateVMs.First());
                AvalonInteractor.RaiseOpenSearchDockable("Заявки", taskSearchVM);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Unexpectable error", ex));
            }
        }

        /// <summary>
        /// Команда для открытия отчета "Сведения о заявке"
        /// </summary>
        public DelegateCommand PrintTaskInformationCommand { get; set; }

        /// <summary>
        /// Открывает отчёт "Сведения о заявке"
        /// </summary>
        private void PrintTaskInformation()
        {
            try
            {
                var report = GuFacade.GetTaskInfoReport(this.Entity);
                AvalonInteractor.RaiseOpenReportDockable("Сведения о заявке", report, false);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        /// <summary>
        /// Возвращает true, если можно открыть отчёт "Сведения о заявке"
        /// </summary>
        /// <returns></returns>
        private bool CanPrintTaskInfoReport()
        {
            if (this.Entity.CurrentState == TaskStatusType.None || this.Entity.CurrentState == TaskStatusType.NotFilled)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
