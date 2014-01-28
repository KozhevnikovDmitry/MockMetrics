using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.Interfaces;

using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.AcceptTask.AcceptException;
using GU.MZ.BL.DomainLogic.Linkage;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.View.TaskView;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{

    /// <summary>
    /// Класс View Model для окна принятия заявления к рассмотрению
    /// </summary>
    public class AcceptTaskVm : NotificationObject, IConfirmableVM
    {
        /// <summary>
        /// Заявка, принимаемая к рассмотрению.
        /// </summary>
        private Task _task;
        private readonly IDialogUiFactory _uiFactory;
        private readonly DossierFileBuilder _dossierFileBuilder;
        private readonly DossierFileRepository _dossierFileRepository;
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Том лицензионного дела, создаваемый для заявки.
        /// </summary>
        public DossierFile DossierFile { get; private set; }

        /// <summary>
        /// Класс View Model для окна принятия заявления к рассмотрению
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <param name="dictionaryManager"></param>
        public AcceptTaskVm(IDialogUiFactory uiFactory, 
                            DossierFileBuilder dossierFileBuilder,
                            DossierFileRepository dossierFileRepository,
                            IDictionaryManager dictionaryManager)
        {
            _uiFactory = uiFactory;
            _dossierFileBuilder = dossierFileBuilder;
            _dossierFileRepository = dossierFileRepository;
            _dictionaryManager = dictionaryManager;
            AddDocumentCommand = new DelegateCommand(AddDocument);
            EmployeeList = _dictionaryManager.GetDynamicDictionary<Employee>();
            EmployeeId = EmployeeList.First().Id;
            AddDocumentVmList = new ObservableCollection<AddDocumentVm>();
        }

        public void Initialize(Task task)
        {
            _task = task;
            _task.AcceptChanges();
            DossierFile = null;
        }

        #region Binding Properties

        /// <summary>
        /// Возвращает дату подачи заявки.
        /// </summary>
        public string TaskStamp
        {
            get
            {
                return _task.CreateDate.Value.ToLongDateString();
            }
        }

        /// <summary>
        /// Возвращает наименование услуги указанной в заявке.
        /// </summary>
        public string ServiceName
        {
            get
            {
                return _task.Service.Name;
            }
        }

        /// <summary>
        /// Вовзвращает имя заявителя.
        /// </summary>
        public string TaskHolderName
        {
            get
            {
                return _task.CustomerFio;
            }
        }

        /// <summary>
        /// Регистрационный номер описи предоставленных документов
        /// </summary>
        private int? _inventoryRegNumber;

        /// <summary>
        /// Возвращает или устанавливает регистрационный номер описи предоставленных документов
        /// </summary>
        public int? InventoryRegNumber
        {
            get
            {
                return _inventoryRegNumber;
            }
            set
            {
                if (_inventoryRegNumber != value)
                {
                    _inventoryRegNumber = value;
                    RaisePropertyChanged(() => InventoryRegNumber);
                }
            }
        }

        /// <summary>
        /// Комментарий к принятию.
        /// </summary>
        private string _acceptNotice;

        /// <summary>
        /// Возвращает или устанавливает комментарий к принятию.
        /// </summary>
        public string AcceptNotice
        {
            get
            {
                return _acceptNotice;
            }
            set
            {
                if (_acceptNotice != value)
                {
                    _acceptNotice = value;
                    RaisePropertyChanged(() => AcceptNotice);
                }
            }
        }

        /// <summary>
        /// Список сотрудников
        /// </summary>
        public List<Employee> EmployeeList { get; set; }

        /// <summary>
        /// Id выбранного сотрудника
        /// </summary>
        private int _employeeId;

        /// <summary>
        /// Возвращает или устанавливает id выбранного сотрудника
        /// </summary>
        public int EmployeeId
        {
            get
            {
                return _employeeId;
            }
            set
            {
                if (_employeeId != value)
                {
                    _employeeId = value;
                    RaisePropertyChanged(() => EmployeeId);
                }
            }
        }

        public ObservableCollection<AddDocumentVm> AddDocumentVmList { get; set; }

        #endregion

        #region Binding Commands

        public DelegateCommand AddDocumentCommand { get; set; }

        private void AddDocument()
        {
            try
            {
                var vm = new AddDocumentVm("Новый документ", 1, AddDocumentVmList);
                if (_uiFactory.ShowValidateableDialogView(new AddDocumentView(), vm, "Добавить новый документ"))
                {
                    AddDocumentVmList.Add(new AddDocumentVm(vm.Name, vm.Quantity, AddDocumentVmList));
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка",ex));
            }
        }

        #endregion

        #region IConfirmableVM
        
        /// <summary>
        /// Проводит процедуру подтверждения диалога.
        /// </summary>
        public void Confirm()
        {
            try
            {
                _dossierFileBuilder.FromTask(_task)
                                   .ToEmployee(EmployeeList.Single(t => t.Id == EmployeeId))
                                   .WithInventoryRegNumber(InventoryRegNumber)
                                   .WithAcceptedStatus(AcceptNotice);

                foreach (var addDocumentVm in AddDocumentVmList)
                {
                    _dossierFileBuilder.AddProvidedDocument(addDocumentVm.Name, addDocumentVm.Quantity);
                }

                DossierFile = _dossierFileRepository.AcceptDossierFile(_dossierFileBuilder.Create());
                IsConfirmed = true;
            }
            catch (BuildingDataNotCompleteException)
            {
                NoticeUser.ShowWarning("Необходимо присвоить регистрационный номер и назначить отвественного сотрудника");
                IsConfirmed = false;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденна ошибка", ex));
            }
        }

        /// <summary>
        /// Восстанавливает состояние после ошибки в процедуре подтверждения диалога.
        /// </summary>
        public void ResetAfterFail()
        {
            _task.RejectChanges();
        }

        /// <summary>
        /// Флаг подтвержденённости диалога. 
        /// </summary>
        public bool IsConfirmed { get; private set; }
        
        #endregion
    }
}
