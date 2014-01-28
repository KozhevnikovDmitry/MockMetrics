using System;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения иформации о происхождении тома
    /// </summary>
    public class DossierFileSourceVm : NotificationObject, IAvalonDockCaller
    {
        private DossierFile _dossierFile;

        /// <summary>
        /// Класс ViewModel для отображения иформации о происхождении тома
        /// </summary>
        public DossierFileSourceVm(IEntityInfoVm<Task> taskInfoVm,
                                   IEntityInfoVm<LicenseHolder> holderInfoVm,
                                   IEntityInfoVm<DocumentInventory> inventoryInfoVm,
                                   IEntityInfoVm<LicenseDossier> licenseDossierInfoVm,
                                   IEntityInfoVm<Employee> employeeInfoVm)
        {
            HolderInfoVm = holderInfoVm;
            TaskInfoVm = taskInfoVm;
            InventoryInfoVm = inventoryInfoVm;
            LicenseDossierInfoVm = licenseDossierInfoVm;
            EmployeeInfoVm = employeeInfoVm;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            GoToTaskCommand = new DelegateCommand(GoToTask);
            GoToLicenseHolderCommand = new DelegateCommand(GoToLicenseHolder, () => IsLinkaged);
            GoToLicenseDossierCommand = new DelegateCommand(GoToLicenseDossier, () => IsLinkaged);
            GoToEmployeeCommand = new DelegateCommand(GoToEmployee);
            GoToInventoryCommand = new DelegateCommand(GoToInventory);
        }

        public void Initialize(DossierFile dossierFile)
        {
            _dossierFile = dossierFile;
            IsLinkaged = _dossierFile.IsLinkaged;

            _taskInfoVm.Initialize(_dossierFile.Task);
            _dossierFile.DocumentInventory.ServiceId = _dossierFile.Service.Id;
            _inventoryInfoVm.Initialize(_dossierFile.DocumentInventory);
            _employeeInfoVm.Initialize(dossierFile.Employee);

            if (IsLinkaged)
            {
                _holderInfoVm.Initialize(_dossierFile.LicenseDossier.LicenseHolder);
                _licenseDossierInfoVm.Initialize(_dossierFile.LicenseDossier);
            }
        }

        /// <summary>
        /// Флаг указывающий по завершённость процедуры привязки.
        /// </summary>
        public bool IsLinkaged { get; private set; }
        
        #region Binding Properties
        
        /// <summary>
        /// ViewModel для отображения данных заявки
        /// </summary>
        private IEntityInfoVm<Task> _taskInfoVm;

        /// <summary>
        /// Возвращает или устанаваливает ViewModel для отображения данных заявки
        /// </summary>
        public IEntityInfoVm<Task> TaskInfoVm
        {
            get
            {
                return _taskInfoVm.Entity == null ? null : _taskInfoVm;
            }
            set
            {
                if (_taskInfoVm != value)
                {
                    _taskInfoVm = value;
                    RaisePropertyChanged(() => TaskInfoVm);
                }
            }
        }

        /// <summary>
        /// ViewModel для отображения данных заявителя
        /// </summary>
        private IEntityInfoVm<LicenseHolder> _holderInfoVm;

        /// <summary>
        /// Возвращает или устанавливает ViewModel для отображения данных заявителя
        /// </summary>
        public IEntityInfoVm<LicenseHolder> HolderInfoVm
        {
            get
            {
                return _holderInfoVm.Entity == null ? null : _holderInfoVm;
            }
            set
            {
                if (_holderInfoVm != value)
                {
                    _holderInfoVm = value;
                    RaisePropertyChanged(() => HolderInfoVm);
                }
            }
        }

        /// <summary>
        /// ViewModel для отображения данных заявителя
        /// </summary>
        private IEntityInfoVm<LicenseDossier> _licenseDossierInfoVm;

        /// <summary>
        /// Возвращает или устанавливает ViewModel для отображения данных заявителя
        /// </summary>
        public IEntityInfoVm<LicenseDossier> LicenseDossierInfoVm
        {
            get
            {
                return _licenseDossierInfoVm.Entity == null ? null : _licenseDossierInfoVm;
            }
            set
            {
                if (_licenseDossierInfoVm != value)
                {
                    _licenseDossierInfoVm = value;
                    RaisePropertyChanged(() => LicenseDossierInfoVm);
                }
            }
        }

        /// <summary>
        /// ViewModel с данными ответственного сотрудника
        /// </summary>
        private IEntityInfoVm<Employee> _employeeInfoVm;

        /// <summary>
        /// Возвращает или устанавливает ViewModel с данными ответственного сотрудника
        /// </summary>
        public IEntityInfoVm<Employee> EmployeeInfoVm
        {
            get
            {
                return _employeeInfoVm.Entity == null ? null : _employeeInfoVm;
            }
            set
            {
                if (_employeeInfoVm != value)
                {
                    _employeeInfoVm = value;
                    RaisePropertyChanged(() => EmployeeInfoVm);
                }
            }
        }

        private IEntityInfoVm<DocumentInventory> _inventoryInfoVm;

        public IEntityInfoVm<DocumentInventory> InventoryInfoVm
        {
            get
            {
                return _inventoryInfoVm.Entity == null ? null : _inventoryInfoVm;
            }
            set
            {
                if (_inventoryInfoVm != value)
                {
                    _inventoryInfoVm = value;
                    this.RaisePropertyChanged(() => InventoryInfoVm);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда перехода к карточке заявки
        /// </summary>
        public DelegateCommand GoToTaskCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход на карточку заявки
        /// </summary>
        private void GoToTask()
        {
            try
            {
                var task = TaskInfoVm.TakeEntity();
                AvalonInteractor.RaiseOpenEditableDockable(task.ToString(), typeof(Task), task, true);
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

        /// <summary>
        /// Команда перехода к карточке лицензиата
        /// </summary>
        public DelegateCommand GoToLicenseHolderCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход на карточку лицензиата
        /// </summary>
        private void GoToLicenseHolder()
        {
            try
            {
                var holder = HolderInfoVm.TakeEntity();
                AvalonInteractor.RaiseOpenEditableDockable(holder.ToString(), typeof(LicenseHolder), holder);
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
        
        /// <summary>
        /// Команда перехода к карточке лицензионного дела
        /// </summary>
        public DelegateCommand GoToLicenseDossierCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход на карточку лицензионного дела
        /// </summary>
        private void GoToLicenseDossier()
        {
            try
            {
                var dossier = LicenseDossierInfoVm.TakeEntity();
                AvalonInteractor.RaiseOpenEditableDockable(dossier.ToString(), typeof(LicenseDossier), dossier);
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

        /// <summary>
        /// Команда перехода к карточке сотрудника
        /// </summary>
        public DelegateCommand GoToEmployeeCommand { get; private set; }

        /// <summary>
        /// Осуществляет переход на карточку сотрудника
        /// </summary>
        private void GoToEmployee()
        {
            try
            {
                var employee = EmployeeInfoVm.TakeEntity();
                AvalonInteractor.RaiseOpenEditableDockable(employee.ToString(), typeof(Employee), employee);
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

        public DelegateCommand GoToInventoryCommand { get; set; }

        private void GoToInventory()
        {
            try
            {
                var inventory = InventoryInfoVm.TakeEntity();
                AvalonInteractor.RaiseOpenEditableDockable(_dossierFile.DocumentInventory.ToString(), typeof(DocumentInventory), inventory);
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

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
