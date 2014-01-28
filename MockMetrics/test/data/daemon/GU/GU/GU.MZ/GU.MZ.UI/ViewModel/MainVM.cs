﻿using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Common.UI.ViewModel;
using Common.UI.ViewModel.AvalonInteraction.InteractionEvents;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.BL.Policy;
using GU.DataModel;
using GU.MZ.UI.View;
using GU.UI.View.UserView;
using GU.UI.ViewModel.UserViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel
{
    /// <summary>
    /// Класс VM для главного окна приложения.
    /// </summary>
    public class MainVm : NotificationObject
    {
        /// <summary>
        /// Класс VM для главного окна приложения.
        /// </summary>
        public MainVm(DbUser dbUser, TaskModuleVm taskModuleVm, LicenseModuleVm licenseModuleVm)
        {
            CloseAppCommand = new DelegateCommand(() =>
            {
                Application.Current.MainWindow.Close();
            });
            SelectModuleCommand = new DelegateCommand(SelectModule);

            SetInteractionBehavior(taskModuleVm, licenseModuleVm);

            ModuleVmList = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new LicenseModuleView { DataContext = licenseModuleVm } }, 
                new ToolVM { View = new TaskModuleView { DataContext = taskModuleVm } }, 
                new ToolVM { View = new MainOptionsView { DataContext = new MainOptionsVm() } }
            };

            ModuleSelectorVmList = new List<ModuleSelectorVM>
            {
                new ModuleSelectorVM { DisplayName = "Модуль лицензирования", IconPath = "/GU.UI;component/Resources/Icons/MZ.png" },
                new ModuleSelectorVM { DisplayName = "Модуль работы с заявлениями", IconPath = "/GU.UI;component/Resources/Icons/RZ.png" },
                new ModuleSelectorVM { DisplayName = "Настройки", IconPath = "/GU.UI;component/Resources/Icons/Settings_256x256.png" }
            };

            SetupUserAdminInterface(dbUser);
        }

        /// <summary>
        /// Задаёт интерфейс администратора по управлению пользователями.
        /// </summary>
        private void SetupUserAdminInterface(DbUser dbUser)
        {
            if (dbUser.HasRole(RoleConstants.GU_USER_ADMIN))
            {
                ModuleVmList.Insert(2, new ToolVM { View = new UserModuleView { DataContext = new UserModuleVM() } });
                ModuleSelectorVmList.Insert(2, new ModuleSelectorVM
                {
                    DisplayName = "Пользователи",
                    IconPath = "/GU.UI;component/Resources/Icons/User_256x256.png"
                });
            }
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
        /// <param name="licenseModuleVM">Модуль лицензирования</param>
        /// <remarks>
        /// В методе задаётся то, каким образом модули могут обмениваться событиями на открытие вкладок AvalonDock. 
        /// </remarks>
        private void SetInteractionBehavior(IAvalonDockVM taskModuleVM, IAvalonDockVM licenseModuleVM)
        {
            taskModuleVM.AvalonInteractor.OpenEditableDockable += (s, e) => OpenEditableDockableOnModule(s, e, licenseModuleVM);
            licenseModuleVM.AvalonInteractor.OpenEditableDockable += (s, e) => OpenEditableDockableOnModule(s, e, taskModuleVM);
        }

        /// <summary>
        /// Обрабатывает событие открытия вкладки редактирования на определённому модуле.
        /// Переключает активный модуль, чтобы отобразить вновь открытую вкладку.
        /// </summary>
        /// <param name="sender">Паблишер события</param>
        /// <param name="e">Аргументы события</param>
        /// <param name="moduleVM">Модуль, на котором небходимо открыть вкладку</param>
        private void OpenEditableDockableOnModule(object sender, OpenEditableDockableEventArgs e, IAvalonDockVM moduleVM)
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
                string title = "Министерство здравоохранения Красноярского края. Версия ";
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