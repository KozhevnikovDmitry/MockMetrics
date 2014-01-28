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
using GU.HQ.UI.View;
using GU.HQ.UI.ViewModel;
using GU.HQ.View;
using GU.UI.View.UserView;
using GU.UI.ViewModel.UserViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.HQ.ViewModel
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
            CloseAppCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            SelectModuleCommand = new DelegateCommand(SelectModule);

            var hqManagementVm = new HqManagementVM { IsDebug = IsDebug };
            var hqRepositoryVm = new HqRepositoryVM();

            SetInteractionBehavior(hqManagementVm, hqRepositoryVm);

            this.ModuleVmList = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new HqRepositoryView { DataContext = hqRepositoryVm } }, 
                new ToolVM { View = new HqManagementView { DataContext = hqManagementVm } }
            };

            this.ModuleSelectorVmList = new List<ModuleSelectorVM>
            {
                new ModuleSelectorVM { DisplayName = "Очередь на муниципальное жильё", IconPath = "/GU.UI;component/Resources/Icons/mun.png" },
                new ModuleSelectorVM { DisplayName = "Модуль работы с заявками", IconPath = "/GU.UI;component/Resources/Icons/RZ.png" }
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
        /// <param name="hqManagementVM">Модуль работы с заявлениями</param>
        /// <param name="hqRepositoryVM">Модуль очереди на жильё</param>
        /// <remarks>
        /// В методе задаётся то, каким образом модули могут обмениваться событиями на открытие вкладок AvalonDock. 
        /// </remarks>
        private void SetInteractionBehavior(HqManagementVM hqManagementVM, HqRepositoryVM hqRepositoryVM)
        {
            hqManagementVM.AvalonInteractor.OpenEditableDockable += (s, e) => OpenEditableDockableOnModule(s, e, hqRepositoryVM);
            hqRepositoryVM.AvalonInteractor.OpenEditableDockable += (s, e) => OpenEditableDockableOnModule(s, e, hqManagementVM);
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
            int index = ModuleVmList.IndexOf(ModuleVmList.Single(t => t.View.DataContext == moduleVM));
            ActiveSelector = ModuleSelectorVmList.ElementAt(index);
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
                return _activeSelector;
            }
            set
            {
                if (_activeSelector != value)
                {
                    _activeSelector = value;
                    RaisePropertyChanged(() => ActiveSelector);
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
                return _currentModuleView;
            }
            set
            {
                if (_currentModuleView != value)
                {
                    _currentModuleView = value;
                    RaisePropertyChanged(() => CurrentModuleView);
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
                string title = "АИС УНЖ. Версия ";
                if (IsDebug)
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
            if (CurrentModuleIndex >= 0)
            {
                CurrentModuleView = ModuleVmList[CurrentModuleIndex].View;
            }
            else
            {
                CurrentModuleIndex = ModuleVmList.IndexOf(ModuleVmList.Single(t => t.View == CurrentModuleView));
                ActiveSelector = ModuleSelectorVmList.ElementAt(CurrentModuleIndex);
            }
        }

        /// <summary>
        /// Команда, закрывающая приложение.
        /// </summary>
        public DelegateCommand CloseAppCommand { get; protected set; }

        #endregion
    }
}