using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Common.UI.ViewModel;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using GU.MZ.CERT.UI.View;
using GU.MZ.CERT.UI.ViewModel;
using GU.MZ.CERT.View;
using GU.UI.View.UserView;
using GU.UI.ViewModel.UserViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.CERT.ViewModel
{
    /// <summary>
    /// Класс VM для главного окна приложения.
    /// </summary>
    public class MainVM : NotificationObject
    {
        /// <summary>
        /// Класс VM для главного окна приложения.
        /// </summary>
        public MainVM()
        {
            this.CloseAppCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            this.SelectModuleCommand = new DelegateCommand(this.SelectModule);

            var taskModuleVm = new TaskModuleVM { IsDebug = this.IsDebug };
            var licenseModuleVm = new CertificationModuleVM();

            this.SetInteractionBehavior(taskModuleVm, licenseModuleVm);

            this.ModuleVmList = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new CertificationModuleView { DataContext = licenseModuleVm } }, 
                new ToolVM { View = new TaskModuleView { DataContext = taskModuleVm } }
            };

            this.ModuleSelectorVmList = new List<ModuleSelectorVM>
            {
                new ModuleSelectorVM { DisplayName = "Модуль аттестации", IconPath = "/GU.UI;component/Resources/Icons/certification.png" },
                new ModuleSelectorVM { DisplayName = "Модуль работы с заявлениями", IconPath = "/GU.UI;component/Resources/Icons/RZ.png" }
            };

            if (GuFacade.GetDbUser().HasRole(RoleConstants.GU_USER_ADMIN))
            {
                ModuleVmList.Add(new ToolVM { View = new UserModuleView { DataContext = new UserModuleVM() } });
                ModuleSelectorVmList.Add(new ModuleSelectorVM
                {
                    DisplayName = "Пользователи",
                    IconPath = "/GU.UI;component/Resources/Icons/User_256x256.png"
                });
            }

            ModuleVmList.Add(new ToolVM { View = new MainOptionsView { DataContext = new MainOptionsVM() } });
            ModuleSelectorVmList.Add(new ModuleSelectorVM
            {
                DisplayName = "Настройки",
                IconPath = "/GU.UI;component/Resources/Icons/Settings_256x256.png"
            });
        }

        /// <summary>
        /// Список рабочих областей приложения
        /// </summary>
        public List<IWorkspaceVM> ModuleVmList { get; private set; }

        #region Module Interaction

        /// <summary>
        /// Настраивает схему взаимодействия между модулями приложения.
        /// </summary>
        /// <param name="taskModuleVM">Модуль работы с заявлениями</param>
        /// <param name="certificationModuleVM">Модуль лицензирования</param>
        /// <remarks>
        /// В методе задаётся то, каким образом модули могут обмениваться событиями на открытие вкладок AvalonDock. 
        /// </remarks>
        private void SetInteractionBehavior(TaskModuleVM taskModuleVM, CertificationModuleVM certificationModuleVM)
        {
            taskModuleVM.AvalonInteractor.OpenEditableDockable += (s, e) => this.OpenEditableDockableOnModule(s, e, certificationModuleVM);
            certificationModuleVM.AvalonInteractor.OpenEditableDockable += (s, e) => this.OpenEditableDockableOnModule(s, e, taskModuleVM);
        }

        /// <summary>
        /// Обрабатывает событие открытия вкладки редактирования на определённому модуле.
        /// Переключает активный модуль, чтобы отобразить вновь открытую вкладку.
        /// </summary>
        /// <param name="sender">Паблишер события</param>
        /// <param name="e">Аргументы события</param>
        /// <param name="moduleVM">Модуль, на котором небходимо открыть вкладку</param>
        private void OpenEditableDockableOnModule(object sender, OpenEditableDockableEventArgs e, BaseAvalonDockVM moduleVM)
        {
            int index = this.ModuleVmList.IndexOf(this.ModuleVmList.Single(t => t.View.DataContext == moduleVM));
            this.ActiveSelector = this.ModuleSelectorVmList.ElementAt(index);
            e.IsInterHost = false;
            moduleVM.AvalonInteractor.RaiseOpenEditableDockable(sender, e);
            moduleVM.NotifyActiveDocumentChangedCommand.Execute();
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// Индекс активного модуля
        /// </summary>
        public int CurrentModuleIndex { get; set; }

        /// <summary>
        /// Список переключателей рабочих областей
        /// </summary>
        public List<ModuleSelectorVM> ModuleSelectorVmList { get; private set; }

        /// <summary>
        /// Текущий активные переключатель рабочих областей.
        /// </summary>
        private ModuleSelectorVM _activeSelector;

        /// <summary>
        /// Вовзращает или устанавливает актичный переключатель рабочих областей приложения.
        /// </summary>
        public ModuleSelectorVM ActiveSelector
        {
            get
            {
                return this._activeSelector;
            }
            set
            {
                if (this._activeSelector != value)
                {
                    this._activeSelector = value;
                    this.RaisePropertyChanged(() => this.ActiveSelector);
                }
            }
        }

        /// <summary>
        /// Текущая рабочая область приложения
        /// </summary>
        private UserControl _currentModuleView;

        /// <summary>
        /// Вовзвращает или устанавливает текущую рабочую область приложения
        /// </summary>
        public UserControl CurrentModuleView
        {
            get
            {
                return this._currentModuleView;
            }
            set
            {
                if (this._currentModuleView != value)
                {
                    this._currentModuleView = value;
                    this.RaisePropertyChanged(() => this.CurrentModuleView);
                }
            }
        }

        /// <summary>
        /// Флаг указывающий на то, является ли сборка DEBUG-версией.
        /// </summary>
        public bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Отображаемое имя приложения.
        /// </summary>
        public string Title
        {
            get
            {
                string title = "Министерство здравоохранения Красноярского края. Версия ";
                if (this.IsDebug)
                {
                    title += "DEBUG";
                }
                else
                {
                    if (ApplicationDeployment.IsNetworkDeployed)
                    {
                        ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                        title += ad.CurrentVersion.ToString();
                    }
                    else
                    {
                        title += "RELEASE";
                    }
                }
                return title;
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда переключения модуля в активное состояние
        /// </summary>
        public DelegateCommand SelectModuleCommand { get; private set; }

        /// <summary>
        /// Выполняет переключение модуля
        /// </summary>
        private void SelectModule()
        {
            if (this.CurrentModuleIndex >= 0)
            {
                this.CurrentModuleView = this.ModuleVmList[this.CurrentModuleIndex].View;
            }
            else
            {
                this.CurrentModuleIndex = this.ModuleVmList.IndexOf(this.ModuleVmList.Single(t => t.View == this.CurrentModuleView));
                this.ActiveSelector = this.ModuleSelectorVmList.ElementAt(this.CurrentModuleIndex);
            }
        }
        
        /// <summary>
        /// Команда, закрывающая приложение.
        /// </summary>
        public DelegateCommand CloseAppCommand { get; protected set; }

        #endregion
    }
}