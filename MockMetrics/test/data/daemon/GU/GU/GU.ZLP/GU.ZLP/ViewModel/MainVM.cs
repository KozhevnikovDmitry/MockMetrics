using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

using GU.BL;
using GU.DataModel;
using GU.UI.ViewModel.TaskViewModel;
using GU.ZLP.View;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.ZLP.ViewModel
{
    public class MainVM : NotificationObject
    {
        public MainVM()
        {
            ShowRepositoryModuleCommand = new DelegateCommand(ShowRepositoryModule);
            ShowManagementModuleCommand = new DelegateCommand(ShowManagementModule);
            ShowOptionsCommand = new DelegateCommand(ShowOptions);
            CloseAppCommand = new DelegateCommand(CloseApp);
            TestCommand = new DelegateCommand(Test);

            ModuleVMs = new List<IWorkspaceVM>
            { 
                new ToolVM { View = new ZlpMainView { DataContext = new ZlpMainVM() } }, 
                new ToolVM { View = new GUMainView { DataContext = new BaseTaskManagementVM(IsDebug) } }, 
                new ToolVM { View = new MainOptionsView { DataContext = new MainOptionsVM() } }
            };
        }

        #region Binding Properties

        private List<IWorkspaceVM> _moduleVMs;
        public List<IWorkspaceVM> ModuleVMs
        {
            get
            {
                return _moduleVMs;
            }
            set
            {
                if (_moduleVMs != value)
                {
                    _moduleVMs = value;
                    RaisePropertyChanged(() => ModuleVMs);
                }
            }
        }

        private UserControl _currentModuleView;
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

        public string Title
        {
            get
            {
                var version = string.Empty;
#if DEBUG
                version = "DEBUG";
#else
                version = "RELEASE";
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                    version = ad.CurrentVersion.ToString();
                }
#endif
                return string.Format("{0} {1} -- {2}", ZlpModuleName, version, DbUser.UserText);
            }
        }

        public DbUser DbUser { get { return GuFacade.GetDbUser(); } }

        public int ZlpAgencyId
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["ZlpAppType"]); }
        }

        public string ZlpModuleName
        {
            get
            {
                if (ZlpAgencyId == 0)
                    return "Администратор";

                var zlpAgency = GuFacade.GetDictionaryManager().GetDictionaryItem<Agency>(ZlpAgencyId);
                return zlpAgency.LongName;
            }
        }

        public string ZlpIcoPath
        {
            get 
            {
                string path = "/GU.UI;component/Resources/Icons/mz.png";

                switch (ZlpAgencyId)
                {
                    case 7: // Тест Строительство
                        path = "/GU.UI;component/Resources/Icons/128X128/place.png";
                        break;
                    case 12: // Тест ЗАГС
                        path = "/GU.UI;component/Resources/Icons/zags.png";
                        break;
                    case 13: // Тест Труд и занятость
                        path = "/GU.UI;component/Resources/Icons/trud.png";
                        break;
                    case 14: // Тест Архив
                        path = "/GU.UI;component/Resources/Icons/archive.png";
                        break;
                    case 15: // Тест ЖКХ
                        path = "/GU.UI;component/Resources/Icons/zh.png";
                        break;
                    case 18: // Тест Муниципалитет
                        path = "/GU.UI;component/Resources/Icons/mun.png";
                        break;
                }

                return path;
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand ShowRepositoryModuleCommand { get; protected set; }

        public DelegateCommand ShowManagementModuleCommand { get; protected set; }

        public DelegateCommand ShowOptionsCommand { get; protected set; }

        public DelegateCommand CloseAppCommand { get; protected set; }

        public DelegateCommand TestCommand { get; protected set; }

        private void ShowRepositoryModule()
        {
            CurrentModuleView = ModuleVMs[0].View;
        }

        private void ShowManagementModule()
        {
            CurrentModuleView = ModuleVMs[1].View;
        }

        private void ShowOptions()
        {
            CurrentModuleView = ModuleVMs.Last().View;
        }

        private void CloseApp()
        {
            var main = Application.Current.MainWindow;
            main.Close();
        }

        private void Test()
        {
                       
        }

        #endregion
    }
}
