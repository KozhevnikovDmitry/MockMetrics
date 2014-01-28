using System;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.View.Import;
using GU.MZ.UI.ViewModel.Import;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel
{
    /// <summary>
    /// Класс VM для View модуля работы с заявлениями.
    /// </summary>
    public class LicenseModuleVm : MzAvalonDockVm
    {
        private readonly ImportVm _importVm;

        /// <summary>
        /// Класс VM для View модуля работы с заявлениями.
        /// </summary>
        public LicenseModuleVm(IDockableUiFactory uiFactory, 
                               IEditableFacade editableFacade,
                               ImportVm importVm)
            : base(uiFactory, editableFacade)
        {
            _importVm = importVm;
            ShowDossiersCommand = new DelegateCommand(ShowDossiers);
            ShowHoldersCommand = new DelegateCommand(ShowHolders);
            ShowLicensesCommand = new DelegateCommand(ShowLicenses);
            ShowEmployeesCommand = new DelegateCommand(ShowEmployees);
            ShowExpertsCommand = new DelegateCommand(ShowExperts);
            CreateNewEmployeeCommand = new DelegateCommand(CreateNewEmployee);
            CreateNewExpertCommand = new DelegateCommand(CreateNewExpert);
            CreateNewLicenseCommand = new DelegateCommand(CreateNewLicense);
            ShowCurrentDossierFilesCommand = new DelegateCommand(ShowCurrentDossierFiles);
            ImportCommand = new DelegateCommand(Import);
        }

        #region Binding Commands

        /// <summary>
        /// Команда открытия вкладки поиска Лицензионных дел
        /// </summary>
        public DelegateCommand ShowDossiersCommand { get; private set; }

        /// <summary>
        /// Команда открыватия вкладки поиска Лицензиатов
        /// </summary>
        public DelegateCommand ShowHoldersCommand { get; private set; }

        /// <summary>
        /// Команда открытия вкладки поиска Лицензий.
        /// </summary>
        public DelegateCommand ShowLicensesCommand { get; private set; }

        /// <summary>
        /// Комадна открытия вкладки поиска Сотрудников.
        /// </summary>
        public DelegateCommand ShowEmployeesCommand { get; private set; }

        /// <summary>
        /// Команда открытия вкладки поиска Экспертов.
        /// </summary>
        public DelegateCommand ShowExpertsCommand { get; private set; }

        /// <summary>
        /// Команда открытия окна с текущими томами лицензионных дел
        /// </summary>
        public DelegateCommand ShowCurrentDossierFilesCommand { get; private set; }

        /// <summary>
        /// Команда заведения нового Сотрудника.
        /// </summary>
        public DelegateCommand CreateNewEmployeeCommand { get; private set; }

        /// <summary>
        /// Команда заведения нового эксперта.
        /// </summary>
        public DelegateCommand CreateNewExpertCommand { get; private set; }

        /// <summary>
        /// Команда заведения новой лицензии
        /// </summary>
        public DelegateCommand CreateNewLicenseCommand { get; private set; }

        public DelegateCommand ImportCommand { get; set; }

        private void Import()
        {
            try
            {
                var importView = new ImportView { DataContext = _importVm, Owner = Application.Current.MainWindow };
                importView.ShowDialog();
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
        /// Открывает окно заведения новой лицензии
        /// </summary>
        private void CreateNewLicense()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable("Новая лицензия", typeof(License), License.CreateInstance());
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
        /// Открывает окно с текущими томами лицензионных дел
        /// </summary>
        private void ShowCurrentDossierFiles()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Ведомые тома", typeof(DossierFile));
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
        /// Открывает вкладку поиска Лицензионных дел
        /// </summary>
        private void ShowDossiers()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Лицензионные дела", typeof(LicenseDossier));
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
        /// Открывает вкладку поиска Лицензиатов
        /// </summary>
        private void ShowHolders()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Лицензиаты и соискатели", typeof(LicenseHolder));
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
        /// Открывает вкладку поиска лицензий.
        /// </summary>
        private void ShowLicenses()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Реестр лицензий", typeof(License));
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
        /// Открывает вкладку поиска сотрудников.
        /// </summary>
        private void ShowEmployees()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Сотрудники", typeof(Employee));
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
        /// Открывает вкладку поиска экспертов.
        /// </summary>
        private void ShowExperts()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Эксперты", typeof(Expert));
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
        /// Открывает вкладку заведения нового сотрудника.
        /// </summary>
        private void CreateNewEmployee()
        {
            try
            {
                var employee = Employee.CreateInstance();
                employee.DbUser = DbUser.CreateInstance();
                employee.AcceptChanges();
                AvalonInteractor.RaiseOpenEditableDockable("Новый сотрудник", typeof(Employee), employee);
            }
            catch (DALException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        /// <summary>
        /// Открывает вкладку заведения нового эксперта.
        /// </summary>
        private void CreateNewExpert()
        {

            try
            {
                var expert = Expert.CreateInstance();
                expert.ExpertState = IndividualExpertState.CreateInstance();
                expert.AcceptChanges();
                AvalonInteractor.RaiseOpenEditableDockable("Новый эксперт", typeof(Expert), expert);
            }
            catch (DALException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}
